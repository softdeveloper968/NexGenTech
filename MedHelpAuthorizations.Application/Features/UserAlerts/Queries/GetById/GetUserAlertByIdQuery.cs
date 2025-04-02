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

namespace MedHelpAuthorizations.Application.Features.UserAlerts.Queries.GetById
{
    public class GetUserAlertByIdQuery : IRequest<Result<GetUserAlertsByIdResponse>>
    {
        public int Id { get; set; }
    }

    public class GetUserAlertByIdHandler : IRequestHandler<GetUserAlertByIdQuery, Result<GetUserAlertsByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserAlertByIdHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<GetUserAlertsByIdResponse>> Handle(GetUserAlertByIdQuery query, CancellationToken cancellationToken)
        {
            var alert = await _unitOfWork.Repository<UserAlert>().GetByIdAsync(query.Id);
            var data = _mapper.Map<GetUserAlertsByIdResponse>(alert);
            return await Result<GetUserAlertsByIdResponse>.SuccessAsync(data);
        }
    }
}
