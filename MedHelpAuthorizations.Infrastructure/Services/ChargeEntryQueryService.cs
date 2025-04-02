using System;
using AutoMapper;
using MedHelpAuthorizations.Application.Extensions;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedHelpAuthorizations.Application.Specifications;
using Microsoft.EntityFrameworkCore;


namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class ChargeEntryQueryService : IChargeEntryQueryService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ChargeEntryQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;

        public ChargeEntryQueryService(IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IStringLocalizer<ChargeEntryQueryService> localizer,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            this._currentUserService = currentUserService;
        }


        //Get a list of clients/Configurations that subscribe to charge entry service
        public async Task<List<ChargeEntryRpaConfiguration>> GetUiPathChargeEntryConfigurations() {

            try
            {
                return await _unitOfWork.Repository<ChargeEntryRpaConfiguration>().Entities
                    .Specify(new ActiveChargeEntryConfigurationSpecification())
                    .Specify(new ChargeEntryConfigurationUiPathSpecification())
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
