using AutoMapper;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Infrastructure.Shared.MultiTenancy;
using MedHelpAuthorizations.Infrastructure.Shared.Persistence.Initialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Initialization
{
    public partial class ApplicationDbSeeder
    {
        private readonly AitTenantInfo _currentTenant;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        //private readonly RoleManager<IdentityRole> _roleManager;
        //private readonly UserManager<ApplicationUser> _userManager;
        private readonly CustomSeederRunner _seederRunner;
        private readonly ILogger<ApplicationDbSeeder> _logger;
        private string _adminUserId;
        private ApplicationContext _dbContext { get; set; }
      
        public ApplicationDbSeeder(AitTenantInfo currentTenant, UserManager<ApplicationUser> userManager, CustomSeederRunner seederRunner, ILogger<ApplicationDbSeeder> logger, IMapper mapper)
        {
            _currentTenant = currentTenant;
            _userManager = userManager;
            _seederRunner = seederRunner;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SeedDatabaseAsync(ApplicationContext dbContext, CancellationToken cancellationToken)
        {
            try
            {
                _dbContext = dbContext;
                var conn = dbContext.Database.GetDbConnection()?.ConnectionString;

                // Get root user
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == "AITADMIN");
                _adminUserId = user?.Id ?? string.Empty;

                // Seed Enum Reference Data
                await SeedDbOperationEnums();
                await SeedAuthorizationStatuses();
                await SeedAdministrativeGenders();
                await SeedStates();
                await SeedTypesOfService();
                await SeedPlaceOfServiceCodes();
                await SeedAddressTypes();
                await SeedClaimStatuses();
                await SeedX12ClaimCategories();
                await SeedX12ClaimCodeTypes();
                await SeedAdjustmentTypes();
                /// test comment for merge test
                await SeedTransactionTypes();
                await SeedInputDocumentTypes();
                await SeedApplicationFeatures();
                await SeedRpaTypes();
                await SeedApiIntegrationTypes();
                await SeedApiIntegrations();
                await SeedSpecialties();
                await SeedApplicationReports();
                await SeedDefaultEmployeeRoleEnum();
                await SeedDefaultDepartments(); //AA-148
                await SeedDefaultReportCategoryEnum();// Seeder For Reports category
                await SeedDefaultReportsEnum();// Seeder For Reports
                await SeedAlphaSplitEnums();
                await SeedDefaultProviderLevels();
                //await SeedWriteOffTypeEnums();
                await SeedTypeOfServiceEnums();
                // Seed default Application Data
                await SeedDefaultRpaInsuranceGroup(); //AA-23
                await SeedRpaInsurances();
                await SeedDefaultClient();
                await SeedDocumentTypeRelated();
                await SeedDocumentTypeRelated();
                await SeedAuthStatuses();
                await SeedDefaultClientProvider();
                await SeedDefaultClientLocation();
                await SeedDefaultPatient();
                await SeedUserClients();
                await SeedDefaultAuthTypes();
                await SeedClaimStatusExceptionReasonCategories();
                await SeedClaimStatusExceptionReasonCategoryMaps();
                await SeedDefaultClientUserApplicationReports();
                await SeedDefaultClientApiIntegrationKeys();
                await SeedEmployeeRoleClaimStatusExceptionReasonCategories();
                await SeedEmployeeRoleDepartments();
                //await SeedDefaultRpaCredentialConfiguration();
                await SeedDashboardItems();
                await SeedDefaultClaimStatusTypes(); //EN-362
                await SeedClaimLineItemStatuses(); //EN-362
                await SeedX12ClaimCategoryCodeLineItemStatuses();
                await SeedX12ClaimCodesFromDataFiles();

                // Seed Questionnaire
                await SeedDefaultClientQuestionnaire();
                await SeedSourceSystemsEnums(); //EN-64

                await SeedSystemDefaultCustomReportFilters();//EN-110

                // Seed ClaimStatusCategoryMaps
                await SeedHolidays(); //EN-515

                await SeedCptCodesFromDataFilesAsync();
                await SeedApiClaimsMessageClaimLineitemStatusMap();
                await SeedX12ClaimStatusCode(); //EN-803
                await _seederRunner.RunSeedersAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
