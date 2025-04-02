using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Commands.AddEdit
{
    public partial class AddEditClientLocationInsuranceIdentifierCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ClientLocationId { get; set; }

        public int ClientInsuranceId { get; set; }

        public string Identifier { get; set; }

        //public ClientLocationDto ClientLocation { get; set; }

        //public ClientInsuranceDto ClientInsurance { get; set; }

    }

    public class AddEditClientLocationInsuranceIdentifierCommandHandler : IRequestHandler<AddEditClientLocationInsuranceIdentifierCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditClientLocationInsuranceIdentifierCommandHandler> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public AddEditClientLocationInsuranceIdentifierCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService, IStringLocalizer<AddEditClientLocationInsuranceIdentifierCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AddEditClientLocationInsuranceIdentifierCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.ClientId = _clientId;
               
                //check for duplicate location/Insurance Combo
                if (command.Id == 0)
                {
                    var locationInsuranceIdentifier = await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>()
                       .Entities
                       .FirstOrDefaultAsync(l => l.ClientLocationId == command.ClientLocationId && l.ClientInsuranceId == command.ClientLocationId);

                    if (locationInsuranceIdentifier != null)
                    {
                        return await Result<int>.FailAsync(_localizer["ClientLocationInsuranceIdentifier with same keys already Exists!"]);
                    }
                    else
                    {
                        locationInsuranceIdentifier = _mapper.Map<Domain.Entities.ClientLocationInsuranceIdentifier> (command);
                        await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().AddAsync(locationInsuranceIdentifier);

                        await _unitOfWork.Commit(cancellationToken);
                        //return await Result<int>.SuccessAsync(_localizer["ClientLocation Saved"]);
                        return await Result<int>.SuccessAsync(locationInsuranceIdentifier.Id, _localizer["ClientLocationInsuranceIdentifier Saved"]);
                    }
                }
                else
                {
                    var locationInsuranceIdentifier = await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().GetByIdAsync(command.Id);
                    if (locationInsuranceIdentifier == null)
                    {
                        return await Result<int>.FailAsync(_localizer["ClientLocationInsuranceIdentifier Not Found!"]);
                    }
                    else
                    {

                        // Check for duplicate keys
                        var existingIdentifier = await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>()
                            .Entities
                            .FirstOrDefaultAsync(l => l.ClientLocationId == command.ClientLocationId && l.ClientInsuranceId == command.ClientInsuranceId);

                        if (existingIdentifier != null)
                        {
                            return await Result<int>.FailAsync(_localizer["ClientLocationInsuranceIdentifier with same keys already Exists!"]);
                        }

                        _mapper.Map(command, locationInsuranceIdentifier);
                        await _unitOfWork.Repository<Domain.Entities.ClientLocationInsuranceIdentifier>().UpdateAsync(locationInsuranceIdentifier);
                        
						await _unitOfWork.Commit(cancellationToken);
                        return await Result<int>.SuccessAsync(locationInsuranceIdentifier.Id, _localizer["ClientLocationInsuranceIdentifier Updated"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(_localizer[$"{ex.Message}--\r\n--{ex.StackTrace}"]);
            }
        }
    }
}
