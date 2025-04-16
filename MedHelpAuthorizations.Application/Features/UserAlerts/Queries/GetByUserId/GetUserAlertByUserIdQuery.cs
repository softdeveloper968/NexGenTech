using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetByUserId
{
    public class GetUserAlertByUserIdQuery : IRequest<PaginatedResult<GetUserAlertByUserIdResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string UserId { get; set; }
    }

    public class GetUserAlertByUserIdHandler : IRequestHandler<GetUserAlertByUserIdQuery, PaginatedResult<GetUserAlertByUserIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserAlertByUserIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<PaginatedResult<GetUserAlertByUserIdResponse>> Handle(GetUserAlertByUserIdQuery query, CancellationToken cancellationToken)
        {
            var alerts = await _unitOfWork.Repository<UserAlert>()
                .Entities
                .Where(x => x.UserId == query.UserId && !x.IsRemoved)
                .Select(x => _mapper.Map<GetUserAlertByUserIdResponse>(x))
                .ToPaginatedListAsync(query.PageNumber, query.PageSize);  
            
            return alerts;
        }
    }
}
