using MedHelpAuthorizations.Application.Interfaces.Services;
using MedHelpAuthorizations.Domain.Contracts;
using MedHelpAuthorizations.Domain.Entities;
using MedHelpAuthorizations.Domain.Entities.Enums;
using MedHelpAuthorizations.Domain.Entities.IntegratedServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authorization = MedHelpAuthorizations.Domain.Entities.Authorization;
using DayOfWeek = MedHelpAuthorizations.Domain.Entities.DayOfWeek;
using Message = MedHelpAuthorizations.Domain.Entities.Message;

namespace MedHelpAuthorizations.Infrastructure.Persistence.Context
{
    using Finbuckle.MultiTenant;
    using MedHelpAuthorizations.Application.Options;
    using MedHelpAuthorizations.Domain.IntegratedServices;
    using Microsoft.Extensions.Options;
    using System.Reflection.Emit;

    public class ApplicationContext : AuditableContext
    {
        private readonly IDateTimeService _dateTimeService;
        public string TenantId { get; set; }

        //private readonly ITenantService2 _tenantService;

        public ApplicationContext(ITenantInfo currentTenant, DbContextOptions<ApplicationContext> options, ICurrentUserService currentUserService, IOptions<DatabaseSettings> dbSettings)
            //(DbContextOptions<ApplicationContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService, ITenantService2 tenantService)
            : base(currentTenant, options, currentUserService, dbSettings)
        {
            //_currentUserService = currentUserService;
            //_dateTimeService = dateTimeService;
            //_tenantService = tenantService;
            //TenantId = _tenantService.GetTenant()?.TID;
        }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<ApiIntegration> ApiIntegrations { get; set; }

		public DbSet<ApiIntegrationType> ApiIntegrationTypes { get; set; }

		//public DbSet<ClientApiIntegration> ClientApiIntegrations { get; set; }

		public DbSet<ClientApiIntegrationKey> ClientApiIntegrationKeys { get; set; }

