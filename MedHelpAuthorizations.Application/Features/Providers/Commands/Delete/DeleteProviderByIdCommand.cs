using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Providers.Commands.Delete
{
    public class DeleteProviderByIdCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    public class DeleteProviderByIdCommandHandler : IRequestHandler<DeleteProviderByIdCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IClientProviderLocationRepository _providerLocationRepository;
        public DeleteProviderByIdCommandHandler(IUnitOfWork<int> unitOfWork, IClientProviderLocationRepository providerLocationRepository)
        {
            _unitOfWork = unitOfWork;
            _providerLocationRepository = providerLocationRepository;
        }

        public async Task<Result<int>> Handle(DeleteProviderByIdCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var item = await _unitOfWork.Repository<ClientProvider>().GetByIdAsync(command.Id);
                var providerLocations = await _providerLocationRepository.GetProviderLocationMappingsByProviderId(command.Id);
                if (providerLocations.Any())
                {
                    var res = await _providerLocationRepository.DeleteProviderLocationMappings(providerLocations, cancellationToken);
                }
                await _unitOfWork.Repository<ClientProvider>().DeleteAsync(item);
				await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(item.Id, "Provider Deleted");
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}
