using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;

namespace MedHelpAuthorizations.Application.Features.Authorizations.Commands.AddEdit
{
    public partial class AddEditAuthorizationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public int AuthTypeId { get; set; }
        [Required]
        public int PatientId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string Completeby { get; set; }
        public string AuthNumber { get; set; }
        [Required]
        public int Units { get; set; } = 1;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? DischargedOn { get; set; }
        public string DischargedBy { get; set; }
        public IList<Document> Documents { get; set; } = new List<Document>();
        public int ClientId { get; set; }
        public AuthorizationStatusEnum AuthorizationStatusId { get; set; }
        public ICollection<AuthorizationClientCptCode> AuthorizationClientCptCodes { get; set; } = new HashSet<AuthorizationClientCptCode>();

    }

    public class AddEditAuthorizationCommandHandler : IRequestHandler<AddEditAuthorizationCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditAuthorizationCommandHandler> _localizer;
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;
        public AddEditAuthorizationCommandHandler(IUnitOfWork<int> unitOfWork, IAuthorizationRepository authorizationRepository, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditAuthorizationCommandHandler> localizer, ICurrentUserService currentUserService)
        {
            _authorizationRepository = authorizationRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditAuthorizationCommand command, CancellationToken cancellationToken)
        {
            command.ClientId = _clientId;
            Authorization dbAuthorization = null;

            if (command.Id == 0)
            {
                var authorization = _mapper.Map<Authorization>(command);
                //var cptCodes = authorization.AuthorizationClientCptCodes;
                //var cptCodeIds = cptCodes.Select(c => c.Id);
                //var items = await _unitOfWork.Repository<AuthorizationClientCptCode>().Entities.Where(x => cptCodeIds.Contains(x.Id)).ToListAsync();
                //authorization.AuthorizationClientCptCodes = items;
                foreach (var item in authorization.AuthorizationClientCptCodes)
                {
                    item.ClientCptCode = null;
                    item.Id = 0;
                }
                
                dbAuthorization = await _unitOfWork.Repository<Authorization>().AddAsync(authorization);
                //dbAuthorization.AuthorizationClientCptCodes = command.AuthorizationClientCptCodes;                
                await _unitOfWork.Commit(cancellationToken);

                var expiringPreviousAuthorization = await _authorizationRepository.GetExpiringForPatientIdAndAuthType(command.PatientId, command.AuthTypeId, _clientId, null);
                if (expiringPreviousAuthorization != null)
                {
                    //If expiring auth found, add record to the ConcurrentAuthprizations Table.
                    ConcurrentAuthorization concurrentAuthorization = new ConcurrentAuthorization() { InitialAuthorizationId = expiringPreviousAuthorization.Id, SucceededAuthorizationId = authorization.Id };
                    await _unitOfWork.Repository<ConcurrentAuthorization>().AddAsync(concurrentAuthorization);
                }
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(authorization.Id, _localizer["Authorization Saved"]);
            }
            else
            {
                var authz = await _unitOfWork.Repository<Authorization>().Entities
                    .Include(x => x.InitialAuthorizations)
                    .Include(x => x.SucceededAuthorizations) 
                    .Include(x => x.AuthorizationClientCptCodes)
                    .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);
                if (authz != null)
                {
                    //delete associated ConcurrentAuthorization for SucceededAuthorization Records. If needed they will be re-added in below code. 
                    foreach (var sa in authz.SucceededAuthorizations)
                    {
                        await _unitOfWork.Repository<ConcurrentAuthorization>().DeleteAsync(sa);
                    }
                    await _unitOfWork.Commit(cancellationToken);

                    var expiringPreviousAuthorization = await _authorizationRepository.GetExpiringForPatientIdAndAuthType(command.PatientId, command.AuthTypeId, _clientId, authz.StartDate);
                    if (expiringPreviousAuthorization != null && expiringPreviousAuthorization.Id != authz.Id)
                    {
                        //If expiring auth found, add record to the ConcurrentAuthprizations Table.
                        ConcurrentAuthorization concurrentAuthorization = new ConcurrentAuthorization() { InitialAuthorizationId = expiringPreviousAuthorization.Id, SucceededAuthorizationId = authz.Id };
                        await _unitOfWork.Repository<ConcurrentAuthorization>().AddAsync(concurrentAuthorization);
                    }

                    bool statusChanged = false;
                    if (authz.CompleteDate == null && command.CompleteDate != null)
                    {
                        command.Completeby = _currentUserService.UserId;
                        if (command.AuthorizationStatusId != AuthorizationStatusEnum.AuthApproved)
                        {
                            command.AuthorizationStatusId = AuthorizationStatusEnum.AuthApproved;
                            statusChanged = true;
                        }
                    }
                    if (authz.DischargedOn == null && command.DischargedOn != null)
                    {
                        command.DischargedBy = _currentUserService.UserId;
                        if (command.AuthorizationStatusId != AuthorizationStatusEnum.AuthApproved)
                        {
                            command.AuthorizationStatusId = AuthorizationStatusEnum.AuthApproved;
                            statusChanged = true;
                        }
                    }

                    string noteContent = null;
                    if (command.CompleteDate != null && authz.CompleteDate != command.CompleteDate && statusChanged)
                    {
                        noteContent = $"User has changed CompletedDate - Status update triggered";
                    }
                    if (command.DischargedOn != null && authz.DischargedOn != command.DischargedOn && statusChanged)
                    {
                        noteContent = $"User has changed DischargedDate - Status update triggered";
                    }
                    if (noteContent != null)
                    {
                        await _unitOfWork.Repository<Note>().AddAsync(new Note()
                        {
                            AuthorizationId = command.Id,
                            NoteTs = DateTime.Now,
                            NoteContent = noteContent,
                            NoteUserId = _currentUserService.UserId,
                            ClientId = _clientId
                        });
                    }

                    _mapper.Map(command, authz);
                    foreach (var item in command.AuthorizationClientCptCodes)
                    {
                        item.ClientCptCode = null;
                        if (item.Id == 0)
                        {                            
                            await _unitOfWork.Repository<AuthorizationClientCptCode>().AddAsync(item);
                        }
                    }
                    foreach (var item in authz.AuthorizationClientCptCodes)
                    {
                        if (!command.AuthorizationClientCptCodes.Select(x => x.Id).Contains(item.Id))
                        {
                            await _unitOfWork.Repository<AuthorizationClientCptCode>().DeleteAsync(item);
                        }
                    }

                    await _unitOfWork.Repository<Authorization>().UpdateAsync(authz);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(authz.Id, _localizer["Authorization Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Authorization Not Found!"]);
                }
            }
        }
    }
}