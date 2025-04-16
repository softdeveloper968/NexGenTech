using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static MedHelpAuthorizations.Shared.Constants.Permission.Permissions;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocations.Commands.Delete
{
    public class DeleteClientLocationCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteClientLocationCommandHandler : IRequestHandler<DeleteClientLocationCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteClientLocationCommandHandler> _localizer;
        private readonly IClientLocationSpecialityRepository _clientLocationSpecialityRepository;
       // private readonly IClientProviderLocationRepository _providerLocationRepository;

        public DeleteClientLocationCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteClientLocationCommandHandler> localizer, IClientProviderLocationRepository providerLocationRepository,
           IClientLocationSpecialityRepository clientLocationSpecialityRepository )
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _clientLocationSpecialityRepository = clientLocationSpecialityRepository;
           // _providerLocationRepository = providerLocationRepository;
        }

        public async Task<Result<int>> Handle(DeleteClientLocationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var clientLocation = await _unitOfWork.Repository<ClientLocation>().GetByIdAsync(command.Id);
                var specialityLocation = await _clientLocationSpecialityRepository.GetLocationSpecialityMappingsByLocationId(command.Id);
                if (specialityLocation.Any())
                {
                    var res = await _clientLocationSpecialityRepository.DeleteSpecialityLocationMappings(specialityLocation, cancellationToken);
                }
                await _unitOfWork.Repository<ClientLocation>().DeleteAsync(clientLocation);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(clientLocation.Id, _localizer["ClientLocation Deleted"]);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }
    }
}