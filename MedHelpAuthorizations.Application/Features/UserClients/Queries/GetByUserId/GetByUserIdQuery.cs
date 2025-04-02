using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using MedHelpAuthorizations.Shared.Wrapper;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.UserClients.Queries.GetByUserId
{
    public class GetByUserIdQuery : IRequest<PaginatedResult<GetByUserIdQueryResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UserId { get; set; }
        public GetByUserIdQuery(int pagenumber, int pagesize, string userId)
        {
            PageNumber = pagenumber;
            PageSize = pagesize;
            UserId = userId;                
        }
    }

    public class GetByUserIdQueryHandler : IRequestHandler<GetByUserIdQuery, PaginatedResult<GetByUserIdQueryResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;        
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public GetByUserIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUserService userService)
        {           
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PaginatedResult<GetByUserIdQueryResponse>> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _userService.GetByUserIdAsync(request);
                return response;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }   
}
