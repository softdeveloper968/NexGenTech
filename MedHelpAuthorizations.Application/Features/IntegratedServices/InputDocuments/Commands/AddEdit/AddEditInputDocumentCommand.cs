using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using MedHelpAuthorizations.Shared.Requests;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.IntegratedServices.InputDocuments.Commands.AddEdit
{
    public partial class AddEditInputDocumentCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        //public int ClientId { get; set; }

        public int ClientInsuranceId { get; set; }

        [Required]
        public InputDocumentTypeEnum? InputDocumentTypeId { get; set; }

        public int? AuthTypeId { get; set; }

        //[Required]
        //public int? RpaInsuranceId { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsPublic { get; set; } = false;
        public UploadRequest UploadRequest { get; set; }

        public string URL { get; set; }
        [Required]
        public DateTime? DocumentDate { get; set; } = DateTime.UtcNow;
        public TransactionTypeEnum TransactionTypeId { get; set; } //EN-44
        public int? ClientLocationId { get; set; }
    }

    public class AddEditInputDocumentCommandHandler : IRequestHandler<AddEditInputDocumentCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IInputDataService _inputDataService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<AddEditInputDocumentCommandHandler> _localizer;
        private readonly IInputDocumentRepository _inputDocumentRepository;
        private readonly IClaimStatusBatchClaimsRepository _claimStatusBatchClaimsRepository;
        private readonly IClientInsuranceRpaConfigurationRepository _clientInsuranceRpaConfigurationRepository;

        private int _clientId => _currentUserService.ClientId;
        public AddEditInputDocumentCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, ICurrentUserService currentUserService, IInputDataService inputDataService,
            IStringLocalizer<AddEditInputDocumentCommandHandler> localizer, IInputDocumentRepository inputDocumentRepository, IClaimStatusBatchClaimsRepository claimStatusBatchClaimsRepository, IClientInsuranceRpaConfigurationRepository clientInsuranceRpaConfigurationRepository, IBlobStorageService blobStorageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _inputDataService = inputDataService;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _inputDocumentRepository = inputDocumentRepository;
            _claimStatusBatchClaimsRepository = claimStatusBatchClaimsRepository;
            _clientInsuranceRpaConfigurationRepository = clientInsuranceRpaConfigurationRepository; //EN-44
            _blobStorageService = blobStorageService;
        }

        public async Task<Result<int>> Handle(AddEditInputDocumentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var doc = _mapper.Map<InputDocument>(command);
                doc.ClientId = _clientId;
                doc.ClientInsuranceId = null;
                ClientInsurance clientInsurance = null;
				clientInsurance = await _unitOfWork.Repository<ClientInsurance>().Entities
                                    .Include(x => x.RpaInsurance)
                                    .FirstOrDefaultAsync(x => x.Id == command.ClientInsuranceId);

				if (command.InputDocumentTypeId != InputDocumentTypeEnum.ClaimStatusWriteOff)
                {
                    if (clientInsurance == null || clientInsurance.ClientId != _currentUserService.ClientId)
                    {
                        throw (new Exception($"ClientInsurance does not reference the ClientId of the logged in User. (ClientInsuranceId: {command.ClientInsuranceId}  ClientId: {_currentUserService.ClientId})"));
                    }
                    doc.ClientInsurance = clientInsurance;
                }

                var uploadRequest = command.UploadRequest;
                if (uploadRequest != null)
                {
                    uploadRequest.FileName = $"{uploadRequest.FileName}{uploadRequest.Extension}";
                }

                var existingInputDocument = await _inputDocumentRepository.GetByFileNameAndClientIdAsync(uploadRequest.FileName);
                if (existingInputDocument != null)
                {
                    if (existingInputDocument.ByteLength == uploadRequest.Data.Length)
                    {
                        return await Result<int>.FailAsync(_localizer[$"We already have the same file in our system , Please upload a different file!"]);
                    }
                    return await Result<int>.FailAsync(_localizer[$"Claims will not be processed, Please change the file name and upload again"]);
                }


                if (uploadRequest != null)
                {
                    //EN-44
                    // If InputDocumentType is ClaimStatusInput
                    if (command.InputDocumentTypeId == InputDocumentTypeEnum.ClaimStatusInput)
                    {
                        command.TransactionTypeId = TransactionTypeEnum.ClaimStatus;

						// Either ClaimStatus is checked by Api or must have a rpaconfiguration
						var isConfigValidOrExempt
                            = clientInsurance.RpaInsurance?.ApiIntegrationId != null
                            || (await _clientInsuranceRpaConfigurationRepository
                                .GetByCriteriaAsync(_clientId, clientInsurance.RpaInsuranceId ?? 0, command.TransactionTypeId, command.AuthTypeId ?? 0, command.ClientLocationId ?? 0) != null);

                        if (!isConfigValidOrExempt)
                        {
                            return await Result<int>.FailAsync(_localizer["There is no configuration for the Input Document"]);
                        }
                        else
                        {
                            doc.URL = await _blobStorageService.UploadToBlobStorageAsync(uploadRequest).ConfigureAwait(false);
                            doc.ByteLength = uploadRequest?.Data?.Length ?? 0;
                            doc.FileName = uploadRequest?.FileName;
                            doc.ImportStatus = ImportStatusEnum.Pending;
                        }
                    }
                    else
                    {
                        doc.URL = await _blobStorageService.UploadToBlobStorageAsync(uploadRequest).ConfigureAwait(false);
                        doc.ByteLength = uploadRequest?.Data?.Length ?? 0;
                        doc.FileName = uploadRequest?.FileName;
                        doc.ImportStatus = ImportStatusEnum.Pending;
                    }
                }
                var inputDocument = await _unitOfWork.Repository<InputDocument>().AddAsync(doc);

                var requestMessages = new List<string>();
                await _unitOfWork.Commit(cancellationToken);

                requestMessages.Insert(0, _localizer[$"Input Document Saved: id = {inputDocument.Id}"]);
                //requestMessages.Insert(1, _localizer["The following claims were not imported:"]);

                return Result<int>.Success(inputDocument.Id, requestMessages);
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"Error Saving Input Document and Creating a ClaimStatus Batch. Claims will not be processed!  Msg: {ex.Message}"]);
            }
        }
    }
}