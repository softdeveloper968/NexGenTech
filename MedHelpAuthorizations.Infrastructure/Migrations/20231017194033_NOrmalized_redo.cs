using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NOrmalized_redo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_RpaInsurances_RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientAlphaSplits_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientInsurances_ClientInsurances_ClientInsuranceId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientInsurances_Employees_EmployeeId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientLocations_ClientLocations_ClientLocationId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientLocations_Employees_EmployeeId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClients_Clients_ClientId",
                table: "EmployeeClients");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClients_Employees_EmployeeId",
                table: "EmployeeClients");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeLevels_EmployeeLevelId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Persons_PersonId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_InputDocuments_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropTable(
                name: "ClientEmployeeDepartment");

            migrationBuilder.DropTable(
                name: "ClientEmployeeKpis");

            migrationBuilder.DropTable(
                name: "ClientReportFilters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeClaimStatusExceptionReasonCategories");

            migrationBuilder.DropTable(
                name: "EmployeeDepartments");

            migrationBuilder.DropTable(
                name: "EmployeeLevels");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmployeeLevelId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientLocations_EmployeeId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientInsurances_EmployeeId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_ClientKpi_ClientId",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientInsuranceId_RpaInsuranceId_TransactionTypeId_AuthTypeId_ExternalId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "UserClients");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "UserAlerts");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ReferringProviders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "QuestionCategories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Persons");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "PatientQuestionnaireAnswers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "PatientQuestionnaire");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropColumn(
                name: "ClaimCountRequired",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropColumn(
                name: "BeginAlpha",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropColumn(
                name: "EndAlpha",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ConcurrentAuthorization");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientUserApplicationReports");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaires");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestionOptions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaireCategories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientPlacesOfService");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropColumn(
                name: "ExpiryWarningReported",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "FailureMessage",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "FailureReported",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "IsConfigInUse",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "Password",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "ReportFailureToEmail",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "TargetUrl",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "UseOffHoursOnly",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "Username",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientCptCodes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientAuthTypes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientApplicationFeatures");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "ClientApiIntegrationKeys");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Authorizations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "dbo",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "Departments");

            migrationBuilder.RenameColumn(
                name: "PersonId",
                table: "Employees",
                newName: "DefaultEmployeeRoleId");

            migrationBuilder.RenameColumn(
                name: "EmployeeLevelId",
                table: "Employees",
                newName: "OverallAverageDailyClaimCount");

            migrationBuilder.RenameColumn(
                name: "AlphabeticalSplit",
                table: "Employees",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_PersonId",
                table: "Employees",
                newName: "IX_Employees_DefaultEmployeeRoleId");

            migrationBuilder.RenameColumn(
                name: "ClientLevelKpisId",
                schema: "dbo",
                table: "Clients",
                newName: "ClientKpiId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                newName: "CalculatedLookupHashInput");

            migrationBuilder.AddColumn<int>(
                name: "RpaInsuranceGroupId",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetUrl",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SocialSecurityNumber",
                schema: "dbo",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ActualImportCount",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttemptedImportCount",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImportStatus",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "DefaultReceiveReport",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "OverallExpectedMonthlyCashCollections",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeClients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "EmployeeClients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AssignedAverageDailyClaimCount",
                table: "EmployeeClients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpectedMonthlyCashCollections",
                table: "EmployeeClients",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveReport",
                table: "EmployeeClients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ClientLocationId",
                table: "EmployeeClientLocations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeClientId",
                table: "EmployeeClientLocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ClientInsuranceId",
                table: "EmployeeClientInsurances",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmployeeClientId",
                table: "EmployeeClientInsurances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeClientId",
                table: "EmployeeClientAlphaSplits",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AlphaSplitId",
                table: "EmployeeClientAlphaSplits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CustomBeginAlpha",
                table: "EmployeeClientAlphaSplits",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomEndAlpha",
                table: "EmployeeClientAlphaSplits",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyCashCollection",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DailyClaimCount",
                schema: "dbo",
                table: "ClientKpi",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "ClientInsurances",
                type: "nvarchar(125)",
                maxLength: 125,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldMaxLength: 25,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LookupName",
                schema: "dbo",
                table: "ClientInsurances",
                type: "nvarchar(125)",
                maxLength: 125,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentDayClaimCount",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DailyClaimLimit",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WriteOffTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CalculatedLookupHash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientFeeScheduleEntryId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "WriteOffAmount",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimLevelMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true,
                computedColumnSql: "CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(TRIM(CONVERT(varchar(12), PatientId)), '|', UPPER(ClaimNumber), '|', UPPER(ClientId), '|', UPPER(ClientInsuranceId), '|', CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)",
                stored: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Departments",
                table: "Departments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AlphaSplits",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    BeginAlpha = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EndAlpha = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlphaSplits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientFeeSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImportStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFeeSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFeeSchedules_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientLocationSpecialities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecialityId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocationSpecialities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocationSpecialities_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientLocationSpecialities_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserReportFilter",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilterConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasDefaultFilter = table.Column<bool>(type: "bit", nullable: false),
                    RunSavedDefaultFilter = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserReportFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserReportFilter_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientUserReportFilter_Report_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "dbo",
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DashboardItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Selector = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CanDrag = table.Column<bool>(type: "bit", nullable: false),
                    NeedsLayoutFilter = table.Column<bool>(type: "bit", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComponentTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Dashboard = table.Column<int>(type: "int", nullable: false),
                    Layout = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DashboardItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportDocumentMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimStatusBatchId = table.Column<int>(type: "int", nullable: false),
                    InputDocumentId = table.Column<int>(type: "int", nullable: false),
                    MessageType = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDocumentMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportDocumentMessages_InputDocuments_InputDocumentId",
                        column: x => x.InputDocumentId,
                        principalSchema: "IntegratedServices",
                        principalTable: "InputDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RpaInsuranceGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultTargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RpaInsuranceGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WriteOffTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WriteOffTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientFeeScheduleEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientCptCodeId = table.Column<int>(type: "int", nullable: false),
                    ClientFeeScheduleId = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AllowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsReimbursable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFeeScheduleEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFeeScheduleEntries_ClientCptCodes_ClientCptCodeId",
                        column: x => x.ClientCptCodeId,
                        principalSchema: "dbo",
                        principalTable: "ClientCptCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFeeScheduleEntries_ClientFeeSchedules_ClientFeeScheduleId",
                        column: x => x.ClientFeeScheduleId,
                        principalTable: "ClientFeeSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientInsuranceFeeSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientFeeScheduleId = table.Column<int>(type: "int", nullable: false),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInsuranceFeeSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInsuranceFeeSchedules_ClientFeeSchedules_ClientFeeScheduleId",
                        column: x => x.ClientFeeScheduleId,
                        principalTable: "ClientFeeSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientInsuranceFeeSchedules_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDashboardItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DashboardItemId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDashboardItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDashboardItems_DashboardItems_DashboardItemId",
                        column: x => x.DashboardItemId,
                        principalTable: "DashboardItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientEmployeeRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    EmployeeRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEmployeeRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeRoles_EmployeeClients_EmployeeClientId",
                        column: x => x.EmployeeClientId,
                        principalTable: "EmployeeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeRoles_EmployeeRoles_EmployeeRoleId",
                        column: x => x.EmployeeRoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoleClaimStatusExceptionReasonCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeRoleId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusExceptionReasonCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleClaimStatusExceptionReasonCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                        column: x => x.ClaimStatusExceptionReasonCategoryId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusExceptionReasonCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleClaimStatusExceptionReasonCategories_EmployeeRoles_EmployeeRoleId",
                        column: x => x.EmployeeRoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoleDepartments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeRoleId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleDepartments_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleDepartments_EmployeeRoles_EmployeeRoleId",
                        column: x => x.EmployeeRoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRpaCredentialConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RpaInsuranceGroupId = table.Column<int>(type: "int", nullable: true),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReportFailureToEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryWarningReported = table.Column<bool>(type: "bit", nullable: false),
                    IsCredentialInUse = table.Column<bool>(type: "bit", nullable: false),
                    UseOffHoursOnly = table.Column<bool>(type: "bit", nullable: false),
                    OtpForwardFromEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureReported = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRpaCredentialConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRpaCredentialConfigurations_RpaInsuranceGroups_RpaInsuranceGroupId",
                        column: x => x.RpaInsuranceGroupId,
                        principalTable: "RpaInsuranceGroups",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RpaInsurances_RpaInsuranceGroupId",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                column: "RpaInsuranceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientLocations_EmployeeClientId",
                table: "EmployeeClientLocations",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientInsurances_EmployeeClientId",
                table: "EmployeeClientInsurances",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientAlphaSplits_AlphaSplitId",
                table: "EmployeeClientAlphaSplits",
                column: "AlphaSplitId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientKpiId",
                schema: "dbo",
                table: "Clients",
                column: "ClientKpiId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientKpi_ClientId",
                schema: "dbo",
                table: "ClientKpi",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientInsuranceId_TransactionTypeId_AuthTypeId_ExternalId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                columns: new[] { "ClientInsuranceId", "TransactionTypeId", "AuthTypeId", "ExternalId" },
                unique: true,
                filter: "[AuthTypeId] IS NOT NULL AND [ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientRpaCredentialConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_WriteOffTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "WriteOffTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClientFeeScheduleEntryId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientFeeScheduleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeRoles_EmployeeClientId",
                schema: "dbo",
                table: "ClientEmployeeRoles",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeRoles_EmployeeRoleId",
                schema: "dbo",
                table: "ClientEmployeeRoles",
                column: "EmployeeRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeScheduleEntries_ClientCptCodeId",
                table: "ClientFeeScheduleEntries",
                column: "ClientCptCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeScheduleEntries_ClientFeeScheduleId",
                table: "ClientFeeScheduleEntries",
                column: "ClientFeeScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeSchedules_ClientId",
                table: "ClientFeeSchedules",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceFeeSchedules_ClientFeeScheduleId",
                table: "ClientInsuranceFeeSchedules",
                column: "ClientFeeScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceFeeSchedules_ClientInsuranceId",
                table: "ClientInsuranceFeeSchedules",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationSpecialities_ClientId",
                table: "ClientLocationSpecialities",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationSpecialities_ClientLocationId",
                table: "ClientLocationSpecialities",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRpaCredentialConfigurations_RpaInsuranceGroupId",
                table: "ClientRpaCredentialConfigurations",
                column: "RpaInsuranceGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserReportFilter_ClientId",
                schema: "dbo",
                table: "ClientUserReportFilter",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserReportFilter_ReportId",
                schema: "dbo",
                table: "ClientUserReportFilter",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                schema: "dbo",
                table: "EmployeeRoleClaimStatusExceptionReasonCategories",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleClaimStatusExceptionReasonCategories_EmployeeRoleId",
                schema: "dbo",
                table: "EmployeeRoleClaimStatusExceptionReasonCategories",
                column: "EmployeeRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleDepartments_DepartmentId",
                schema: "dbo",
                table: "EmployeeRoleDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleDepartments_EmployeeRoleId",
                schema: "dbo",
                table: "EmployeeRoleDepartments",
                column: "EmployeeRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportDocumentMessages_InputDocumentId",
                table: "ImportDocumentMessages",
                column: "InputDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDashboardItems_DashboardItemId",
                table: "UserDashboardItems",
                column: "DashboardItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientFeeScheduleEntries_ClientFeeScheduleEntryId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientFeeScheduleEntryId",
                principalTable: "ClientFeeScheduleEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTransactions_WriteOffTypes_WriteOffTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "WriteOffTypeId",
                principalTable: "WriteOffTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "AuthTypeId",
                principalSchema: "dbo",
                principalTable: "AuthTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientRpaCredentialConfigurations_ClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientRpaCredentialConfigurationId",
                principalTable: "ClientRpaCredentialConfigurations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_RpaInsurances_RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "RpaInsuranceId",
                principalSchema: "IntegratedServices",
                principalTable: "RpaInsurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_ClientKpi_ClientKpiId",
                schema: "dbo",
                table: "Clients",
                column: "ClientKpiId",
                principalSchema: "dbo",
                principalTable: "ClientKpi",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientAlphaSplits_AlphaSplits_AlphaSplitId",
                table: "EmployeeClientAlphaSplits",
                column: "AlphaSplitId",
                principalSchema: "dbo",
                principalTable: "AlphaSplits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientAlphaSplits_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientAlphaSplits",
                column: "EmployeeClientId",
                principalTable: "EmployeeClients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientInsurances_ClientInsurances_ClientInsuranceId",
                table: "EmployeeClientInsurances",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientInsurances_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientInsurances",
                column: "EmployeeClientId",
                principalTable: "EmployeeClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientLocations_ClientLocations_ClientLocationId",
                table: "EmployeeClientLocations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientLocations_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientLocations",
                column: "EmployeeClientId",
                principalTable: "EmployeeClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClients_Clients_ClientId",
                table: "EmployeeClients",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClients_Employees_EmployeeId",
                table: "EmployeeClients",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeRoles_DefaultEmployeeRoleId",
                table: "Employees",
                column: "DefaultEmployeeRoleId",
                principalTable: "EmployeeRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InputDocuments_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RpaInsurances_RpaInsuranceGroups_RpaInsuranceGroupId",
                schema: "IntegratedServices",
                table: "RpaInsurances",
                column: "RpaInsuranceGroupId",
                principalTable: "RpaInsuranceGroups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientFeeScheduleEntries_ClientFeeScheduleEntryId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTransactions_WriteOffTypes_WriteOffTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientRpaCredentialConfigurations_ClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_RpaInsurances_RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_Clients_ClientKpi_ClientKpiId",
                schema: "dbo",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientAlphaSplits_AlphaSplits_AlphaSplitId",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientAlphaSplits_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientInsurances_ClientInsurances_ClientInsuranceId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientInsurances_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientLocations_ClientLocations_ClientLocationId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClientLocations_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClients_Clients_ClientId",
                table: "EmployeeClients");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeClients_Employees_EmployeeId",
                table: "EmployeeClients");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeRoles_DefaultEmployeeRoleId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_InputDocuments_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_RpaInsurances_RpaInsuranceGroups_RpaInsuranceGroupId",
                schema: "IntegratedServices",
                table: "RpaInsurances");

            migrationBuilder.DropTable(
                name: "AlphaSplits",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientEmployeeRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientFeeScheduleEntries");

            migrationBuilder.DropTable(
                name: "ClientInsuranceFeeSchedules");

            migrationBuilder.DropTable(
                name: "ClientLocationSpecialities");

            migrationBuilder.DropTable(
                name: "ClientRpaCredentialConfigurations");

            migrationBuilder.DropTable(
                name: "ClientUserReportFilter",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeRoleClaimStatusExceptionReasonCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeRoleDepartments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ImportDocumentMessages");

            migrationBuilder.DropTable(
                name: "UserDashboardItems");

            migrationBuilder.DropTable(
                name: "WriteOffTypes");

            migrationBuilder.DropTable(
                name: "ClientFeeSchedules");

            migrationBuilder.DropTable(
                name: "RpaInsuranceGroups");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "DashboardItems");

            migrationBuilder.DropIndex(
                name: "IX_RpaInsurances_RpaInsuranceGroupId",
                schema: "IntegratedServices",
                table: "RpaInsurances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientLocations_EmployeeClientId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientInsurances_EmployeeClientId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeClientAlphaSplits_AlphaSplitId",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropIndex(
                name: "IX_Clients_ClientKpiId",
                schema: "dbo",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_ClientKpi_ClientId",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientInsuranceId_TransactionTypeId_AuthTypeId_ExternalId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTransactions_WriteOffTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_ClientFeeScheduleEntryId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Departments",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ClaimLevelMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropColumn(
                name: "RpaInsuranceGroupId",
                schema: "IntegratedServices",
                table: "RpaInsurances");

            migrationBuilder.DropColumn(
                name: "TargetUrl",
                schema: "IntegratedServices",
                table: "RpaInsurances");

            migrationBuilder.DropColumn(
                name: "ActualImportCount",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropColumn(
                name: "AttemptedImportCount",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropColumn(
                name: "ImportStatus",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropColumn(
                name: "DefaultReceiveReport",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "OverallExpectedMonthlyCashCollections",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "AssignedAverageDailyClaimCount",
                table: "EmployeeClients");

            migrationBuilder.DropColumn(
                name: "ExpectedMonthlyCashCollections",
                table: "EmployeeClients");

            migrationBuilder.DropColumn(
                name: "ReceiveReport",
                table: "EmployeeClients");

            migrationBuilder.DropColumn(
                name: "EmployeeClientId",
                table: "EmployeeClientLocations");

            migrationBuilder.DropColumn(
                name: "EmployeeClientId",
                table: "EmployeeClientInsurances");

            migrationBuilder.DropColumn(
                name: "AlphaSplitId",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropColumn(
                name: "CustomBeginAlpha",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropColumn(
                name: "CustomEndAlpha",
                table: "EmployeeClientAlphaSplits");

            migrationBuilder.DropColumn(
                name: "ClientRpaCredentialConfigurationId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "CurrentDayClaimCount",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "DailyClaimLimit",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "WriteOffTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropColumn(
                name: "CalculatedLookupHash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropColumn(
                name: "ClientFeeScheduleEntryId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropColumn(
                name: "WriteOffAmount",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Department");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Employees",
                newName: "AlphabeticalSplit");

            migrationBuilder.RenameColumn(
                name: "OverallAverageDailyClaimCount",
                table: "Employees",
                newName: "EmployeeLevelId");

            migrationBuilder.RenameColumn(
                name: "DefaultEmployeeRoleId",
                table: "Employees",
                newName: "PersonId");

            migrationBuilder.RenameIndex(
                name: "IX_Employees_DefaultEmployeeRoleId",
                table: "Employees",
                newName: "IX_Employees_PersonId");

            migrationBuilder.RenameColumn(
                name: "ClientKpiId",
                schema: "dbo",
                table: "Clients",
                newName: "ClientLevelKpisId");

            migrationBuilder.RenameColumn(
                name: "CalculatedLookupHashInput",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                newName: "TenantId");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "UserClients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "UserAlerts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ResponsibleParties",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ReferringProviders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "QuestionCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Providers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SocialSecurityNumber",
                schema: "dbo",
                table: "Persons",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Persons",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "PatientQuestionnaireAnswers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "PatientQuestionnaire",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "InsuranceCards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClaimCountRequired",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "EmployeeClients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "EmployeeClients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientLocationId",
                table: "EmployeeClientLocations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeeClientLocations",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientInsuranceId",
                table: "EmployeeClientInsurances",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "EmployeeClientInsurances",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeClientId",
                table: "EmployeeClientAlphaSplits",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BeginAlpha",
                table: "EmployeeClientAlphaSplits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndAlpha",
                table: "EmployeeClientAlphaSplits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "ConcurrentAuthorization",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientUserApplicationReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaires",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestionOptions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientQuestionnaireCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientLocations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MonthlyCashCollection",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<int>(
                name: "DailyClaimCount",
                schema: "dbo",
                table: "ClientKpi",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "dbo",
                table: "ClientInsurances",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(125)",
                oldMaxLength: 125,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LookupName",
                schema: "dbo",
                table: "ClientInsurances",
                type: "nvarchar(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(125)",
                oldMaxLength: 125,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientInsurances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ExpiryWarningReported",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FailureMessage",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FailureReported",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsConfigInUse",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReportFailureToEmail",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetUrl",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "UseOffHoursOnly",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientCptCodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientAuthTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientApplicationFeatures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "ClientApiIntegrationKeys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Cardholders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Authorizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                schema: "dbo",
                table: "Addresses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClientEmployeeDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEmployeeDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeDepartment_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeDepartment_EmployeeClients_EmployeeClientId",
                        column: x => x.EmployeeClientId,
                        principalTable: "EmployeeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientEmployeeKpis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    AverageDailyClaimsWorkedKpi = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MonthlyCashCollectionsKpi = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEmployeeKpis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEmployeeKpis_EmployeeClients_EmployeeClientId",
                        column: x => x.EmployeeClientId,
                        principalTable: "EmployeeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientReportFilters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FilterConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasDefaultFilter = table.Column<bool>(type: "bit", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RunSavedDefaultFilter = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientReportFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientReportFilters_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientReportFilters_Report_ReportId",
                        column: x => x.ReportId,
                        principalSchema: "dbo",
                        principalTable: "Report",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeClaimStatusExceptionReasonCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimStatusExceptionReasonCategoryId = table.Column<int>(type: "int", nullable: true),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClaimStatusExceptionReasonCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                        column: x => x.ClaimStatusExceptionReasonCategoryId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusExceptionReasonCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeClaimStatusExceptionReasonCategories_EmployeeClients_EmployeeClientId",
                        column: x => x.EmployeeClientId,
                        principalTable: "EmployeeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeClaimStatusExceptionReasonCategories_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeDepartments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeDepartments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeDepartments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDepartments_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeDepartments_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLevels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeLevelId",
                table: "Employees",
                column: "EmployeeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientLocations_EmployeeId",
                table: "EmployeeClientLocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientInsurances_EmployeeId",
                table: "EmployeeClientInsurances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientKpi_ClientId",
                schema: "dbo",
                table: "ClientKpi",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientInsuranceId_RpaInsuranceId_TransactionTypeId_AuthTypeId_ExternalId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                columns: new[] { "ClientInsuranceId", "RpaInsuranceId", "TransactionTypeId", "AuthTypeId", "ExternalId" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeDepartment_DepartmentId",
                table: "ClientEmployeeDepartment",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeDepartment_EmployeeClientId",
                table: "ClientEmployeeDepartment",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEmployeeKpis_EmployeeClientId",
                table: "ClientEmployeeKpis",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReportFilters_ClientId",
                schema: "dbo",
                table: "ClientReportFilters",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientReportFilters_ReportId",
                schema: "dbo",
                table: "ClientReportFilters",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                table: "EmployeeClaimStatusExceptionReasonCategories",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClaimStatusExceptionReasonCategories_EmployeeClientId",
                table: "EmployeeClaimStatusExceptionReasonCategories",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClaimStatusExceptionReasonCategories_EmployeeId",
                table: "EmployeeClaimStatusExceptionReasonCategories",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartments_ClientId",
                table: "EmployeeDepartments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartments_DepartmentId",
                table: "EmployeeDepartments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeDepartments_EmployeeId",
                table: "EmployeeDepartments",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "AuthTypeId",
                principalSchema: "dbo",
                principalTable: "AuthTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_RpaInsurances_RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "RpaInsuranceId",
                principalSchema: "IntegratedServices",
                principalTable: "RpaInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientAlphaSplits_EmployeeClients_EmployeeClientId",
                table: "EmployeeClientAlphaSplits",
                column: "EmployeeClientId",
                principalTable: "EmployeeClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientInsurances_ClientInsurances_ClientInsuranceId",
                table: "EmployeeClientInsurances",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientInsurances_Employees_EmployeeId",
                table: "EmployeeClientInsurances",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientLocations_ClientLocations_ClientLocationId",
                table: "EmployeeClientLocations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClientLocations_Employees_EmployeeId",
                table: "EmployeeClientLocations",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClients_Clients_ClientId",
                table: "EmployeeClients",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeClients_Employees_EmployeeId",
                table: "EmployeeClients",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeLevels_EmployeeLevelId",
                table: "Employees",
                column: "EmployeeLevelId",
                principalTable: "EmployeeLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Persons_PersonId",
                table: "Employees",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Persons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InputDocuments_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