        public DbSet<AddressType> AddressTypes { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<AdministrativeGender> AdministrativeGenders { get; set; }

        //public DbSet<ChatHistory> ChatHistories { get; set; }

        public DbSet<Document> Documents { get; set; }

        public DbSet<Authorization> Authorizations { get; set; }

        public DbSet<AuthType> AuthTypes { get; set; }

        public DbSet<Domain.Entities.Client> Clients { get; set; }

        public DbSet<ClientAuthType> ClientAuthTypes { get; set; }

		public DbSet<ClientCptCode> ClientCptCodes { get; set; }

        public DbSet<CptCode> CptCodes { get; set; }

        public DbSet<TypeOfService> TypesOfService { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<PlaceOfServiceCode> PlaceOfServiceCodes { get; set; }

        public DbSet<ClientPlaceOfService> ClientPlacesOfService { get; set; }

        public DbSet<ClientLocation> ClientLocations { get; set; }

        public DbSet<ClientLocationInsuranceIdentifier> ClientLocationInsuranceIdentifiers { get; set; }

        public DbSet<UserClient> UserClients { get; set; }

        public DbSet<AuthorizationStatus> AuthorizationStatuses { get; set; }

        public DbSet<ClientQuestionnaire> ClientQuestionnaires { get; set; }

        public DbSet<ClientQuestionnaireCategory> ClientQuestionnaireCategories { get; set; }

        public DbSet<QuestionCategory> QuestionCategories { get; set; }

        public DbSet<ClientQuestionnaireCategoryQuestion> ClientQuestionnaireCategoryQuestions { get; set; }


        //PatientQuestionnaireAnswer
        public DbSet<PatientQuestionnaireAnswer> PatientQuestionnaireAnswers { get; set; }

        public DbSet<DocumentType> DocumentTypes { get; set; }

        public DbSet<ClientInsurance> ClientInsurances { get; set; }

        public DbSet<UserAlert> UserAlerts { get; set; }

        public DbSet<InsuranceCard> InsuranceCards { get; set; }

        public DbSet<Cardholder> Cardholders { get; set; }

        public DbSet<GenderIdentity> GenderIdentities { get; set; }

        public DbSet<Person> Persons { get; set; }

        public DbSet<ClaimLineItemStatus> ClaimLineItemStatuses { get; set; }

        public DbSet<ClaimStatus> ClaimStatuses { get; set; }

        public DbSet<ClaimStatusTransaction> ClaimStatusTransactions { get; set; }

        public DbSet<ClaimStatusTransactionHistory> ClaimStatusTransactionHistories { get; set; }

        public DbSet<ClientInsuranceRpaConfiguration> ClientInsuranceRpaConfigurations { get; set; }

        public DbSet<ClaimStatusBatch> ClaimStatusBatches { get; set; }

        public DbSet<ClaimStatusBatchHistory> ClaimStatusBatchHistories { get; set; }

        public DbSet<ClaimStatusBatchClaim> ClaimStatusBatchClaims { get; set; }

        public DbSet<ClaimStatusExceptionReasonCategory> ClaimStatusExceptionReasonCategories { get; set; }

        public DbSet<ClaimStatusExceptionReasonCategoryMap> ClaimStatusExceptionReasonCategoryMaps { get; set; }

        //public DbSet<ClaimStatusResolutionInterval> ClaimStatusResolutionIntervals { get; set; }

        public DbSet<RpaInsurance> RpaInsurances { get; set; }

        public DbSet<TransactionType> TransactionTypes { get; set; }

        public DbSet<DbOperation> DbOperations { get; set; }

        public DbSet<InputDocumentType> InputDocumentTypes { get; set; }

        public DbSet<InputDocument> InputDocuments { get; set; }

        public DbSet<ResponsibleParty> ResponsibleParties { get; set; }

        public DbSet<ApplicationFeature> ApplicationFeatures { get; set; }

        public DbSet<ClientApplicationFeature> ClientApplicationFeatures { get; set; }

        public DbSet<Flow> Flows { get; set; }

        public DbSet<FlowLogEntry> FlowLogEntries { get; set; }

        public DbSet<ChargeEntryBatch> ChargeEntryBatches { get; set; }

        public DbSet<ChargeEntryBatchHistory> ChargeEntryBatchHistories { get; set; }

        public DbSet<ChargeEntryRpaConfiguration> ChargeEntryRpaConfigurations { get; set; }

        public DbSet<ChargeEntryTransaction> ChargeEntryTransactions { get; set; }

        public DbSet<ChargeEntryTransactionHistory> ChargeEntryTransactionHistories { get; set; }

        public DbSet<RpaType> RpaTypes { get; set; }

        public DbSet<ApplicationReport> ApplicationReports { get; set; }

        public DbSet<ClientUserApplicationReport> ClientUserApplicationReports { get; set; }

        public DbSet<ClaimStatusTransactionLineItemStatusChangẹ> ClaimStatusTransactionLineItemStatusChangẹs { get; set; }//TAPI-118

        public DbSet<ClientProvider> ClientProviders { get; set; }

        public DbSet<ProviderLevel> ProviderLevels { get; set; }

        public DbSet<ReferringProvider> ReferringProviders { get; set; }

        public DbSet<Specialty> Specialties { get; set; }

        public DbSet<ClaimStatusWorkstationNotes> ClaimStatusWorkstationNotes { get; set; }

        public DbSet<ClientProviderLocation> ClientProviderLocations { get; set; } //AA-106
        public DbSet<EmployeeRole> EmployeeRoles { get; set; } // AA-128
        public DbSet<ClientEmployeeRole> ClientEmployeeRoles { get; set; } // AA-128        
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeClient> EmployeeClients { get; set; }
        public DbSet<EmployeeClientLocation> EmployeeClientLocations { get; set; }
        public DbSet<EmployeeRoleClaimStatusExceptionReasonCategory> EmployeeRoleClaimStatusExceptionReasonCategories { get; set; }
        public DbSet<EmployeeClientInsurance> EmployeeClientInsurances { get; set; }
        public DbSet<EmployeeClientAlphaSplit> EmployeeClientAlphaSplits { get; set; }
        //public DbSet<ClientEmployeeKpi> ClientEmployeeKpis { get; set; }   //AA-144
        // public DbSet<ClientLocationServiceType> ClientLocationServiceTypes { get; set; }//AA-127
        public DbSet<Department> Departments { get; set; } //AA-148
        public DbSet<EmployeeRoleDepartment> EmployeeRoleDepartments { get; set; } //AA-148
        public DbSet<Report> Report { get; set; }//AA-157
        public DbSet<ClientUserReportFilter> ClientUserReportFilters { get; set; }//AA-157
        public DbSet<ReportCategories> ReportCategories { get; set; }//AA-157
        public DbSet<ClientKpi> ClientKpi { get; set; } //AA-183
        public DbSet<AlphaSplit> AlphaSplits { get; set; }
        public DbSet<ClientFeeSchedule> ClientFeeSchedules { get; set; } //AA-160
        public DbSet<ClientFeeScheduleProviderLevel> ClientFeeScheduleProviderLevels { get; set; }
        public DbSet<ClientFeeScheduleSpecialty> ClientFeeScheduleSpecialties { get; set; }
        public DbSet<ClientFeeScheduleEntry> ClientFeeScheduleEntries { get; set; } //AA-161
        public DbSet<ClientInsuranceFeeSchedule> ClientInsuranceFeeSchedules { get; set; } //AA-161
        public DbSet<ClientRpaCredentialConfiguration> ClientRpaCredentialConfigurations { get; set; } //AA-23
        public DbSet<RpaInsuranceGroup> RpaInsuranceGroups { get; set; } //AA-23
        public DbSet<ImportDocumentMessage> ImportDocumentMessages { get; set; } //AA-264
        public DbSet<ClientLocationSpeciality> ClientLocationSpecialities { get; set; } //AA-242
        public DbSet<DashboardItem> DashboardItems { get; set; } //AA-284
        public DbSet<UserDashboardItem> UserDashboardItems { get; set; } //AA-284
        public DbSet<WriteOffType> WriteOffTypes { get; set; } //AA-231
        public DbSet<ClientInsuranceAverageCollectionPercentage> ClientInsuranceAverageCollectionPercentages { get; set; } //EN-91
        public DbSet<EmployeeClientUserReportFilter> EmployeeClientUserReportFilter { get; set; } //AA-231
        public DbSet<UnmappedFeeScheduleCpt> UnmappedFeeScheduleCpts { get; set; } //EN-155
        public DbSet<SourceSystem> SourceSystems { get; set; } //EN-64
        public DbSet<ClientSpecialty> ClientSpecialties { get; set; } //EN-201
		public DbSet<AdjustmentType> AdjustmentTypes { get; set; }
		public DbSet<ClientAdjustmentCode> ClientAdjustmentCodes { get; set; }
		public DbSet<PatientLedgerCharge> PatientLedgerCharges { get; set; }
		public DbSet<PatientLedgerPayment> PatientLedgerPayments { get; set; }
		public DbSet<PatientLedgerAdjustment> PatientLedgerAdjustments { get; set; }
		public DbSet<ClientRemittance> ClientRemittances { get; set; }
		public DbSet<SystemDefaultReportFilter> SystemDefaultReportFilters { get; set; }//EN-108
        public DbSet<SystemDefaultReportFilterEmployeeRole> SystemDefaultReportFilterEmployeeRoles { get; set; }//EN-108
        public DbSet<ClaimStatusTotalResult> ClaimStatusTotalResults { get; set; } //EN-305
        public DbSet<ClaimStatusType> ClaimStatusTypes { get; set; } //EN-362

        //public DbSet<ClientLocationInsuranceRpaConfiguration> ClientLocationInsuranceRpaConfigurations { get; set; }
        public DbSet<ClientEndOfMonthTotal> ClientEndOfMonthTotals { get; set; }
        public DbSet<X12ClaimCategory> X12ClaimCategories { get; set; }
		public DbSet<X12ClaimCategoryCodeLineItemStatus> X12ClaimCategoryCodeLineItemStatuses { get; set; }
		public DbSet<X12ClaimCodeType> X12ClaimCodeTypes { get; set; }
		public DbSet<X12ClaimCodeLineItemStatus> X12ClaimCodeLineItemStatuses { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        //public DbSet<DayOfWeek> DaysOfWeek { get; set; }
        public DbSet<ClientHoliday> clientHolidays { get; set; }
        public DbSet<ClientDayOfOperation> clientDaysOfOperation { get; set; }
        public DbSet<ClientEncounterType> ClientEncounterTypes { get; set; } //EN-701

        public DbSet<ApiClaimsMessageClaimLineitemStatusMap> ApiClaimsMessageClaimLineitemStatusMaps { get; set; }  //EN-678
        public DbSet<ClientNote> ClientNotes { get; set; }
        public DbSet<ClientDocument> ClientDocuments { get; set; }

        public DbSet<ClientUserNotification> ClientUserNotifications { get; set; }
        public DbSet<X12ClaimStatusCode> X12ClaimStatusCodes { get; set; }
        
        #region Monthly Entities
        public DbSet<MonthlyCashCollectionData> MonthlyCashCollectionData { get; set; }
        public DbSet<MonthlyDenialData> MonthlyDenialData { get; set; }
        public DbSet<MonthlyReceivablesData> MonthlyReceivablesData { get; set; }
        public DbSet<MonthlyARData> MonthlyARData { get; set; }
        public DbSet<MonthlyARSummary> MonthlyARSummaries { get; set; }
        #endregion

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = DateTime.UtcNow;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            //foreach (var entry in ChangeTracker.Entries<ITenant>().ToList())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Added:
            //        case EntityState.Modified:
            //            entry.Entity.TenantId = TenantId;
            //            break;
            //    }
            //}
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            builder.Entity<ClientProviderLocation>(entity =>
            {
                entity.ToTable(name: "ClientProviderLocations", "dbo")
               .HasIndex(c => new { c.ClientLocationId, c.ClientProviderId })
               .IsUnique(true);
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});
            builder.Entity<ClientLocationInsuranceIdentifier>(entity =>
            {
                entity.ToTable(name: "ClientLocationInsuranceIdentifiers", "dbo")
               .HasIndex(c => new { c.ClientLocationId, c.ClientInsuranceId, c.ClientId })
               .IsUnique(true);
            });
            //AA-127
            builder.Entity<ClientLocationTypeOfService>(entity =>
            {
                entity.ToTable(name: "ClientLocationServiceTypes", "dbo")
               .HasIndex(c => new { c.ClientLocationId, c.TypeOfServiceId, c.ClientId }).IsUnique(true);
			});
            builder.Entity<Authorization>(entity =>
            {
                entity.ToTable(name: "Authorizations", "dbo");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });

			builder.Entity<AdjustmentType>(entity =>
			{
				entity.ToTable(name: "AdjustmentTypes", "dbo");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
			});
			builder.Entity<AuthType>(entity =>
            {
                entity.ToTable(name: "AuthTypes", "dbo");
            });
            builder.Entity<Domain.Entities.ClientKpi>(entity =>
            {
                entity.ToTable(name: "ClientKpi", "dbo");
            });
            builder.Entity<Domain.Entities.Client>(entity =>
            {
                entity.HasOne(c => c.ClientKpi);
                entity.ToTable(name: "Clients", "dbo");
            });
            builder.Entity<ClientAuthType>(entity =>
            {
                entity.ToTable(name: "ClientAuthTypes", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClientCptCode>(entity =>
            {
                entity.ToTable(name: "ClientCptCodes", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<CptCode>(entity =>
            {
                entity.ToTable(name: "CptCodes", "dbo");
            });
            builder.Entity<TypeOfService>(entity =>
            {
                entity.ToTable(name: "TypesOfService", "dbo");
            });
            builder.Entity<Document>(entity =>
            {
                entity.ToTable(name: "Documents", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<Message>(entity =>
            {
                entity.ToTable(name: "Messages", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<Note>(entity =>
            {
                entity.ToTable(name: "Notes", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<Patient>(entity =>
            {
                entity.ToTable(name: "Patients", "dbo");
                entity.Property(e => e.AccountNumber)
                .HasMaxLength(25)
                .IsRequired(true)
                .HasComputedColumnSql("CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
				//entity.HasQueryFilter(a => a.TenantId == TenantId);
			});
            builder.Entity<UserClient>(entity =>
            {
                entity.ToTable(name: "UserClients", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<PlaceOfServiceCode>(entity =>
            {
                entity.ToTable(name: "PlaceOfServiceCodes", "dbo");
            });
            builder.Entity<ClientPlaceOfService>(entity =>
            {
                entity.ToTable(name: "ClientPlacesOfService", "dbo");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
				//entity.HasQueryFilter(a => a.TenantId == TenantId);
			});
            builder.Entity<ClientLocation>(entity =>
            {
                entity.ToTable(name: "ClientLocations", "dbo");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});
            builder.Entity<ClientQuestionnaire>(entity =>
            {
                entity.ToTable(name: "ClientQuestionnaires", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClientQuestionnaireCategoryQuestion>(entity =>
            {
                entity.ToTable(name: "ClientQuestionnaireCategoryQuestions", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClientQuestionnaireCategory>(entity =>
            {
                entity.ToTable(name: "ClientQuestionnaireCategories", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<QuestionCategory>(entity =>
            {
                entity.ToTable(name: "QuestionCategories", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClientQuestionnaireCategoryQuestionOption>(entity =>
            {
                entity.ToTable(name: "ClientQuestionnaireCategoryQuestionOptions", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<PatientQuestionnaireAnswer>(entity =>
            {
                entity.ToTable(name: "PatientQuestionnaireAnswers", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<State>(entity =>
            {
                entity.ToTable(name: "States", "dbo")
                .Property(et => et.Id)
                .ValueGeneratedNever()
                .HasConversion(
                       v => (int)v,
                       v => (StateEnum)v)
                       .IsUnicode(false); ;
            });
            builder.Entity<AuthorizationStatus>(entity =>
            {
                entity.ToTable(name: "AuthorizationStatuses", "dbo");
                entity.Property(e => e.Id)
                   .ValueGeneratedNever()
                   .HasConversion(
                       v => (int)v,
                       v => (AuthorizationStatusEnum)v)
                       .IsUnicode(false);
            });

            builder.Entity<DocumentType>(entity =>
            {
                entity.ToTable(name: "DocumentTypes", "dbo");
            });

            builder.Entity<DocumentType>().HasData(new DocumentType[] {
                new DocumentType{Id =1, Name = "DLA20", Description="Daily Living Activities-20"},
                new DocumentType{Id =2, Name = "Referral", Description="Referral Document"},
                new DocumentType{Id =3,Name = "Insurance Card", Description="Insurance Card Document"},
                new DocumentType{Id =4,Name = "Treatment Plan", Description="Treatment Plan Document"},
            });

            builder.Entity<ClientInsurance>(entity =>
            {
                entity.ToTable(name: "ClientInsurances", "dbo");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});

            builder.Entity<UserAlert>(entity =>
            {
                entity.ToTable(name: "UserAlerts", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });

            //builder.Entity<Insurance>(entity =>
            //{
            //    entity.ToTable(name: "Insurances", "dbo");
            //});
            builder.Entity<InsuranceCard>(entity =>
            {
                entity.ToTable(name: "InsuranceCards", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});
            builder.Entity<AdministrativeGender>(entity =>
            {
                entity.ToTable(name: "AdministrativeGenders", "dbo");
            });
            builder.Entity<Cardholder>(entity =>
            {
                entity.ToTable(name: "Cardholders", "dbo");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});
            builder.Entity<GenderIdentity>(entity =>
            {
                entity.ToTable(name: "GenderIdentities", "dbo");
            });
            builder.Entity<Person>(entity =>
            {
                entity.ToTable(name: "Persons", "dbo");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });
            builder.Entity<GenderIdentity>().HasData(Enum.GetValues(typeof(GenderIdentityEnum))
                .Cast<GenderIdentityEnum>()
                .Select(x => new GenderIdentity()
                {
                    Id = x,
                    Name = x.ToString()
                }));
            builder.Entity<ClientInsuranceRpaConfiguration>(entity =>
            {
                entity.ToTable(name: "ClientInsuranceRpaConfigurations", "IntegratedServices")
                .HasIndex(c => new { c.ClientInsuranceId, c.TransactionTypeId, c.AuthTypeId, c.ExternalId })
                .IsUnique(true);
            });
            builder.Entity<ClaimStatusBatch>(entity =>
            {
                entity.ToTable(name: "ClaimStatusBatches", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(c => c.AssignedDateTimeUtc);
                entity.HasIndex(c => c.IsDeleted);
                entity.HasIndex(c => c.Priority);
                entity.HasIndex(c => c.AbortedOnUtc);
                entity.Property(e => e.BatchNumber)
                .HasMaxLength(12)
                .IsRequired(true)
                .HasComputedColumnSql("CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)");
            });
            builder.Entity<ClaimStatusBatchHistory>(entity =>
            {
                entity.ToTable(name: "ClaimStatusBatchHistories", "IntegratedServices");
                entity.HasIndex(c => c.AssignedDateTimeUtc);
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(c => c.AbortedOnUtc);
            });
            builder.Entity<RpaInsuranceGroup>(entity =>
            {
                entity.ToTable(name: "RpaInsuranceGroups", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
            builder.Entity<RpaInsurance>(entity =>
            {
                entity.ToTable(name: "RpaInsurances", "IntegratedServices");
            });
            builder.Entity<ClaimLineItemStatus>(entity =>
            {
                entity.ToTable(name: "ClaimLineItemStatuses", "IntegratedServices");
            });
			builder.Entity<ApiIntegrationType>(entity =>
			{
				entity.ToTable(name: "ApiIntegrationTypes", "dbo");
			});
			builder.Entity<ApiIntegration>(entity =>
            {
                entity.ToTable(name: "ApiIntegrations", "dbo");
            });
			//builder.Entity<ClientApiIntegration>(entity =>
			//{
			//	entity.ToTable(name: "ClientApiIntegrations", "dbo");
			//});
			builder.Entity<ClientApiIntegrationKey>(entity =>
            {
                entity.ToTable(name: "ClientApiIntegrationKeys", "dbo")
                    .HasIndex(c => new { c.ClientId, c.ApiIntegrationId, c.ApiVersion });
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClaimStatus>(entity =>
            {
                entity.ToTable(name: "ClaimStatuses", "IntegratedServices");
            });
            builder.Entity<ClaimStatusBatchClaim>(entity =>
            {
                entity.Ignore(c => c.BilledAmountString);
                entity.Ignore(c => c.DateOfServiceFromString);
                entity.Ignore(c => c.DateOfServiceToString);
                entity.Ignore(c => c.ClaimBilledOnString);
                entity.Property(c => c.EntryMd5Hash)
                    .IsRequired(true)
                    //.HasComputedColumnSql($"CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(UPPER(ClaimNumber), '|', UPPER(ProcedureCode), '|',CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)", stored: true);
                    //.HasComputedColumnSql($"CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(UPPER(TRIM(PatientLastName)), '|', UPPER(TRIM(PatientFirstName)), '|', UPPER(CONVERT(varchar(8),DateOfBirth, 112)), '|', UPPER(ClaimNumber), '|', UPPER(ProcedureCode), '|', UPPER(Modifiers), '|', CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)", stored: true);
                    .HasComputedColumnSql($"CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(TRIM(CONVERT(varchar(12), PatientId)), '|', UPPER(ProcedureCode), '|', UPPER(Modifiers), '|', CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)", stored: true);
                entity.Property(c => c.ClaimLevelMd5Hash)
                    .IsRequired(false)
                    .HasComputedColumnSql($"CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(TRIM(CONVERT(varchar(12), PatientId)), '|', UPPER(NormalizedClaimNumber), '|', UPPER(ClientId), '|', UPPER(ClientInsuranceId), '|', CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)", stored: true);
                entity.HasIndex(c => c.ClaimBilledOn);
                entity.HasIndex(c => c.ClientFeeScheduleEntryId);
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(e => e.DateOfServiceFrom);
                entity.HasIndex(e => e.DateOfServiceTo);
                entity.HasIndex(c => c.EntryMd5Hash);
                entity.HasIndex(c => c.ClaimLevelMd5Hash);
                entity.HasOne(c => c.ClientFeeScheduleEntry)
                   .WithMany(c => c.ClaimStatusBatchClaims)
                   .OnDelete(DeleteBehavior.ClientSetNull);
                entity.ToTable(name: "ClaimStatusBatchClaims", "IntegratedServices");
                    
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClaimStatusTransaction>(entity =>
            {
                entity.ToTable(name: "ClaimStatusTransactions", "IntegratedServices")
                    .HasOne(c => c.ClaimLineItemStatus)
                    .WithMany(c => c.ClaimStatusTransactions);
                entity.HasIndex(c => c.ClaimStatusBatchClaimId)
                .IsUnique(true);
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(e => e.CheckDate);
                entity.HasIndex(e => e.ClaimStatusTransactionEndDateTimeUtc);
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ClaimStatusTransactionHistory>(entity =>
            {
                entity.ToTable(name: "ClaimStatusTransactionHistories", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });
            builder.Entity<TransactionType>(entity =>
            {
                entity.ToTable(name: "TransactionTypes", "IntegratedServices");
            });
            builder.Entity<ClaimStatusExceptionReasonCategory>(entity =>
            {
                entity.ToTable(name: "ClaimStatusExceptionReasonCategories", "IntegratedServices");
            });
            builder.Entity<ClaimStatusExceptionReasonCategoryMap>(entity =>
                {
                    entity.ToTable(name: "ClaimStatusExceptionReasonCategoryMaps", "IntegratedServices");
                });
            builder.Entity<InputDocumentType>(entity =>
            {
                entity.ToTable(name: "InputDocumentTypes", "IntegratedServices");
            });
            builder.Entity<InputDocument>(entity =>
            {
                entity.ToTable(name: "InputDocuments", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });
            builder.Entity<DbOperation>(entity =>
            {
                entity.ToTable(name: "DbOperations", "IntegratedServices");
            });
            builder.Entity<Flow>(entity =>
            {
                entity.ToTable(name: "Flows", "IntegratedServices");
            });
            builder.Entity<FlowLogEntry>(entity =>
            {
                entity.ToTable(name: "FlowLogEntries", "IntegratedServices");
            });
            builder.Entity<ChargeEntryRpaConfiguration>(entity =>
            {
                entity.ToTable(name: "ChargeEntryRpaConfigurations", "IntegratedServices");
            });
            builder.Entity<ChargeEntryBatch>(entity =>
            {
                entity.ToTable(name: "ChargeEntryBatches", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.Property(e => e.BatchNumber)
                .HasMaxLength(12)
                .IsRequired(true)
                .HasComputedColumnSql("CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)");
            });
            builder.Entity<ChargeEntryBatchHistory>(entity =>
            {
                entity.ToTable(name: "ChargeEntryBatchHistories", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn); ;
            });
            builder.Entity<ChargeEntryTransaction>(entity =>
            {
                entity.ToTable(name: "ChargeEntryTransactions", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(e => e.ChargeEntryTransactionEndDateTimeUtc);
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ChargeEntryTransactionHistory>(entity =>
            {
                entity.ToTable(name: "ChargeEntryTransactionHistories", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });
            builder.Entity<RpaType>(entity =>
            {
                entity.ToTable(name: "RpaTypes", "IntegratedServices");
                entity.Property(e => e.IsMaxConsecutiveIssueResolved)
                    .IsRequired(true)
                    .HasDefaultValue(true);
            });
            builder.Entity<ResponsibleParty>(entity =>
            {
                entity.ToTable(name: "ResponsibleParties", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(25)
                    .IsRequired(true)
                    .HasComputedColumnSql("CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)");
				entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
				//entity.HasQueryFilter(a => a.TenantId == TenantId);
			});
            builder.Entity<ApplicationFeature>(entity =>
            {
                entity.ToTable(name: "ApplicationFeatures", "dbo");
            });
            builder.Entity<ClientApplicationFeature>(entity =>
            {
                entity.ToTable(name: "ClientApplicationFeatures", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<ApplicationReport>(entity =>
            {
                entity.ToTable(name: "ApplicationReports", "dbo");
            });
            builder.Entity<ClientUserApplicationReport>(entity =>
            {
                entity.ToTable(name: "ClientUserApplicationReports", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            //TAPI-118
            builder.Entity<ClaimStatusTransactionLineItemStatusChangẹ>(entity =>
            {
                entity.ToTable(name: "ClaimStatusTransactionLineItemStatusChangẹs", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });
			builder.Entity<ClientRemittance>(entity =>
			{
				entity.ToTable(name: "ClientRemittances", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});
			builder.Entity<ClientProvider>(entity =>
            {
                entity.ToTable(name: "Providers", "dbo");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
				//entity.HasQueryFilter(a => a.TenantId == TenantId);
			});
            builder.Entity<ReferringProvider>(entity =>
            {
                entity.ToTable(name: "ReferringProviders", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<Specialty>(entity =>
            {
                entity.ToTable(name: "Specialties", "dbo");
            });
            builder.Entity<Address>(entity =>
            {
                entity.ToTable(name: "Addresses", "dbo");
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });
            builder.Entity<AddressType>(entity =>
            {
                entity.ToTable(name: "AddressTypes", "dbo");
            });
            builder.Entity<ClaimStatusWorkstationNotes>(entity =>
            {
                entity.ToTable(name: "ClaimStatusWorkstationNotes", "IntegratedServices");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
            });
            builder.Entity<Report>(entity =>
            {
                entity.ToTable(name: "Report", "dbo");
            });
            builder.Entity<ClientUserReportFilter>(entity =>
            {
                entity.ToTable(name: "ClientUserReportFilter", "dbo");
            });
            builder.Entity<ReportCategories>(entity =>
            {
                entity.ToTable(name: "ReportCategories", "dbo");
            });
            builder.Entity<EmployeeRoleClaimStatusExceptionReasonCategory>(entity =>
            {
                entity.ToTable(name: "EmployeeRoleClaimStatusExceptionReasonCategories", "dbo");
            });
            builder.Entity<EmployeeRoleDepartment>(entity =>
            {
                entity.ToTable(name: "EmployeeRoleDepartments", "dbo");
            });
            builder.Entity<ClientEmployeeRole>(entity =>
            {
                entity.ToTable(name: "ClientEmployeeRoles", "dbo");
            });
            builder.Entity<AlphaSplit>(entity =>
            {
                entity.ToTable(name: "AlphaSplits", "dbo");
            });
            builder.Entity<ClientInsuranceAverageCollectionPercentage>(entity =>
            {
                entity.ToTable(name: "ClientInsuranceAverageCollectionPercentages", "dbo");
            });
            builder.Entity<EmployeeClientUserReportFilter>(entity =>
            {
                entity.ToTable(name: "EmployeeClientUserReportFilter", "dbo");
            });
            builder.Entity<UnmappedFeeScheduleCpt>(entity =>
            {
                entity.ToTable(name: "UnmappedFeeScheduleCpts", "dbo");
            });

            builder.Entity<SystemDefaultReportFilter>(entity =>
            {
                entity.ToTable(name: "SystemDefaultReportFilters", "dbo");
            });
            builder.Entity<SystemDefaultReportFilterEmployeeRole>(entity =>
            {
                entity.ToTable(name: "SystemDefaultReportFilterEmployeeRoles", "dbo");
            });
            builder.Entity<ClientFeeSchedule>(entity =>
            {
                entity.ToTable(name: "ClientFeeSchedules", "dbo");
            });
            builder.Entity<ClientFeeScheduleProviderLevel>(entity =>
            {
                entity.ToTable(name: "ClientFeeScheduleProviderLevels", "dbo");
            });
            builder.Entity<ClientFeeScheduleSpecialty>(entity =>
            {
                entity.ToTable(name: "ClientFeeScheduleSpecialties", "dbo");
            });
            builder.Entity<ClientFeeScheduleEntry>(entity =>
            {
                entity.ToTable(name: "ClientFeeScheduleEntries", "dbo");
            });
            builder.Entity<ClientInsuranceFeeSchedule>(entity =>
            {
                entity.ToTable(name: "ClientInsuranceFeeSchedules", "dbo");
            });
			builder.Entity<X12ClaimCategory>(entity =>
			{
				entity.ToTable(name: "X12ClaimCategories", "IntegratedServices");
			});
			builder.Entity<X12ClaimCategoryCodeLineItemStatus>(entity =>
			{
				entity.ToTable(name: "X12ClaimCategoryCodeLineItemStatuses", "IntegratedServices");
			});
			builder.Entity<X12ClaimCodeLineItemStatus>(entity =>
			{
				entity.ToTable(name: "X12ClaimCodeLineItemStatuses", "IntegratedServices");
			});
			builder.Entity<X12ClaimCodeType>(entity =>
			{
				entity.ToTable(name: "X12ClaimCodeTypes", "IntegratedServices");
			});
			builder.Entity<ClientAdjustmentCode>(entity =>
			{
                entity.ToTable(name: "ClientAdjustmentCodes", "dbo");
                entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);
			});
			builder.Entity<PatientLedgerCharge>(entity =>
			{
				entity.ToTable(name: "PatientLedgerCharges", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);

                // Specify the associated trigger
                entity.ToTable("PatientLedgerCharges", tb => tb.HasTrigger("trg_UpdateOutstandingBalance_OnCharges"));
            });
			builder.Entity<PatientLedgerPayment>(entity =>
			{
				entity.ToTable(name: "PatientLedgerPayments", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.CreatedOn);
				entity.HasIndex(e => e.LastModifiedOn);
				entity.HasIndex(e => e.DfExternalId);
				entity.HasIndex(e => e.DfCreatedOn);
				entity.HasIndex(e => e.DfLastModifiedOn);

                // Specify the associated trigger
                entity.ToTable("PatientLedgerPayments", tb => tb.HasTrigger("trg_UpdateOutstandingBalance_OnPayments"));
            });
            builder.Entity<PatientLedgerAdjustment>(entity =>
            {
                entity.ToTable(name: "PatientLedgerAdjustments", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(e => e.DfExternalId);
                entity.HasIndex(e => e.DfCreatedOn);
                entity.HasIndex(e => e.DfLastModifiedOn);

                // Specify the associated trigger
                entity.ToTable("PatientLedgerAdjustments", tb => tb.HasTrigger("trg_UpdateOutstandingBalance_OnAdjustments"));
            });
            builder.Entity<ApiClaimsMessageClaimLineitemStatusMap>(entity =>
            {
                entity.ToTable(name: "ApiClaimsMessageClaimLineitemStatusMaps", "IntegratedServices");
                entity.HasIndex(e => e.Code);
            });
            builder.Entity<ResponsibleParty>(entity =>
            {
                entity.ToTable(name: "ResponsibleParties", "dbo");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.AccountNumber)
                    .HasMaxLength(25)
                    .IsRequired(true)
                    .HasComputedColumnSql("CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)");
                entity.HasIndex(e => e.CreatedOn);
                entity.HasIndex(e => e.LastModifiedOn);
                entity.HasIndex(e => e.DfExternalId);
                entity.HasIndex(e => e.DfCreatedOn);
                entity.HasIndex(e => e.DfLastModifiedOn);
                //entity.HasQueryFilter(a => a.TenantId == TenantId);
            });

            builder.Entity<MonthlyARData>(entity =>
            {
                entity.ToTable(name: "MonthlyARData", "dbo");
                entity.Property(e => e.PercentageOfAR)
                    .HasComputedColumnSql(
                        "CASE WHEN TotalReceivables = 0 THEN 0 ELSE (Receivables * 100) / TotalReceivables END"
                    );
            });
            builder.Entity<X12ClaimStatusCode>(entity =>
            {
                entity.ToTable(name: "X12ClaimStatusCodes", "IntegratedServices");
            });
        }
    }
}
