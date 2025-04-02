
using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Application.Models.Identity;
using MedHelpAuthorizations.Infrastructure.Persistence.Context;
using MedHelpAuthorizations.Shared.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;

namespace MedHelpAuthorizations.Infrastructure
{
	public partial class DatabaseSeeder : IDatabaseSeeder
	{
		private readonly ILogger<DatabaseSeeder> _logger;
		private readonly ApplicationContext _db;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private string adminIdResult = Guid.NewGuid().ToString();
		private string basicUserIdResult = Guid.NewGuid().ToString();

		public DatabaseSeeder(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationContext db, ILogger<DatabaseSeeder> logger)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_db = db;
			_logger = logger;
		}

		public void Initialize()
		{
			CreateHashTest();
			//SeedFromDataFiles();
			//_db.SaveChanges();

			//AddRpaInsurances();
			////AddClaimStatusResolutionIntervals();
			//_db.SaveChanges();

			////AddCustomerPermissionClaims();
			//AddAdministrator();
			//AddBasicUser();
			//AddDefaultClient();
			//IncludeClientClaims();
			//IncludePatientClaims();
			//IncludeAuthClaims();
			//IncludeAdminUtilityClaims();
			//IncludeNotesClaims();
			//IncludeReportsClaims();
			//IncludeDocumentsClaims();
			//IncludeDeveloperClaims();
			//IncludeDocumentTypeClaims();
			//IncludeClientInsuranceClaims();
			//IncludeClientCptCodeClaims();
			//IncludeProviderClaims();
			//AddDefaultClientProvider();
			//AddDefaultClientLocation();
			//AddDefaultSpecialtyEnum(); //  Seeder for specilities
			//						   //AddDefaultPatient();

			//AddDefaultAuth();
			//_db.SaveChanges();

			////AddDefaultInputDocument();
			//AddDefaultUserClient();
			//AddBasicUserClient();
			//AddAuthStatusesAndPermission();
			//AddDefaultClientQuestionnaire();
			//AddDocumentTypeRelated();
			////AddDefaultClientInsuranceRpaConfiguration();
			//AddClaimStatusExceptionReasonCategoryMaps();
			//AddPermissionForDocuments();
			//AddPermissionForQuestionnaire();
			//AddPermissionForUserAlert();
			//AddPermissionForCardholder();
			//AddPermissionForInsuranceCards();
			//AddPermissionForClaimStatus();
			//AddPermissionForChargeEntry();
			//AddPermissionForInputDocuments();
			//AddPermissionForIntegratedServicess();
			//AddPermissionForClientLocations();
			//AddPermissionForEmployees();
			//AddDefaultEmployeeRoleEnum(); //
			//AddDefaultDepartmentEnum(); //AA-148
			//AddDefaultReportCategoryEnum();// Seeder For Reports category
			//AddDefaultReportsEnum();// Seeder For Reports
			//AddAlphaSplitEnums();
			//SeedClaimLineItemStatuses(); //AA-69
			//SeedEmployeeRoleClaimStatusExceptionReasonCategories();
			//SeedEmployeeRoleDepartments();

   //         AddDefaultRpaInsuranceGroup(); //AA-23
   //         AddDefaultRpaCredentialConfiguration(); //AA-23
   //         SeedDashboardItems(); //AA-284
			//AddDefaultRpaInsuranceGroup(); //AA-23
			//AddDefaultRpaCredentialConfiguration(); //AA-23
			//SeedWriteOffTypeEnum(); //AA-231


			//_db.SaveChanges();

		}

		private string CreateHashTest()
		{ //0x85839953A0ACB314A8905BE653B8BC0A   //  0x26F85C49C507233C9EC97A076486B454
		  //var hash = HashHelpers.CreateClaimSql0xMD5("EWALDT", "PAUL", new DateTime(1970,06,10), "107497.00", "99214", null, new DateTime(2021,10,01));
			var hash = HashHelpers.CreateClaimSql0xMD5(582, "H0015", null, new DateTime(2022, 07, 24));
			//Should = 0x54ABB2BD746D67345AD5D562A943B76F
			return hash;
		}

