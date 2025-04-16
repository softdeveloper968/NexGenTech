using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientNotes.Queries.GetAllPagedData
{
    public class GetAllPagedClientNotesQuery : IRequest<PaginatedResult<GetAllPagedClientNotesResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public GetAllPagedClientNotesQuery(int pageNumber, int pageSize, string searchString)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
        }
    }
    public class GetAllPagedClientNotesQueryHandler : IRequestHandler<GetAllPagedClientNotesQuery, PaginatedResult<GetAllPagedClientNotesResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        private int _clientId => _currentUserService.ClientId;
        public GetAllPagedClientNotesQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }
        public async Task<PaginatedResult<GetAllPagedClientNotesResponse>> Handle(GetAllPagedClientNotesQuery request, CancellationToken cancellationToken)
        {

            var clientNotes = await _unitOfWork.Repository<ClientNote>().Entities.Where(x => x.ClientId == _clientId)
               .Specify(new ClientNoteBySearchStringSpecification(request.SearchString, _clientId))
               .Select(x => _mapper.Map<GetAllPagedClientNotesResponse>(x))
               .ToPaginatedListAsync(request.PageNumber, request.PageSize);

            if (clientNotes.Data != null)
            {
                foreach (var data in clientNotes.Data)
                {
                    var user = await _userManager.FindByIdAsync(data.CreatedBy);
                    if (user != null)
                    {
                        data.CreatedBy = $"{user.LastName},{user.FirstName}";
                        data.UserId = user.Id;
                    }
                }
            }
            return clientNotes;
        }
    }
}
