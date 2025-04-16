using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Features.Cardholders.Queries.CardholderViewModels;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Specifications;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;

namespace MedHelpAuthorizations.Application.Features.Cardholders.Queries.GetBySearchString
{
    public class GetCardholdersBySearchStringQuery : IRequest<Result<List<CardholderViewModel>>>
    {
        public string SearchString { get; set; }

        public GetCardholdersBySearchStringQuery(string searchString)
        {
            SearchString = searchString;
        }
    }

    public class GetCardholdersSearchStringQueryHandler : IRequestHandler<GetCardholdersBySearchStringQuery, Result<List<CardholderViewModel>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private int _clientId => _currentUserService.ClientId;

        public GetCardholdersSearchStringQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<List<CardholderViewModel>>> Handle(GetCardholdersBySearchStringQuery request, CancellationToken cancellationToken)
        {
            var cardholdersSearchResult = await _unitOfWork.Repository<Cardholder>().Entities
                .Specify(new CardholderFilterSpecification(request.SearchString, _clientId))
                .ToListAsync(cancellationToken);

            var mappedcardholders = _mapper.Map<List<CardholderViewModel>>(cardholdersSearchResult);

            return await Result<List<CardholderViewModel>>.SuccessAsync(mappedcardholders);
        }
    }
}
