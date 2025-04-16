using AutoMapper;
using MedHelpAuthorizations.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using MedHelpAuthorizations.Application.Interfaces.Repositories;
using MedHelpAuthorizations.Domain.Entities;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;


namespace MedHelpAuthorizations.Infrastructure.Services
{
    public class AuthorizationQueryService : IAuthorizationQueryService
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AuthorizationQueryService> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        private int _clientId => _currentUserService.ClientId;
        public AuthorizationQueryService(
            IUnitOfWork<int> unitOfWork,
            IMapper mapper,
            IStringLocalizer<AuthorizationQueryService> localizer, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
            this._currentUserService = currentUserService;
        }

        public async Task<int> GetActiveOmhcAuthorizationsCountAsync()
        {
            var activeOmhcAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.EndDate.Value.Date >= DateTime.Now.Date || x.EndDate == null) && (x.AuthType.Name.ToUpper().Contains("OMHC")));

            return activeOmhcAuthorizationsCount;
        }

        public async Task<int> GetActiveOtherAuthorizationsCountAsync()
        {
            var activeOtherAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId 
                            && (x.EndDate.Value.Date >= DateTime.Now.Date || x.EndDate == null) 
                            && (!x.AuthType.Name.ToUpper().Contains("SUD")
                                && !x.AuthType.Name.ToUpper().Contains("PRP") 
                                && !x.AuthType.Name.ToUpper().Contains("OMHC")
                                && !x.AuthType.Name.ToUpper().Contains("MH")));

            return activeOtherAuthorizationsCount;
        }

        public async Task<int> GetActivePrpAuthorizationsCountAsync()
        {
            var activePrpAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.EndDate.Value.Date >= DateTime.Now.Date || x.EndDate == null) && (x.AuthType.Name.ToUpper().Contains("PRP")));

            return activePrpAuthorizationsCount;
        }

        public async Task<int> GetActiveMhAuthorizationsCountAsync()
        {
            var activePrpAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.EndDate.Value.Date >= DateTime.Now.Date || x.EndDate == null) && (x.AuthType.Name.ToUpper().Contains("MH")));

            return activePrpAuthorizationsCount;
        }

        public async Task<int> GetActiveSudAuthorizationsCountAsync()
        {
            var activeSudAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.EndDate.Value.Date >= DateTime.Now.Date || (x.EndDate == null)) && (x.AuthType.Name.ToUpper().Contains("SUD")));

            return activeSudAuthorizationsCount;
        }

        public async Task<int> GetAllActiveAuthorizationsCountAsync()
        {
            var activeAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.EndDate.Value.Date >= DateTime.Now.Date || (x.EndDate == null)));;

            return activeAuthorizationsCount;
        }

        public async Task<int> GetAllAuthorizationsDischargedMtdCountAsync()
        {
            var dischargedAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            && (x.DischargedOn != null
                                && x.DischargedOn.Value.Year == DateTime.UtcNow.Year)
                            && (x.DischargedOn.Value.Month == DateTime.UtcNow.Month));

            return dischargedAuthorizationsCount;
        }

        public async Task<int> GetAllAuthorizationsNotCompletedCountAsync()
        {
            var completedAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && x.CompleteDate == null && x.DischargedOn == null && x.EndDate > DateTime.UtcNow);

            return completedAuthorizationsCount;
        }

        public async Task<int> GetOmhcAuthorizationsDischargedMtdCountAsync()
        {
            var dischargedOmhcAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.AuthType.Name.ToUpper().Contains("OMHC")) && (x.DischargedOn != null
                            && x.DischargedOn.Value.Year == DateTime.UtcNow.Year) && (x.DischargedOn.Value.Month == DateTime.UtcNow.Month));

            return dischargedOmhcAuthorizationsCount;
        }

        public async Task<int> GetOmhcAuthorizationsNotCompletedCountAsync()
        {
            var completedOmhcAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId && (x.AuthType.Name.ToUpper().Contains("OMHC")) && (x.CompleteDate == null && x.DischargedOn == null && x.EndDate > DateTime.UtcNow));

            return completedOmhcAuthorizationsCount;
        }

        public async Task<int> GetOtherAuthorizationsDischargedMtdCountAsync()
        {
            var dischargedOtherAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            && (!x.AuthType.Name.ToUpper().Contains("SUD")
                            && !x.AuthType.Name.ToUpper().Contains("PRP")
                            && !x.AuthType.Name.ToUpper().Contains("OMHC")
                            && !x.AuthType.Name.ToUpper().Contains("MH"))
                            && (x.DischargedOn != null
                            && x.DischargedOn.Value.Year == DateTime.UtcNow.Year)
                            && (x.DischargedOn.Value.Month == DateTime.UtcNow.Month));


            return dischargedOtherAuthorizationsCount;
        }

        public async Task<int> GetOtherAuthorizationsNotCompletedCountAsync()
        {
            var notCompletedAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            
                            && (x.CompleteDate == null && x.DischargedOn == null && x.EndDate > DateTime.UtcNow) 
                            && (!x.AuthType.Name.ToUpper().Contains("SUD")
                                && !x.AuthType.Name.ToUpper().Contains("PRP")
                                && !x.AuthType.Name.ToUpper().Contains("OMHC")
                                && !x.AuthType.Name.ToUpper().Contains("MH")));

            return notCompletedAuthorizationsCount;
        }

        public async Task<int> GetPatientsAddedMtdCountAsync()
        {
            var newPatientsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            
                            && (x.CreatedOn.Year == DateTime.UtcNow.Year)
                            && (x.CreatedOn.Month == DateTime.UtcNow.Month));

            return newPatientsCount;
        }

        public async Task<int> GetPrpAuthorizationsDischargedMtdCountAsync()
        {
            var dischargedPrpAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            
                            && (x.AuthType.Name.ToUpper().Contains("PRP"))
                            && (x.DischargedOn != null
                                && x.DischargedOn.Value.Year == DateTime.UtcNow.Year)
                            && (x.DischargedOn.Value.Month == DateTime.UtcNow.Month));

            return dischargedPrpAuthorizationsCount;
        }

        public async Task<int> GetPrpAuthorizationsNotCompletedCountAsync()
        {
            var completedAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            
                            && (x.AuthType.Name.ToUpper().Contains("PRP"))
                            && (x.CompleteDate == null && x.DischargedOn == null && x.EndDate > DateTime.UtcNow));

            return completedAuthorizationsCount;
        }

        public async Task<int> GetMhAuthorizationsDischargedMtdCountAsync()
        {
            var dischargedPrpAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId

                                 && (x.AuthType.Name.ToUpper().Contains("MH"))
                                 && (x.DischargedOn != null
                                     && x.DischargedOn.Value.Year == DateTime.UtcNow.Year)
                                 && (x.DischargedOn.Value.Month == DateTime.UtcNow.Month));

            return dischargedPrpAuthorizationsCount;
        }

        public async Task<int> GetMhAuthorizationsNotCompletedCountAsync()
        {
            var completedAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId

                                 && (x.AuthType.Name.ToUpper().Contains("MH"))
                                 && (x.CompleteDate == null && x.DischargedOn == null && x.EndDate > DateTime.UtcNow));

            return completedAuthorizationsCount;
        }

        public async Task<int> GetSudAuthorizationsDischargedMtdCountAsync()
        {
            var dischargedSudAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            
                            && (x.AuthType.Name.ToUpper().Contains("SUD"))
                            && (x.DischargedOn != null
                            && x.DischargedOn.Value.Year == DateTime.UtcNow.Year)
                            && (x.DischargedOn.Value.Month == DateTime.UtcNow.Month));


            return dischargedSudAuthorizationsCount;
        }

        public async Task<int> GetSudAuthorizationsNotCompletedCountAsync()
        {
            var completedSudAuthorizationsCount = await _unitOfWork.Repository<Authorization>().Entities
                .CountAsync(x => x.ClientId == _clientId
                            
                            && (x.AuthType.Name.ToUpper().Contains("SUD"))
                            && (x.CompleteDate == null && x.DischargedOn == null && x.EndDate > DateTime.UtcNow));

            return completedSudAuthorizationsCount;
        }

    }
}