		//private void AddCustomerPermissionClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		if (adminRoleInDb != null)
		//		{
		//			await _roleManager.AddCustomPermissionClaim(adminRoleInDb, "Permissions.Communication.Chat");
		//		}
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddAdministrator()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if Role Exists
		//		var adminRole = new IdentityRole(RoleConstants.AdministratorRole);
		//		var adminRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		if (adminRoleInDb == null)
		//		{
		//			await _roleManager.CreateAsync(adminRole);
		//			_logger.LogInformation("Seeded Administrator Role.");
		//		}
		//		//Check if User Exists
		//		var superUser = new ApplicationUser
		//		{
		//			Id = adminIdResult,
		//			FirstName = "MedHelp",
		//			LastName = "Administrator",
		//			Email = "administrator@medhelpinc.com",
		//			UserName = "administrator",
		//			EmailConfirmed = true,
		//			PhoneNumberConfirmed = true,
		//			CreatedOn = DateTime.Now,
		//			IsActive = true
		//		};
		//		var superUserInDb = await _userManager.FindByEmailAsync(superUser.Email);
		//		if (superUserInDb == null)
		//		{
		//			await _userManager.CreateAsync(superUser, UserConstants.DefaultPassword);
		//			var result = await _userManager.AddToRoleAsync(superUser, RoleConstants.AdministratorRole);
		//			if (result.Succeeded)
		//			{
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Authorizations);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Users);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Roles);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Patients);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Providers);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.AdminUtilities);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Notes);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Documents);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.DocumentTypes);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Questionnaires);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Reports);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Developer);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ClientInsurances);
		//				await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ClientCptCodes);
		//			}
		//			_logger.LogInformation("Seeded User with Administrator Role.");
		//		}
		//		else
		//		{
		//			adminIdResult = superUserInDb.Id;
		//		}
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddBasicUser()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if Role Exists
		//		var basicRole = new IdentityRole(RoleConstants.BasicRole);
		//		var basicRoleInDb = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		if (basicRoleInDb == null)
		//		{
		//			await _roleManager.CreateAsync(basicRole);
		//			_logger.LogInformation("Seeded Basic Role.");
		//		}
		//		//Check if User Exists
		//		var basicUser = new ApplicationUser
		//		{
		//			Id = basicUserIdResult,
		//			FirstName = "John",
		//			LastName = "Doe",
		//			Email = "basicuser@medhelpinc.com",
		//			UserName = "johndoe",
		//			EmailConfirmed = true,
		//			PhoneNumberConfirmed = true,
		//			CreatedOn = DateTime.Now,
		//			IsActive = true
		//		};
		//		var basicUserInDb = await _userManager.FindByEmailAsync(basicUser.Email);
		//		if (basicUserInDb == null)
		//		{
		//			await _userManager.CreateAsync(basicUser, UserConstants.DefaultPassword);
		//			await _userManager.AddToRoleAsync(basicUser, RoleConstants.BasicRole);
		//			_logger.LogInformation("Seeded User with Basic Role.");
		//		}
		//		else
		//		{
		//			basicUserIdResult = basicUserInDb.Id;
		//		}

		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludePatientClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Patients);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.Patients);

		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeClientClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Clients);
		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeAdminUtilityClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.AdminUtilities);
		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeNotesClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Notes);
		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeAuthClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Authorizations);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.Authorizations);
		//	}).GetAwaiter().GetResult();
		//}
		//private void IncludeReportsClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Reports);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.Reports);
		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeDocumentsClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Documents);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.Documents);
		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeDocumentTypeClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.DocumentTypes);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.DocumentTypes);
		//	}).GetAwaiter().GetResult();
		//}

		//private void IncludeDeveloperClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Developer);

		//		//var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		//await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.Reports);
		//	}).GetAwaiter().GetResult();
		//}
		//private void IncludeClientInsuranceClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ClientInsurances);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.ClientInsurances);
		//	}).GetAwaiter().GetResult();
		//}
		//private void IncludeClientCptCodeClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ClientCptCodes);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.ClientCptCodes);
		//	}).GetAwaiter().GetResult();
		//}
		//private void IncludeProviderClaims()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Providers);

		//		var basicRole = await _roleManager.FindByNameAsync(RoleConstants.BasicRole);
		//		await _roleManager.GeneratePermissionClaimByModule(basicRole, PermissionModules.Providers);
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddDefaultClient()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if patient Exists
		//		var defaultClient = await _db.Clients.Where(c => c.ClientCode == "Client123").FirstOrDefaultAsync();
		//		if (defaultClient == null)
		//		{
		//			ICollection<ClientAuthType> clientAuthTypes = new List<ClientAuthType>();

		//			var newClient = new Domain.Entities.Client()
		//			{
		//				Name = "MedHelp Test Client Number 1",
		//				ClientCode = "Client123",
		//				ClientAuthTypes = clientAuthTypes
		//			};
		//			_db.Clients.Add(newClient);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Client");
		//}

		//private void AddDefaultClientProvider()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if patient Exists
		//		var defaultProvider = await _db.ClientProviders.Where(c => c.Npi == "0123456789").FirstOrDefaultAsync();
		//		if (defaultProvider == null)
		//		{
		//			var newProvider = new ClientProvider()
		//			{
		//				Person = new Person() { FirstName = "Default", LastName = "Provider1", DateOfBirth = new DateTime(1976, 04, 21), Email = "default@provider1.com", MobilePhoneNumber = 1112223333, OfficePhoneNumber = 9998887777 },
		//				ClientId = 1,
		//				ExternalId = "123456789AIT",
		//				Npi = "0123456789",
		//				TaxId = "999887777",
		//				SpecialtyId = SpecialtyEnum.InternalMedicine,
		//				Upin = "A12345"
		//			};
		//			_db.ClientProviders.Add(newProvider);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Provider");
		//}

		////AA-77 for seeding clienetLocations
		//private void AddDefaultClientLocation()
		//{
		//	Task.Run(async () =>
		//	{
		//		//check if clientLocations exists
		//		var defaultLocation = await _db.ClientLocations.Where(l => l.Name == "Default").FirstOrDefaultAsync();
		//		if (defaultLocation == null)
		//		{
		//			var newLocation = new ClientLocation()
		//			{
		//				Name = "Default",
		//				OfficePhoneNumber = 7897897897,
		//				OfficeFaxNumber = 7897897897,
		//				//AddressId = 1,
		//				ClientId = 1,
		//			};
		//			_db.ClientLocations.Add(newLocation);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Cliet location");
		//}

		//private void AddDefaultPatient()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if patient Exists
		//		var defaultPatient = await _db.Patients.FirstOrDefaultAsync(x => x.Person.FirstName.ToLower() == "kevin");
		//		if (defaultPatient == null)
		//		{
		//			ICollection<ClientAuthType> clientAuthTypes = new List<ClientAuthType>();

		//			var newPatient = new Domain.Entities.Patient()
		//			{
		//				Person = new Person()
		//				{
		//					FirstName = "Kevin",
		//					LastName = "McInitialPerson",
		//					DateOfBirth = new DateTime(1976, 04, 21),
		//					GenderIdentityId = GenderIdentityEnum.Male,
		//					SocialSecurityNumber = SocialSecurityNumberExtensions.EncryptSSN("123456789"), //AA-218
		//					ClientId = 1
		//				},
		//				ExternalId = "1000001",
		//				ClientInsurance = null,
		//				InsurancePolicyNumber = "PolicyNumberForECSID001",
		//				InsuranceGroupNumber = "GroupNumberFor001",
		//				//ClientId = 1
		//			};
		//			_db.Patients.Add(newPatient);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded patient");
		//}

		//private void AddDefaultUserClient()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if user client Exists
		//		var defaultUserClient = await _db.UserClients.FirstOrDefaultAsync(x => x.UserId.ToLower() == this.adminIdResult);
		//		if (defaultUserClient != null)
		//		{
		//			this.adminIdResult = defaultUserClient.UserId;
		//		}
		//		else
		//		{
		//			var newUserClient = new Domain.Entities.UserClient()
		//			{
		//				UserId = this.adminIdResult,
		//				ClientId = 1
		//			};
		//			_db.UserClients.Add(newUserClient);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded user client");
		//}

		//private void AddBasicUserClient()
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if patient Exists
		//		//TODO- Store in the UserClient Table as UserId instead of username. 
		//		var basicUserClient = await _db.UserClients.FirstOrDefaultAsync(x => x.UserId.ToLower() == this.basicUserIdResult);
		//		if (basicUserClient != null)
		//		{
		//			this.basicUserIdResult = basicUserClient.UserId;
		//		}
		//		else
		//		{
		//			var newUserClient = new UserClient()
		//			{
		//				UserId = this.basicUserIdResult,
		//				ClientId = 1
		//			};
		//			_db.UserClients.Add(newUserClient);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded basic user client");
		//}
		//private void AddRpaInsurances()
		//{
		//	List<RpaInsurance> rpaInsurances = new List<RpaInsurance>()
		//	{
		//		new RpaInsurance("Ambetter", "Ambetter"),
		//		new RpaInsurance("Amerigroup", "Amerigroup"),
		//		new RpaInsurance("Carefirst BCBS", "Carefirst"),
		//		new RpaInsurance("CollabMD", "CollabMD"),
		//		new RpaInsurance("DentaQuest", "DentaQuest"),
		//		new RpaInsurance("Optum", "Optum"),
		//		new RpaInsurance("Maryland Physcians Care", "MPC"),
		//		new RpaInsurance("United Healthcare", "UHC"),
		//		new RpaInsurance("Payspan", "Payspan"),
		//		new RpaInsurance("Novitas CMS", "Novitas"),
		//		new RpaInsurance("Maryland Medicaid", "MedicaidMD"),
		//		new RpaInsurance("Cigna", "Cigna"),
		//		new RpaInsurance("Aetna", "Aetna"),
		//		new RpaInsurance("Aetna Better Health", "AetnaBH"),
		//		new RpaInsurance("Carefirst Michigan", "CarefirstMI"),
		//		new RpaInsurance("MedStar Family Choice MCO", "MedStar"),
		//		new RpaInsurance("Aetna Medicaid", "AetnaMC"),
		//		new RpaInsurance("Connex Medicare Part A", "ConnexA"),
		//		new RpaInsurance("Healthy Blue", "HealthyBlue"),
		//		new RpaInsurance("DentaQuest Michigan", "DentaQuestMI"),
		//	};
		//	Task.Run(async () =>
		//	{
		//		foreach (var ins in rpaInsurances)
		//		{
		//			if (!_db.RpaInsurances.Any(ri => ri.Code == ins.Code))
		//			{
		//				_db.RpaInsurances.Add(ins);
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded RpaInsurances");
		//}

		////private void AddClaimStatusResolutionIntervals()
		////{
		////    List<ClaimStatusResolutionInterval> claimStatusResolutionIntervals = new List<ClaimStatusResolutionInterval>()
		////    {
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Paid, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Approved, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 3, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Denied, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.ClaimNotFound, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 2, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.MemberNotFound, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Pended, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 3, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Received, DaysWaitBetweenAttempts = 2, MaximumPipelineDays = 7, MinimumResolutionAttempts = 4, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Error, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 7, MinimumResolutionAttempts = 4, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Ignored, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.NotAdjudicated, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 4, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Voided, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Rejected, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.UnMatchedProcedureCode, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 2, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.UnMatchedProcedureCode, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 2, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.Unknown, DaysWaitBetweenAttempts = 1, MaximumPipelineDays = 7, MinimumResolutionAttempts = 4, SubmissionDateWaitPeriod = 3 },
		////        new ClaimStatusResolutionInterval() { ClaimLineItemStatusId = ClaimLineItemStatusEnum.ZeroPay, DaysWaitBetweenAttempts = 0, MaximumPipelineDays = 0, MinimumResolutionAttempts = 0, SubmissionDateWaitPeriod = 0 },
		////    };
		////    Task.Run(async () =>
		////    {
		////        foreach (var itvl in claimStatusResolutionIntervals)
		////        {
		////            if (!_db.ClaimStatusResolutionIntervals.Any(i => i.ClaimLineItemStatusId == itvl.ClaimLineItemStatusId))
		////            {
		////                _db.ClaimStatusResolutionIntervals.Add(itvl);
		////                var claimLineItemStatus = _db.ClaimLineItemStatuses.Where(i => i.ClaimLineItemStatusId == itvl.ClaimLineItemStatusId).FirstOrDefault();
		////                if(claimLineItemStatus != null)
		////                {
		////                    claimLineItemStatus.ClaimStatusResolutionInterval = itvl;
		////                }
		////            }
		////        }
		////    }).GetAwaiter().GetResult();
		////    _logger.LogInformation("Seeded ClaimStatusResolutionIntervals");
		////}

		//private void AddDefaultRpaInsuranceGroup() //AA-23
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if config Exists
		//		if (!_db.RpaInsuranceGroups.Any())
		//		{
		//			var defaultRpaInsuranceGroup = new RpaInsuranceGroup()
		//			{
		//				Name = "Availity",
		//				DefaultTargetUrl = "https://apps.availity.com/availity/web/public.elegant.login"
		//			};

		//			_db.RpaInsuranceGroups.Add(defaultRpaInsuranceGroup);
		//			_db.SaveChanges();
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded Default RpaInsuranceGroups");
		//}

		//private void AddDefaultRpaCredentialConfiguration() //AA-23
		//{
		//	Task.Run(async () =>
		//	{
		//		//Check if config Exists
		//		if (!_db.ClientRpaCredentialConfigurations.Any())
		//		{
		//			var defaultClientRpaCredentialConfiguration = new ClientRpaCredentialConfiguration()
		//			{
		//				RpaInsuranceGroupId = 1,
		//				Username = "defaultConfigUsername",
		//				Password = "defaultConfigPassword",
		//				ReportFailureToEmail = "defaultReportFailureToEmail",
		//				IsCredentialInUse = false,
		//				UseOffHoursOnly = false,
		//			};

		//			_db.ClientRpaCredentialConfigurations.Add(defaultClientRpaCredentialConfiguration);
		//			_db.SaveChanges();
		//		}
		//	}).GetAwaiter().GetResult();

		//	_logger.LogInformation("Seeded Default ClientRpaCredentialConfigurations");
		//}

		////private void AddDefaultClientInsuranceRpaConfiguration()
		////{
		////    Task.Run(async () =>
		////    {
		////        //Check if config Exists
		////        if (!_db.ClientInsuranceRpaConfigurations.Any())
		////        {
		////            var defaultClientInsuranceRpaConfiguration = new ClientInsuranceRpaConfiguration(1, TransactionTypeEnum.ClaimStatus, 1)
		////            {
		////                IsDeleted = false,
		////                ClientRpaCredentialConfigurationId = 1
		////                //TargetUrl = "http://automatedintegrationtechnologies.com",
		////                //Username = "defaultConfigUsername",
		////                //Password = "defaultConfigPassword"
		////            };

		////            _db.ClientInsuranceRpaConfigurations.Add(defaultClientInsuranceRpaConfiguration);
		////        }
		////    }).GetAwaiter().GetResult();
		////    _logger.LogInformation("Seeded Default ClientInsuranceRpaConfiguration");
		////}

		////private void AddDefaultInputDocument()
		////{
		////    Task.Run(async () =>
		////    {
		////        //Check if claim status an Input Document Exists for clientId = 1
		////        if (!(await _db.InputDocuments.AnyAsync(x => x.InputDocumentTypeId == InputDocumentTypeEnum.ClaimStatusInput)))
		////        {
		////            var defaultInputDocument = new InputDocument(InputDocumentTypeEnum.ClaimStatusInput.GetDescription(), InputDocumentTypeEnum.ClaimStatusInput)
		////            {
		////                ClientInsuranceId = 1,
		////                URL = "/placeholderUrl",
		////                InputDocumentTypeId = InputDocumentTypeEnum.ClaimStatusInput,
		////                Title = "PlaceholderTitle",
		////                IsPublic = false,
		////                Description = $"Default {InputDocumentTypeEnum.ClaimStatusInput.GetDescription()}",
		////                DocumentDate = DateTime.UtcNow,
		////                IsDeleted = false,
		////            };

		////            _db.InputDocuments.Add(defaultInputDocument);
		////        }
		////    }).GetAwaiter().GetResult();
		////    _logger.LogInformation("Seeded Default Input Documents");
		////}

		//private void AddDefaultAuth()
		//{
		//	Task.Run(async () =>
		//	{
		//		string[] names = new[] { "PT50", "PT20", "PT54", "PRP", "OMHC", "MH-IOP", "MH-PHP", "DENTAL", "FQHC", "PHTHPY", "OPHTH", "TYPE32", "ACT", "SCMP", "SupEmp" };
		//		string[] descriptions = new[] { "PT50 - SUD IOP", "PT20 - SUD Doctors office", "PT54 - Residential",
		//										  "PRP - Psychiatric Rehabilitation Program", "OMHC - Outpatient Mental Health Clinic",
		//										  "Mental Health - IOP","MH-PHP Mental Health - Partial Hospitalization Program","Dental",
		//										  "Federal Qualified Health Center", "Physical Therapy", "Ophthalmology", "Opioid Treatment Center",
		//										  "Assertive Community Treatment", "Specialized Case Management Program", "Supportive Employment Vocational Services" };

		//		for (int i = 0; i < names.Length; i++)
		//		{
		//			//add new AuthType if authtype does not exist                
		//			if (!_db.AuthTypes.Any(x => x.Name == names[i]))
		//			{
		//				var newAuth = new Domain.Entities.AuthType()
		//				{
		//					Name = names[i],
		//					Description = descriptions[i]
		//				};
		//				_db.AuthTypes.Add(newAuth);
		//				_db.ClientAuthTypes.Add(new Domain.Entities.ClientAuthType
		//				{
		//					ClientId = 1,
		//					AuthType = newAuth
		//				});
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Auth");
		//}

		//private void AddAuthStatusesAndPermission()
		//{
		//	Task.Run(async () =>
		//	{
		//		if (!await _db.AuthorizationStatuses.AnyAsync())
		//		{
		//			foreach (var enumValue in Enum.GetValues<AuthorizationStatusEnum>())
		//			{
		//				_db.AuthorizationStatuses.Add(new AuthorizationStatus
		//				{
		//					Id = enumValue,
		//					Name = enumValue.ToString(),
		//					Description = enumValue.GetDescription()
		//				});
		//			}
		//			foreach (var auth in _db.Authorizations.Where(x => x.AuthorizationStatusId == null))
		//			{
		//				auth.AuthorizationStatusId = AuthorizationStatusEnum.ClientRequestAdded;
		//			}

		//			var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//			await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.AuthStatuses);
		//		}

		//	}).GetAwaiter().GetResult();
		//}

		//private void AddDefaultClientQuestionnaire()
		//{
		//	Task.Run(async () =>
		//	{
		//		ClientQuestionnaire clientQuestionnaire = new ClientQuestionnaire();

		//		if (!await _db.ClientQuestionnaires.AnyAsync())
		//		{
		//			clientQuestionnaire.Description = "Default clientQuestionnaire for clients in the State of Maryland.";
		//			clientQuestionnaire.Name = "Default ClientQuestionnaire";
		//			clientQuestionnaire.RelatedState = StateEnum.MD;
		//			clientQuestionnaire.ClientId = 1;
		//			await _db.ClientQuestionnaires.AddAsync(clientQuestionnaire);
		//		}

		//		foreach (var enumValue in Enum.GetValues<QuestionCategoryEnum>())
		//		{
		//			//Check if QuestionCategory Exists            
		//			if (!await _db.QuestionCategories.AnyAsync(x => x.Id == enumValue))
		//			{
		//				QuestionCategory qc = new QuestionCategory();
		//				qc.Id = enumValue;
		//				qc.Name = enumValue.ToString();
		//				qc.Description = enumValue.GetDescription();
		//				await _db.QuestionCategories.AddAsync(qc);
		//			}
		//		}

		//		if (!await _db.ClientQuestionnaireCategories.AnyAsync())
		//		{
		//			ClientQuestionnaireCategory cqc1 = new ClientQuestionnaireCategory() { QuestionCategoryId = QuestionCategoryEnum.PatientGeneral, ClientQuestionnaireId = clientQuestionnaire.Id, ClientQuestionnaire = clientQuestionnaire, CategoryOrder = 0, CreatedOn = DateTime.UtcNow };
		//			await _db.ClientQuestionnaireCategories.AddAsync(cqc1);
		//			AddQuestionsForCategory(cqc1);

		//			var cqc2 = new ClientQuestionnaireCategory() { QuestionCategoryId = QuestionCategoryEnum.PatientSubstanceUse, ClientQuestionnaireId = clientQuestionnaire.Id, ClientQuestionnaire = clientQuestionnaire, CategoryOrder = 1, CreatedOn = DateTime.UtcNow };
		//			await _db.ClientQuestionnaireCategories.AddAsync(cqc2);
		//			AddQuestionsForCategory(cqc2);
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default ClientQuestionnaire");
		//}

		//private void AddQuestionsForCategory(ClientQuestionnaireCategory category)
		//{
		//	Task.Run(async () =>
		//	{

		//		//Check if question Exist for Question Category
		//		if (!await _db.ClientQuestionnaireCategoryQuestions.AnyAsync(x => x.ClientQuestionnaireCategory == category))
		//		{
		//			//Add Questions for Question Category
		//			switch (category.QuestionCategoryId)
		//			{
		//				case QuestionCategoryEnum.PatientGeneral:
		//					//await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					//{

		//					//    ClientQuestionnaireCategory = category,
		//					//    CategoryQuestionOrder = 0,
		//					//    QuestionContent = "I choose not to participate in data collection",
		//					//    CreatedOn = DateTime.UtcNow
		//					//});
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 0,
		//						QuestionContent = "I am filling this form at the time of individual's auth.",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//							{
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//							}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 2,
		//						QuestionContent = "Legal status at admission?",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//							{
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "INVOL", IsDefaultAnswer = false },
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "VOL", IsDefaultAnswer = false },
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//							}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 3,
		//						QuestionContent = "Source of referral?",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//								{
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Mental Health Therapist", IsDefaultAnswer = false },
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Self Referral,", IsDefaultAnswer = false },
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Criminal (Court)", IsDefaultAnswer = false },
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//								}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 4,
		//						QuestionContent = "Education Level (Highest Level Completed)?",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//									{
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "K", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "1-5", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "6-8", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "9-12", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "College", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//									}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 5,
		//						QuestionContent = "Is the individual deaf or hard of hearing?",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//							{
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//							}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 6,
		//						QuestionContent =
		//							"Is the individual blind or is having serious difficulty seeing even when wearing glasses?",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//							{
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//								new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//							}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 7,
		//						QuestionContent = "Because of a Physical, Mental, or Emotional Condition, is the Individual having Serious Difficulty Concentrating, Remembering, or Making Decisions? (5 years old or older)",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//								{
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//								}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 8,
		//						QuestionContent =
		//								"Is the individual having serious difficulty walking or climbing stairs (5 years old or older)?",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//								{
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//									new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//								}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 9,
		//						QuestionContent =
		//								"Is the individual having difficulty dressing or bathing (5 years or older)",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//									{
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//									}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 10,
		//						QuestionContent =
		//								"Because of a Physical, Mental, or Emotional Condition, is the Individual Having Serious Difficulty doing Errands Alone such as Visiting a Doctor's Office or Shopping? (15 years old or older)",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//									{
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//									}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 11,
		//						QuestionContent = "Was the individual screened for gambling",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//									{
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//										new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//									}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 12,
		//						QuestionContent = "Number of times in self-help support group in the past 30 days",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//										{
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "0", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "1-5", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "6-10", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "11-15", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//										}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 12,
		//						QuestionContent = "Number of Arrests in the Past 30 Days",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//										{
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "0", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "1-5", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "6-10", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "11-15", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//										}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 13,
		//						QuestionContent = "Number of dependent children",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//											{
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "0", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "1-5", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "6-10", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//											}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 14,
		//						QuestionContent = "Primary source of income",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//											{
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "SSDI,", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "SSI", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Employment", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Family", IsDefaultAnswer = false },
		//												new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//											}
		//					}); ;
		//					break;

		//				case QuestionCategoryEnum.PatientSubstanceUse:
		//					//    await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					//    {
		//					//        ClientQuestionnaireCategory = category,
		//					//        CategoryQuestionOrder = 15,
		//					//        QuestionContent = "Please confirm individual's substance use history",
		//					//        CreatedOn = DateTime.UtcNow
		//					//    });
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 16,
		//						QuestionContent = "Expected source of payment",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//												{
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Medicaid,", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//												}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 17,
		//						QuestionContent = "Psych problem in addition to alcohol or drug",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//										{
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Yes", IsDefaultAnswer = false },
		//											new ClientQuestionnaireCategoryQuestionOption(){ Answer = "No", IsDefaultAnswer = false }
		//										}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 18,
		//						QuestionContent = "Primary substance of use",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//												{
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Opioids", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Amphetamines", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Methamphetamines", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Crack/Cocaine", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Benzos", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Alcohol", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Other", IsDefaultAnswer = true }
		//												}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 19,
		//						QuestionContent = "Age at first use",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//												{
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "0-10", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "11-20", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "21-40", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "41-60", IsDefaultAnswer = false },
		//													new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Unknown", IsDefaultAnswer = true }
		//												}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 20,
		//						QuestionContent = "Route of administration",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//													{
		//														new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Oral", IsDefaultAnswer = false },
		//														new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Inhalation", IsDefaultAnswer = false },
		//														new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Smoking", IsDefaultAnswer = false },
		//														new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Injection", IsDefaultAnswer = false }
		//													}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 21,
		//						QuestionContent = "Frequency of use",
		//						CreatedOn = DateTime.UtcNow,
		//						ClientQuestionnaireCategoryQuestionOptions = new List<ClientQuestionnaireCategoryQuestionOption>()
		//														{
		//															new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Daily", IsDefaultAnswer = false },
		//															new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Intermittenly", IsDefaultAnswer = false },
		//															new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Weekly", IsDefaultAnswer = false },
		//															new ClientQuestionnaireCategoryQuestionOption(){ Answer = "Monthly", IsDefaultAnswer = false }
		//														}
		//					}); ;
		//					await _db.ClientQuestionnaireCategoryQuestions.AddAsync(new ClientQuestionnaireCategoryQuestion()
		//					{
		//						ClientQuestionnaireCategory = category,
		//						CategoryQuestionOrder = 22,
		//						QuestionContent = "Date last used",
		//						CreatedOn = DateTime.UtcNow
		//					});
		//					break;

		//				case QuestionCategoryEnum.PatientGambling:
		//					break;

		//				default:
		//					break;
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Questionnaire");
		//}

		//private void AddOptionsForQuestionnaireCategoryQuestions(ClientQuestionnaireCategoryQuestion question)
		//{

		//}
		//private void AddDocumentTypeRelated()
		//{
		//	Task.Run(async () =>
		//	{
		//		var defaultclient = await _db.Clients.Include(x => x.DocumentTypes).FirstOrDefaultAsync(x => x.Id == 1);
		//		if (!_db.DocumentTypes.Any())
		//		{
		//			//add the default types
		//			var defaultTypes = DocumentTypeConstants.GetDefaults();
		//			foreach (var type in defaultTypes)
		//			{
		//				var doctype = new DocumentType() { Name = type.Item1, Description = type.Item2, IsDefault = true };
		//				_db.DocumentTypes.Add(doctype);
		//			}
		//		}
		//		if (defaultclient.DocumentTypes.Count == 0)
		//		{
		//			var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//			await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.DocumentTypes);
		//			var defaultTypes = DocumentTypeConstants.GetDefaults();
		//			var docs = _db.DocumentTypes.Where(x => defaultTypes.Select(x => x.Item1).Contains(x.Name));
		//			foreach (var doc in docs)
		//			{
		//				defaultclient?.DocumentTypes.Add(doc);
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default DocumentTypes");
		//}

		//private void AddPermissionForDocuments()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Documents);
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddPermissionForQuestionnaire()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Questionnaires);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForUserAlert()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.UserAlerts);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForCardholder()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Cardholders);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForClaimStatus()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ClaimStatus);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForChargeEntry()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ChargeEntry);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForInputDocuments()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.InputDocuments);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForIntegratedServicess()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.IntegratedServices);
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddPermissionForInsuranceCards()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.InsuranceCards);
		//	}).GetAwaiter().GetResult();
		//}
		//private void AddPermissionForClientLocations()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ClientLocations);
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddPermissionForEmployees()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.Employees);
		//	}).GetAwaiter().GetResult();
		//}

		//private void AddPermissionForComparisonDashboard()
		//{
		//	Task.Run(async () =>
		//	{
		//		var adminRole = await _roleManager.FindByNameAsync(RoleConstants.AdministratorRole);
		//		await _roleManager.GeneratePermissionClaimByModule(adminRole, PermissionModules.ComparisonDashboard);
		//	}).GetAwaiter().GetResult();
		//}

		////private void AddDefaultOptionsForQuestionnaire()
		////{
		////    Task.Run(async () =>
		////    {
		////        if (await _db.ClientQuestionnaireCategoryQuestions.AnyAsync(x => x.ClientQuestionnaireCategoryQuestionOptions.Count == 0))
		////        {
		////            var noOptions = _db.ClientQuestionnaireCategoryQuestions.Where(x => x.ClientQuestionnaireCategoryQuestionOptions.Count == 0);
		////            foreach (var record in noOptions)
		////            {
		////                record.ClientQuestionnaireCategoryQuestionOptions.Add(new ClientQuestionnaireCategoryQuestionOption()
		////                {
		////                   Answer = "DefaultAnswer",
		////                   IsDefaultAnswer = true,
		////                   ClientQuestionnaireCategoryQuestionId = record.Id
		////                });
		////            }

		////        }
		////        else if (await _db.ClientQuestionnaireCategoryQuestions.AnyAsync(x => x.ClientQuestionnaireCategoryQuestionOptions.Any(y => y.IsDefaultAnswer && y.Answer == "")))
		////        {
		////            var emptyOptions = _db.ClientQuestionnaireCategoryQuestions.Where(x => x.ClientQuestionnaireCategoryQuestionOptions.All(y => y.IsDefaultAnswer && y.Answer == "")).SelectMany(y => y.ClientQuestionnaireCategoryQuestionOptions);
		////            foreach (var record in emptyOptions)
		////            {
		////                record.Answer = "DefaultAnswer";
		////            }

		////        }
		////    }).GetAwaiter().GetResult();
		////}

		//private void SeedFromDataFiles()
		//{
		//	//Task.Run(async () =>
		//	//{
		//	if (_db.CptCodes.Any())
		//		return;

		//	//var resourceNames = this.GetType().Assembly.GetManifestResourceNames();
		//	Assembly assembly = Assembly.GetExecutingAssembly();
		//	string resourceName = "MedHelpAuthorizations.Infrastructure.SeedData.HCPC2020_ANWEB_w_disclaimer.csv";
		//	using (Stream stream = assembly.GetManifestResourceStream(resourceName))
		//	{
		//		CsvConfiguration csvConfig = new(CultureInfo.InvariantCulture) { Delimiter = ",", IgnoreBlankLines = true, HasHeaderRecord = true, MissingFieldFound = null };

		//		using (var reader = new StreamReader(stream))
		//		using (var csvReader = new CsvReader(reader, csvConfig))
		//		{
		//			try
		//			{
		//				csvReader.Read();
		//				csvReader.ReadHeader();

		//				// Store all content inside a new List as objetcs
		//				var records = csvReader.GetRecords<CptCode>().ToList();

		//				// Loop through the List and show them in Console
		//				foreach (var record in records)
		//				{
		//					_ = _db.CptCodes.Add(record);
		//				}
		//			}
		//			catch (CsvHelper.HeaderValidationException exception)
		//			{
		//				Console.WriteLine(exception);
		//			}
		//		}
		//	}
		//	//}).GetAwaiter().GetResult();
		//}

		///// <summary>
		///// 
		///// </summary>
		//private void AddAlphaSplitEnums()
		//{
		//	Task.Run(async () =>
		//	{
		//		if (!await _db.AlphaSplits.AnyAsync())
		//		{
		//			foreach (var enumValue in Enum.GetValues<AlphaSplitEnum>())
		//			{
		//				if (enumValue == AlphaSplitEnum.CustomRange)
		//				{
		//					_db.AlphaSplits.Add(new AlphaSplit
		//					{
		//						Id = enumValue,
		//						Name = enumValue.ToString(),
		//						Description = enumValue.GetDescription(),
		//						Code = enumValue.ToString(),
		//						BeginAlpha = null,
		//						EndAlpha = null
		//					});
		//				}
		//				if (enumValue == AlphaSplitEnum.AtoG)
		//				{
		//					_db.AlphaSplits.Add(new AlphaSplit
		//					{
		//						Id = enumValue,
		//						Name = enumValue.ToString(),
		//						Description = enumValue.GetDescription(),
		//						Code = enumValue.ToString(),
		//						BeginAlpha = "A",
		//						EndAlpha = "Z"
		//					});
		//				}
		//				if (enumValue == AlphaSplitEnum.HtoL)
		//				{
		//					_db.AlphaSplits.Add(new AlphaSplit
		//					{
		//						Id = enumValue,
		//						Name = enumValue.ToString(),
		//						Description = enumValue.GetDescription(),
		//						Code = enumValue.ToString(),
		//						BeginAlpha = "H",
		//						EndAlpha = "L"
		//					});
		//				}
		//				if (enumValue == AlphaSplitEnum.MtoR)
		//				{
		//					_db.AlphaSplits.Add(new AlphaSplit
		//					{
		//						Id = enumValue,
		//						Name = enumValue.ToString(),
		//						Description = enumValue.GetDescription(),
		//						Code = enumValue.ToString(),
		//						BeginAlpha = "M",
		//						EndAlpha = "R"
		//					});
		//				}
		//				if (enumValue == AlphaSplitEnum.StoZ)
		//				{
		//					_db.AlphaSplits.Add(new AlphaSplit
		//					{
		//						Id = enumValue,
		//						Name = enumValue.ToString(),
		//						Description = enumValue.GetDescription(),
		//						Code = enumValue.ToString(),
		//						BeginAlpha = "S",
		//						EndAlpha = "Z"
		//					});
		//				}
		//			}
		//		}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default EmployeeRoles");
		//}

		///// <summary>
		///// 
		///// </summary>
		//private void AddDefaultEmployeeRoleEnum()
		//{
		//	Task.Run(async () =>
		//	{
		//		if (!await _db.EmployeeRoles.AnyAsync())
		//		{
		//			foreach (var enumValue in Enum.GetValues<EmployeeRoleEnum>())
		//			{
		//				var employeeLevel = EmployeeLevelEnum.NonManagementLevel;
		//				if (enumValue.ToString().ToUpper().Contains("MANAGER") || enumValue.ToString().ToUpper().Contains("DIRECTOR"))
		//					employeeLevel = EmployeeLevelEnum.ManagerLevel;
		//				else if (enumValue.ToString().ToUpper().Contains("SUPERVISOR"))
		//					employeeLevel = EmployeeLevelEnum.SupervisorLevel;
		//				else if (enumValue.ToString().Length == 3 || enumValue.ToString().ToUpper().Contains("PRESIDENT"))
		//					employeeLevel = EmployeeLevelEnum.Executive;

		//				_db.EmployeeRoles.Add(new EmployeeRole
		//				{
		//					Id = enumValue,
		//					Name = enumValue.ToString(),
		//					Description = enumValue.GetDescription(),
		//					EmployeeLevel = employeeLevel
		//				});
		//			}
		//		}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default EmployeeRoles");
		//}
		//private void AddDefaultSpecialtyEnum()
		//{
		//	Task.Run(async () =>
		//	{
		//		if (!await _db.Specialties.AnyAsync())
		//		{
		//			foreach (var enumValue in Enum.GetValues<SpecialtyEnum>())
		//			{
		//				_db.Specialties.Add(new Specialty
		//				{
		//					Id = enumValue,
		//					Name = enumValue.ToString(),
		//					Description = enumValue.GetDescription(),
		//					Code = string.Empty
		//				});
		//			}
		//		}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Specilites");
		//}

		//private void AddDefaultDepartmentEnum()
		//{
		//	Task.Run(async () =>
		//	{
		//		//if (!await _db.Departments.AnyAsync())
		//		//{

		//		foreach (var enumValue in Enum.GetValues<DepartmentEnum>())
		//		{
		//			if (!await _db.Departments.AnyAsync(x => x.Id == enumValue))
		//			{
		//				//_db.Departments.Add(new Department
		//				//{
		//				//    Id = enumValue,
		//				//    Name = enumValue.ToString(),
		//				//    Description = enumValue.GetDescription()
		//				//});
		//				//_db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Departments] ON");
		//				var sqlString = $"INSERT INTO [dbo].[Departments] (Name, Description, CreatedOn, CreatedBy) VALUES ('{enumValue.ToString()}', '{enumValue.GetDescription()}', GETDATE(), 'DataBaseSeeder')";
		//				_db.Database.ExecuteSqlRaw(sqlString);

		//				//await _db.SaveChangesAsync();
		//			}
		//		}
		//		_db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Departments] OFF");

		//		//}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Department");
		//}
		//private void AddDefaultReportsEnum()
		//{
		//	Task.Run(async () =>
		//	{
		//		foreach (var enumValue in Enum.GetValues<ReportsEnum>())
		//		{
		//			//If already exist
		//			//if get reports by Id (enum)==null then add
		//			var reportExist = _db.Report.FirstOrDefault(z => z.Id == enumValue) ?? null;
		//			if (reportExist is null)
		//			{
		//				_db.Report.Add(new Report
		//				{
		//					Id = enumValue,
		//					Name = enumValue.ToString(),
		//					Description = enumValue.GetDescription(),
		//					Code = enumValue.GetDescription(),
		//					ReportCategoryId = GetReportCategory(enumValue)
		//				});
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default Reports");
		//}
		//private void AddDefaultReportCategoryEnum()
		//{
		//	Task.Run(async () =>
		//	{
		//		foreach (var enumValue in Enum.GetValues<ReportCategoryEnum>())
		//		{
		//			var reportcategoryExist = _db.ReportCategories.FirstOrDefault(z => z.Id == enumValue) ?? null;
		//			if (reportcategoryExist is null)
		//			{
		//				_db.ReportCategories.Add(new ReportCategories
		//				{
		//					Id = enumValue,
		//					Name = enumValue.ToString(),
		//					Description = enumValue.GetDescription(),
		//					Code = enumValue.GetDescription(),
		//				});
		//			}
		//		}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default ReportCategories");
		//}

		//private void SeedClaimLineItemStatuses()
		//{
		//	List<ClaimLineItemStatus> claimLineItemStatuses = new List<ClaimLineItemStatus>()
		//	{
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Unknown, "Unknown", "Unknown", 0, 10, 30, 100, 1),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Paid, "Paid", "Paid", 0, 0, 0, 0, 21),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Approved, "Approved", "Approved", 1, 14, 4, 14, 16),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Rejected, "Rejected", "Rejected", 0, 0, 0, 0, 8),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Voided, "Voided", "Voided", 0, 0, 0, 0, 9),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Received, "Received", "Received", 2, 14, 4, 14, 14),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.NotAdjudicated, "NotAdjudicated", "Not-Adjudicated", 2, 14, 4, 14, 10),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Denied, "Denied", "Denied", 10, 60, 3, 6, 15),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Pended, "Pended", "Pended", 2, 14, 4, 14, 13),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.UnMatchedProcedureCode, "UnMatchedProcedureCode", "UnMatched-ProcedureCode", 0, 14, 2, 100, 7),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Error, "Error", "Error / Exception", 0, 10, 4, 20, 3),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.ClaimNotFound, "ClaimNotFound", "Claim Not Found", 0, 90, 4, 100, 6),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.MemberNotFound, "MemberNotFound", "Member Not Found", 2, 14, 2, 100, 5),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Ignored, "Ignored", "Ignored", 0, 0, 0, 0, 4),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.ZeroPay, "ZeroPay", "Zero Pay", 0, 0, 0, 0, 20),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.BundledFqhc, "BundledFqhc", "Bundled Fqhc", 0, 0, 0, 0, 19),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.NeedsReview, "NeedsReview", "Needs Review", 0, 0, 0, 0, 11),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.TransientError, "TransientError", "Transient Error", 0, 99, 10, 99, 2),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.CallPayer, "CallPayer", "Call Payer", 0, 0, 0, 0, 12),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Returned, "Returned", "Returned", 0, 0, 0, 0, 17),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Writeoff, "Writeoff", "Write-off", 0, 0, 0, 0, 18),
		//		new ClaimLineItemStatus(ClaimLineItemStatusEnum.Rebilled, "Rebilled", "Rebilled", 1, 14, 4, 14, 4),

		//	};
		//	Task.Run(async () =>
		//	{
		//		foreach (var status in claimLineItemStatuses)
		//		{
		//			if (!_db.ClaimLineItemStatuses.Any(a => a.Id == status.Id))
		//			{
		//				_db.ClaimLineItemStatuses.Add(status);
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded ClaimLineItemStatuses");
		//}

		//private void SeedEmployeeRoleClaimStatusExceptionReasonCategories()
		//{
		//	Task.Run(async () =>
		//	{
		//		foreach (var enumValue in Enum.GetValues<EmployeeRoleEnum>())
		//		{
		//			var roleAssociatedExceptionReasonCategories = GetClaimStatusExceptionReasonCategoryEnumsByRoleId(enumValue);
		//			foreach (var erc in roleAssociatedExceptionReasonCategories)
		//			{
		//				var EmployeeRoleClaimStatusExceptionReasonCategoryExist = _db.EmployeeRoleClaimStatusExceptionReasonCategories.FirstOrDefault(z => z.EmployeeRoleId == enumValue && z.ClaimStatusExceptionReasonCategoryId == erc) ?? null;
		//				if (EmployeeRoleClaimStatusExceptionReasonCategoryExist is null)
		//				{
		//					_db.EmployeeRoleClaimStatusExceptionReasonCategories.Add(new EmployeeRoleClaimStatusExceptionReasonCategory
		//					{
		//						EmployeeRoleId = enumValue,
		//						ClaimStatusExceptionReasonCategoryId = erc
		//					});
		//				}

		//			}

		//		}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default ReportCategories");
		//}

		//private static List<ClaimStatusExceptionReasonCategoryEnum> GetClaimStatusExceptionReasonCategoryEnumsByRoleId(EmployeeRoleEnum role)
		//{
		//	switch (role)
		//	{
		//		case EmployeeRoleEnum.RegistrationManager:
		//			return ReadOnlyObjects.RegistrationManagerExceptionReasonCategorEnums.ToList();
		//		case EmployeeRoleEnum.BillingManager:
		//			return ReadOnlyObjects.BillingManagerExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.Registor:
		//			return ReadOnlyObjects.RegistorExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.MedicalAssistance:
		//			return ReadOnlyObjects.MedicalAssistanceExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.CEO:
		//			return ReadOnlyObjects.CEOExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.CFO:
		//			return ReadOnlyObjects.CFOExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.COO:
		//			return ReadOnlyObjects.COOExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.CIO:
		//			return ReadOnlyObjects.CIOExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.DirectorOfPatientFinancialServices:
		//			return ReadOnlyObjects.DOPFSExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.VicePresident:
		//			return ReadOnlyObjects.VicePresidentExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.MedicalDirector:
		//			return ReadOnlyObjects.MedicalDirectorExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.BillingSupervisor:
		//			return ReadOnlyObjects.BillingSupervisorExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.CashPostingManager:
		//			return ReadOnlyObjects.CashPostingManagerExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.Biller:
		//			return ReadOnlyObjects.BillerExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.CashPoster:
		//			return ReadOnlyObjects.CashPosterExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.ChargeEnrty:
		//			return ReadOnlyObjects.ChargeEnrtyExceptionReasonCategoryEnum.ToList();
		//		case EmployeeRoleEnum.InsuranceContractor:
		//			return ReadOnlyObjects.InsuranceContractorExceptionReasonCategoryEnum.ToList();
		//		default:
		//			return new List<ClaimStatusExceptionReasonCategoryEnum>();
		//	}
		//}

		//private ReportCategoryEnum GetReportCategory(ReportsEnum enumValue)
		//{
		//	var arManageMentReports = new List<ReportsEnum> { ReportsEnum.AR_Aging_Summary, ReportsEnum.AR_Aging_Summary_With_Payment_info, ReportsEnum.Activity_Summary, ReportsEnum.Activity_Summary_By_Charge_Status };
		//	var dailyClaimReports = ReportsEnum.Daily_Claim_Report;
		//	if (arManageMentReports.Contains(enumValue))
		//	{
		//		return ReportCategoryEnum.AR_Management_Report;
		//	}
		//	else if (enumValue == dailyClaimReports)
		//	{
		//		return ReportCategoryEnum.Daily_Monthly_Reports;
		//	}
		//	return 0;
		//}

		//private void SeedEmployeeRoleDepartments()
		//{
		//	Task.Run(async () =>
		//	{
		//		foreach (var enumValue in Enum.GetValues<DepartmentEnum>())
		//		{
		//			var departmentAssociatedRoles = GetRolesByDepartment(enumValue);
		//			foreach (var dr in departmentAssociatedRoles)
		//			{
		//				var EmployeeRoleDepartmentExist = _db.EmployeeRoleDepartments.FirstOrDefault(z => z.DepartmentId == enumValue && z.EmployeeRoleId == dr) ?? null;
		//				if (EmployeeRoleDepartmentExist is null)
		//				{
		//					_db.EmployeeRoleDepartments.Add(new EmployeeRoleDepartment
		//					{
		//						EmployeeRoleId = dr,
		//						DepartmentId = enumValue
		//					});
		//				}

		//			}

		//		}

		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default EmployeeRoleDepartments");
		//}

		//private static List<EmployeeRoleEnum> GetRolesByDepartment(DepartmentEnum dept)
		//{
		//	switch (dept)
		//	{
		//		case DepartmentEnum.Registor:
		//			return ReadOnlyObjects.RegistrationEmployeeRoles.ToList();
		//		case DepartmentEnum.Medical:
		//			return ReadOnlyObjects.MedicalEmployeeRoles.ToList();
		//		case DepartmentEnum.Billing:
		//			return ReadOnlyObjects.BillingEmployeeRoles.ToList();
		//		case DepartmentEnum.Credentialing:
		//			return ReadOnlyObjects.CredentialingEmployeeRoles.ToList();
		//		case DepartmentEnum.ChargeEntry:
		//			return ReadOnlyObjects.ChargeEntryEmployeeRoles.ToList();
		//		case DepartmentEnum.CashPosting:
		//			return ReadOnlyObjects.CashPostingEmployeeRoles.ToList();
		//		default:
		//			return new List<EmployeeRoleEnum>();
		//	}
		//}

		//#region custom dashboard seeding
		//public static List<DashboardItem> DashboardItems = new()
		//{
		//	new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Date Lag Cards", Selector = "available", Icon = Icons.Material.Filled.SpaceDashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ClaimsDateLagInfoCardComponent", NeedsLayoutFilter = true },
  //          //new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Info Cards", Selector = "available", Icon = Icons.Material.Filled.Dashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout },
  //          new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Revenue Analysis Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "RevenueAnalysisChartComponent"},
		//	new DashboardItem() { Order = 3, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "AR Aging Amount Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ARAgingAmountDistributionChartComponent" },
		//	new DashboardItem() { Order = 4, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Reverse Analysis Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ARAgingReverseAnalysisChartComponent" },
		//	new DashboardItem() { Order = 5, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Claim Status Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ClaimStatusDashboardComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 6, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Denial Reason Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsDashboardComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 7, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Denial Reasons By Insurance Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsByInsuranceDashboardComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 8, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Claims In Process By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ClaimsInProcessByPayerComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 9, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "Average Allowed Amount By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "AverageAllowedAmountByPayerComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.CurrentSummaryDashboard, Name = "AR Aging Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ARAgingChartComponent", NeedsLayoutFilter = true },
  //          //new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Info Cards", Selector = "available", Icon = Icons.Material.Filled.Dashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout },
  //          new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Claim Status Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "ClaimStatusDashboardComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Denial Reason Chart", Selector = "available", Icon = Icons.Material.Filled.PieChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsDashboardComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 3, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Denial Reasons By Insurance Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "DenialReasonsByInsuranceDashboardComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 4, Dashboard = DashboardCategoryEnum.InitialSummaryDashboard, Name = "Initially Reviewed By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.InsertChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.InitialClaimStatusDashboardLayout, ComponentTitle = "InitiallyReviewedByPayerComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.ComparisonDashboard, Name = "Provider Comparison Grid", Selector = "available", Icon = Icons.Material.Filled.TableChart, Category = ItemCategoryEnum.Grid, Layout = LayoutCategoryEnum.ComparisonDashboardLayout, ComponentTitle = "ProviderComparisonTableComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.ComparisonDashboard, Name = "Provider Visits Stacked Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ComparisonDashboardLayout, ComponentTitle = "ProviderComparisonByVisitComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.ComparisonDashboard, Name = "Provider Visits Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ComparisonDashboardLayout, ComponentTitle = "ProviderComparisonByVisitTotalsComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 0, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Insurance Self Pay Review Card", Selector = "available", Icon = Icons.Material.Filled.SpaceDashboard, Category = ItemCategoryEnum.Card, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "InsurancesSelfPayReviewedComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Eligibilities By Status Insurance Stacked Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "EligibilitiesByStatusInsuranceStackedComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Discovered Eligibilities Bar Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "DiscoveredEligibilitiesBarChartComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Eligibility Monthly Totals Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.LargeChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "EligibilityMonthlyTotalsComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Eligibility Value By Payer Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "EligibilityValueByPayerComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.EligibilityDashboard, Name = "Percentage Returned By Month Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.SelfPayEligibilityDashboardLayout, ComponentTitle = "PercentageReturnedByMonthComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.InsurancesDashbaord, Name = "Charges By Insurance Bar Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ChargesByInsuranceBarChartComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.InsurancesDashbaord, Name = "Payments By Insurance Bar Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "PaymentsByInsuranceBarChartComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.InsurancesDashbaord, Name = "Denials By Insurance Bar Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.SmallChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialsByInsuranceBarChartComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 2, Dashboard = DashboardCategoryEnum.ProcedureLevelsDashboard, Name = "Procedures By Denial Reason Chart", Selector = "available", Icon = Icons.Material.Filled.StackedBarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "ProceduresByDenialReasonComponent", NeedsLayoutFilter = true },
		//	new DashboardItem() { Order = 1, Dashboard = DashboardCategoryEnum.ProcedureLevelsDashboard, Name = "Denials By Procedure Bar Chart", Selector = "available", Icon = Icons.Material.Filled.BarChart, Category = ItemCategoryEnum.MediumChart, Layout = LayoutCategoryEnum.ClaimStatusDashboardLayout, ComponentTitle = "DenialsByProcedureBarChartComponent", NeedsLayoutFilter = true },
		//};

		//private void SeedDashboardItems()
		//{
		//	Task.Run(async () =>
		//	{
		//		foreach (var item in DashboardItems)
		//		{
		//			var DashboardItemExists = _db.DashboardItems.FirstOrDefault(z => z.Name == item.Name && z.Dashboard == item.Dashboard) ?? null;
		//			if (DashboardItemExists is null)
		//			{
		//				_db.DashboardItems.Add(item);
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default DashboardItems");
		//}
		//#endregion

		////AA-231
		//private void SeedWriteOffTypeEnum()
		//{
		//	Task.Run(async () =>
		//	{
		//		foreach (var enumValue in Enum.GetValues<WriteOffTypeEnum>())
		//		{
		//			var existingWriteOffType = _db.WriteOffTypes.FirstOrDefault(z => z.Id == enumValue) ?? null;
		//			if (existingWriteOffType is null)
		//			{
		//				_db.WriteOffTypes.Add(new WriteOffType
		//				{
		//					Id = enumValue,
		//					Name = enumValue.ToString(),
		//					Description = enumValue.GetDescription(),
		//				});
		//			}
		//		}
		//	}).GetAwaiter().GetResult();
		//	_logger.LogInformation("Seeded default WriteOffTypeEnum");
		//}
	}
}