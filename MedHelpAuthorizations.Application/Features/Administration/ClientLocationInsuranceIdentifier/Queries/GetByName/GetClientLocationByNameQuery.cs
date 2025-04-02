using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationInsuranceIdentifier.Queries.GetByName
{
    //public class GetClientLocationInsuranceIdentifierBySearchStringQuery : IRequest<Result<GetClientLocationInsuranceIdentifierByNameResponse>>
    //{
    //    public string searchString { get; set; }
    //}

    //public class GetClientLocationByNameQueryHandler : IRequestHandler<GetClientLocationInsuranceIdentifierBySearchStringQuery, Result<GetClientLocationInsuranceIdentifierByNameResponse>>
    //{
    //    private readonly IUnitOfWork<int> _unitOfWork;
    //    private readonly IMapper _mapper;

    //    public GetClientLocationByNameQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _mapper = mapper;
    //    }

    //    public async Task<Result<GetClientLocationInsuranceIdentifierByNameResponse>> Handle(GetClientLocationInsuranceIdentifierBySearchStringQuery query, CancellationToken cancellationToken)
    //    {
    //        ///Todo: confirm from kevin, do we need to check for clientId with locationName.
    //        var clientLocation = await _unitOfWork.Repository<ClientLocation>()
    //                                              .Entities
    //                                              .FirstOrDefaultAsync(x => x.Name == query.LocationName && x.ClientId == query.ClientId);
    //        var mappedClientLocation = _mapper.Map<GetClientLocationInsuranceIdentifierByNameResponse>(clientLocation);
    //        return await Result<GetClientLocationInsuranceIdentifierByNameResponse>.SuccessAsync(mappedClientLocation);
    //    }
    //}
}
