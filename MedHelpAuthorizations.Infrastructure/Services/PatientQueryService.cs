using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Application.Interfaces.Services;


namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class PatientQueryService : IPatientQueryService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<PatientQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private int _clientId => _currentUserService.ClientId;


        public PatientQueryService(
            IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IStringLocalizer<PatientQueryService> localizer,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            this._currentUserService = currentUserService;
        }

        public async Task<int> GetActivePatientsCountAsync()
        {
            var activePatientsCount = await _unitOfWork.Repository<Patient>().Entities
                .CountAsync(x => x.ClientId == _clientId && x.Authorizations.Any(z => (z.EndDate.Value.Date >= DateTime.Now.Date) || (z.EndDate == null)));

            return activePatientsCount;

        }

        public async Task<int> GetPatientsAddedMtdNoBenefitCheckCountAsync()
        {
            var newPatientsCount = await _unitOfWork.Repository<Patient>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.CreatedOn.Year == DateTime.UtcNow.Year) && (x.CreatedOn.Month == DateTime.UtcNow.Month) && x.BenefitsCheckedOn == null);

            return newPatientsCount;

        }

        public async Task<int> GetPatientsAddedMtdCountAsync()
        {
            var newPatientsCount = await _unitOfWork.Repository<Patient>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.CreatedOn.Year == DateTime.UtcNow.Year) && (x.CreatedOn.Month == DateTime.UtcNow.Month));

            return newPatientsCount;
        }
    }
}