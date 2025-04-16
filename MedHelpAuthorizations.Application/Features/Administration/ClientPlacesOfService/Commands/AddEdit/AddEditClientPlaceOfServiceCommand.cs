using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientPlacesOfService.Commands.AddEdit
{
    public partial class AddEditClientPlaceOfServiceCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long? OfficePhoneNumber { get; set; }
        public long? OfficeFaxNumber { get; set; }
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public int ClientId { get; set; }

    }

    public class AddEditClientPlaceOfServiceCommandHandler : IRequestHandler<AddEditClientPlaceOfServiceCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientPlaceOfServiceCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditClientPlaceOfServiceCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<AddEditClientPlaceOfServiceCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditClientPlaceOfServiceCommand command, CancellationToken cancellationToken)
        {
            command.ClientId = _clientId;

            if (command.Id == 0)
            {
                var clientPlaceOfService = _mapper.Map<Domain.Entities.ClientPlaceOfService>(command);
                await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().AddAsync(clientPlaceOfService);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(clientPlaceOfService.Id, _localizer["ClientPlaceOfService Saved"]);
            }
            else
            {
                var clientPlaceOfService = await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().GetByIdAsync(command.Id);
                if (clientPlaceOfService != null)
                {
                    _mapper.Map(command, clientPlaceOfService);

                    await _unitOfWork.Repository<Domain.Entities.ClientPlaceOfService>().UpdateAsync(clientPlaceOfService);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(clientPlaceOfService.Id, _localizer["ClientPlaceOfService Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ClientPlaceOfService Not Found!"]);
                }
            }
        }
    }
}
