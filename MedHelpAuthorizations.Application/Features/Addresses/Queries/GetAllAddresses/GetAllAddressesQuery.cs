using MedHelpAuthorizations.Application.Interfaces.Repositories;
using AutoMapper;
using System.Threading;
using MedHelpAuthorizations.Application.Features.Addresses.ViewModels;
using MedHelpAuthorizations.Shared.Wrapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Domain.Entities;
using System.Linq.Expressions;
using System.Linq;

namespace MedHelpAuthorizations.Application.Features.Addresses.Queries.GetAllAddresses
{
    public class GetAllAddressesQuery : IRequest<PaginatedResult<GetAddressesViewModel>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllAddressesQueryHandler : IRequestHandler<GetAllAddressesQuery, PaginatedResult<GetAddressesViewModel>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        
        public GetAllAddressesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PaginatedResult<GetAddressesViewModel>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Address, GetAddressesViewModel>> expression = e => _mapper.Map<GetAddressesViewModel>(e);

            var data = await _unitOfWork.Repository<Address>().Entities
                               .Select(expression)
                               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            return data;
        }
    }
}
