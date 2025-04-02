using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientCptCodes.Queries.GetAllPaged
{
    public class GetAllClientCptCodesQuery : IRequest<PaginatedResult<GetAllPagedClientCptCodesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }

        public GetAllClientCptCodesQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }

    public class GetAllClientCptCodesQueryHandler : IRequestHandler<GetAllClientCptCodesQuery, PaginatedResult<GetAllPagedClientCptCodesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetAllClientCptCodesQueryHandler(IUnitOfWork<int> unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAllPagedClientCptCodesResponse>> Handle(GetAllClientCptCodesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<ClientCptCode, GetAllPagedClientCptCodesResponse>> expression = e => new GetAllPagedClientCptCodesResponse
            {
                Id = e.Id,
                ClientId = e.ClientId,
                LookupName = e.LookupName,
                TypeOfServiceId = e.TypeOfServiceId,
                CptCodeGroupId = e.CptCodeGroupId,
                Description = e.Description,
                ShortDescription = e.ShortDescription,
                Code = e.Code,
                CodeVersion = e.CodeVersion,
                ScheduledFee = e.ScheduledFee,
            };
            var data = await _unitOfWork.Repository<ClientCptCode>().Entities
               .Specify(new ClientCptCodesByClientIdSpecification(request.SearchString,_clientId))
               .Select(expression)               
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}