using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilalityQuery.cs;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Application.Features.Administration.ClientLocationSpeciality.Queries.GetClientLocationSpecilality
{
    public class GetClientLocationSpecilalityQuery :  IRequest<Result<List<GetClientLocationSpecilalityResponse>>>
    {
        public int LocationId { get; set; }
    }

    public class GetClientLocationSpecilalityQueryHandler : IRequestHandler<GetClientLocationSpecilalityQuery, Result<List<GetClientLocationSpecilalityResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetClientLocationSpecilalityQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<GetClientLocationSpecilalityResponse>>> Handle(GetClientLocationSpecilalityQuery query, CancellationToken cancellationToken)
        {
            try
            {
				var data = await _unitOfWork.Repository<Domain.Entities.ClientLocationSpeciality>()
			   .Entities
			   .Specify(new GetClientLocationSpecialitySpecification(query, _currentUserService.ClientId))
			   .Select(x => _mapper.Map<GetClientLocationSpecilalityResponse>(x))
			   .ToListAsync();

				return await Result<List<GetClientLocationSpecilalityResponse>>.SuccessAsync(data);
			}
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
