using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "IntegratedServices");

            migrationBuilder.CreateTable(
                name: "AddressTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeGenders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeGenders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiIntegrations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiIntegrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationFeatures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationFeatures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditTrails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditTrails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationStatuses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimLineItemStatuses",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    MaximumPipelineDays = table.Column<int>(type: "int", nullable: false),
                    MinimumResolutionAttempts = table.Column<int>(type: "int", nullable: false),
                    MaximumResolutionAttempts = table.Column<int>(type: "int", nullable: false),
                    DaysWaitBetweenAttempts = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimLineItemStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatuses",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusExceptionReasonCategories",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusExceptionReasonCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CptCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfServiceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CptCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DbOperations",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Flows",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlowName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenderIdentities",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenderIdentities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InputDocumentTypes",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaceOfServiceCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LookupName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceOfServiceCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RelativeDateRange",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelativeDateRange", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ReportCategoryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RpaInsurances",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ClaimBilledOnWaitDays = table.Column<int>(type: "int", nullable: false),
                    InactivatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RpaInsurances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RpaTypes",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    ReleaseKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsMaxConsecutiveIssueResolved = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RpaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", unicode: false, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTypes",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypesOfService",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LookupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypesOfService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAlerts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlertType = table.Column<int>(type: "int", nullable: false),
                    PreviewText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsViewed = table.Column<bool>(type: "bit", nullable: false),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationReports",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ApplicationFeatureId = table.Column<int>(type: "int", nullable: false),
                    ReportName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationReports_ApplicationFeatures_ApplicationFeatureId",
                        column: x => x.ApplicationFeatureId,
                        principalSchema: "dbo",
                        principalTable: "ApplicationFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageDataURL = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusExceptionReasonCategoryMaps",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimStatusExceptionReasonCategoryId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusExceptionReasonText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusExceptionReasonCategoryMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusExceptionReasonCategoryMaps_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                        column: x => x.ClaimStatusExceptionReasonCategoryId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusExceptionReasonCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressTypeId = table.Column<int>(type: "int", nullable: false),
                    AddressStreetLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressStreetLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Normalized = table.Column<bool>(type: "bit", nullable: false),
                    DeliveryPointBarcode = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "dbo",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationClientCptCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizationId = table.Column<int>(type: "int", nullable: false),
                    ClientCptCodeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationClientCptCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthTypeId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Completeby = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Units = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DischargedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DischargedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallbackDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CareManagerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    AuthorizationStatusId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    ClientPlaceOfServiceId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Authorizations_AuthorizationStatuses_AuthorizationStatusId",
                        column: x => x.AuthorizationStatusId,
                        principalSchema: "dbo",
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConcurrentAuthorization",
                columns: table => new
                {
                    InitialAuthorizationId = table.Column<int>(type: "int", nullable: false),
                    SucceededAuthorizationId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrentAuthorization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConcurrentAuthorization_Authorizations_InitialAuthorizationId",
                        column: x => x.InitialAuthorizationId,
                        principalSchema: "dbo",
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConcurrentAuthorization_Authorizations_SucceededAuthorizationId",
                        column: x => x.SucceededAuthorizationId,
                        principalSchema: "dbo",
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cardholders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SignatureOnFile = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cardholders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChargeEntryBatches",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false, computedColumnSql: "CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)"),
                    ChargeEntryRpaConfigurationId = table.Column<int>(type: "int", nullable: false),
                    DateOfServiceFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfServiceTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessStartDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessStartedByHostIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessStartedByRpaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeEntryBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeEntryBatches_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChargeEntryBatchHistories",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeEntryBatchId = table.Column<int>(type: "int", nullable: false),
                    ChargeEntryRpaConfigurationId = table.Column<int>(type: "int", nullable: false),
                    DateOfServiceFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfServiceTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessStartDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessStartedByHostIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessStartedByRpaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DbOperationId = table.Column<int>(type: "int", nullable: false),
                    AllClaimStatusesResolvedOrExpired = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    AuthTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeEntryBatchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeEntryBatchHistories_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChargeEntryBatchHistories_ChargeEntryBatches_ChargeEntryBatchId",
                        column: x => x.ChargeEntryBatchId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ChargeEntryBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChargeEntryBatchHistories_DbOperations_DbOperationId",
                        column: x => x.DbOperationId,
                        principalSchema: "IntegratedServices",
                        principalTable: "DbOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargeEntryRpaConfigurations",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    RpaTypeId = table.Column<int>(type: "int", nullable: false),
                    AuthTypeId = table.Column<int>(type: "int", nullable: true),
                    RelativeDateRangeId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureReported = table.Column<bool>(type: "bit", nullable: false),
                    FailureMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeEntryRpaConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeEntryRpaConfigurations_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ChargeEntryRpaConfigurations_RelativeDateRange_RelativeDateRangeId",
                        column: x => x.RelativeDateRangeId,
                        principalTable: "RelativeDateRange",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChargeEntryRpaConfigurations_RpaTypes_RpaTypeId",
                        column: x => x.RpaTypeId,
                        principalSchema: "IntegratedServices",
                        principalTable: "RpaTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChargeEntryRpaConfigurations_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalSchema: "IntegratedServices",
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChargeEntryTransactionHistories",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeEntryTransactionId = table.Column<int>(type: "int", nullable: false),
                    PatientFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeEntryBatchId = table.Column<int>(type: "int", nullable: false),
                    ChargeEntryTransactionBeginDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChargeEntryTransactionEndDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OriginalPrimaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalSecondaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedPrimaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedSecondaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalTaxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedTaxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineItemControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UiPathUniqueReference = table.Column<string>(type: "nvarchar(72)", maxLength: 72, nullable: true),
                    Successful = table.Column<bool>(type: "bit", nullable: false),
                    ExceptionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DbOperationId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeEntryTransactionHistories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChargeEntryTransactions",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientAccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChargeEntryBatchId = table.Column<int>(type: "int", nullable: false),
                    CommaDelimitedDxCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommaDelimitedProcedureCodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfServiceFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfServiceTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChargeEntryTransactionBeginDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChargeEntryTransactionEndDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalPrimaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalSecondaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedPrimaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedSecondaryInsurance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalTaxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorrectedTaxId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LineItemControlNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuccessfullyBilled = table.Column<bool>(type: "bit", nullable: false),
                    ExceptionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UiPathUniqueReference = table.Column<string>(type: "nvarchar(72)", maxLength: 72, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeEntryTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeEntryTransactions_ChargeEntryBatches_ChargeEntryBatchId",
                        column: x => x.ChargeEntryBatchId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ChargeEntryBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusBatchClaimRoot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    PatientLastName = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    PatientFirstName = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfBirthString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PolicyNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DateOfServiceFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfServiceFromString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfServiceTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfServiceToString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimBilledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClaimBilledOnString = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Modifiers = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BilledAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BilledAmountString = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RenderingNpi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GroupNpi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    EntryMd5Hash = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusBatchClaimRoot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusBatchClaims",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusBatchId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusTransactionId = table.Column<int>(type: "int", nullable: true),
                    PatientLastName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    PatientFirstName = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    PolicyNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    DateOfServiceFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfServiceTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClaimBilledOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcedureCode = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Modifiers = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BilledAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsSupplanted = table.Column<bool>(type: "bit", nullable: false),
                    RenderingNpi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GroupNpi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    EntryMd5Hash = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false, computedColumnSql: "CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(TRIM(CONVERT(varchar(12), PatientId)), '|', UPPER(ProcedureCode), '|', UPPER(Modifiers), '|', CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)", stored: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    ClaimStatusBatchClaimRootId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusBatchClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusBatchClaims_ClaimStatusBatchClaimRoot_ClaimStatusBatchClaimRootId",
                        column: x => x.ClaimStatusBatchClaimRootId,
                        principalTable: "ClaimStatusBatchClaimRoot",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusBatches",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNumber = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false, computedColumnSql: "CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    InputDocumentId = table.Column<int>(type: "int", nullable: false),
                    AuthTypeId = table.Column<int>(type: "int", nullable: false),
                    AssignedClientRpaConfigurationId = table.Column<int>(type: "int", nullable: true),
                    AssignedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedToIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToHostName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToRpaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToRpaLocalProcessIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllClaimStatusesResolvedOrExpired = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    RpaInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusBatches_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimStatusBatches_RpaInsurances_RpaInsuranceId",
                        column: x => x.RpaInsuranceId,
                        principalSchema: "IntegratedServices",
                        principalTable: "RpaInsurances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusBatchHistories",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimStatusBatchId = table.Column<int>(type: "int", nullable: false),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    AuthTypeId = table.Column<int>(type: "int", nullable: false),
                    AssignedClientRpaConfigurationId = table.Column<int>(type: "int", nullable: true),
                    AssignedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AssignedToIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToHostName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToRpaCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AssignedToRpaLocalProcessIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompletedDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AbortedReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewedOnUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DbOperationId = table.Column<int>(type: "int", nullable: false),
                    AllClaimStatusesResolvedOrExpired = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusBatchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusBatchHistories_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimStatusBatchHistories_ClaimStatusBatches_ClaimStatusBatchId",
                        column: x => x.ClaimStatusBatchId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClaimStatusBatchHistories_DbOperations_DbOperationId",
                        column: x => x.DbOperationId,
                        principalSchema: "IntegratedServices",
                        principalTable: "DbOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusTransactionHistories",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ClaimStatusTransactionId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusTransactionBeginDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimStatusTransactionEndDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LineItemControlNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TotalClaimStatusId = table.Column<int>(type: "int", nullable: false),
                    TotalClaimStatusValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimLineItemStatusId = table.Column<int>(type: "int", nullable: true),
                    ClaimLineItemStatusValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimStatusExceptionReasonCategoryId = table.Column<int>(type: "int", nullable: true),
                    ExceptionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemarkCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ReasonCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ReasonDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemarkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalNonAllowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAllowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalMemberResponsibilityAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeductibleAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CopayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CoinsuranceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CobAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PenalityAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineItemChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineItemPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineItemApprovedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiagnosisDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateEntered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ServiceLineDenialReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AuthorizationFound = table.Column<bool>(type: "bit", nullable: true),
                    AuthorizationStatusId = table.Column<int>(type: "int", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EligibilityInsurance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EligibilityPolicyNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EligibilityPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CobLastVerified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrimaryPayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedMemberId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActiveEligibleDateRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityStatus = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    ICN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferringProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HippaStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingProviderNPI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateClaimFinalized = table.Column<DateTime>(type: "datetime2", nullable: true),
                    COInsurerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoInsurerEffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_EligibilityFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_EligibilityToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_DeductibleFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_DeductibleToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_RemainingDeductible = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartB_EligibilityFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_EligibilityToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_DeductibleFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_DeductibleToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_RemainingDeductible = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtCapYearFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtCapYearTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtCapUsedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PtCapYearFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PtCapYearTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PtCapUsedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InputDataListIndex = table.Column<int>(type: "int", nullable: true),
                    InputDataFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DbOperationId = table.Column<int>(type: "int", nullable: false),
                    WriteoffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusTransactionHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionHistories_AuthorizationStatuses_AuthorizationStatusId",
                        column: x => x.AuthorizationStatusId,
                        principalSchema: "dbo",
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionHistories_ClaimLineItemStatuses_ClaimLineItemStatusId",
                        column: x => x.ClaimLineItemStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimLineItemStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionHistories_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                        column: x => x.ClaimStatusExceptionReasonCategoryId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusExceptionReasonCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionHistories_ClaimStatuses_TotalClaimStatusId",
                        column: x => x.TotalClaimStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionHistories_DbOperations_DbOperationId",
                        column: x => x.DbOperationId,
                        principalSchema: "IntegratedServices",
                        principalTable: "DbOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusTransactionLineItemStatusChangẹs",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimStatusTransactionId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PreviousClaimLineItemStatusId = table.Column<int>(type: "int", nullable: true),
                    UpdatedClaimLineItemStatusId = table.Column<int>(type: "int", nullable: false),
                    DbOperationId = table.Column<int>(type: "int", nullable: false),
                    WriteoffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusTransactionLineItemStatusChangẹs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionLineItemStatusChangẹs_ClaimLineItemStatuses_PreviousClaimLineItemStatusId",
                        column: x => x.PreviousClaimLineItemStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimLineItemStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionLineItemStatusChangẹs_ClaimLineItemStatuses_UpdatedClaimLineItemStatusId",
                        column: x => x.UpdatedClaimLineItemStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimLineItemStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactionLineItemStatusChangẹs_DbOperations_DbOperationId",
                        column: x => x.DbOperationId,
                        principalSchema: "IntegratedServices",
                        principalTable: "DbOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusTransactions",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    ClaimStatusBatchClaimId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusTransactionBeginDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClaimStatusTransactionEndDateTimeUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LineItemControlNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TotalClaimStatusId = table.Column<int>(type: "int", nullable: false),
                    TotalClaimStatusValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimLineItemStatusId = table.Column<int>(type: "int", nullable: true),
                    ClaimStatusTransactionLineItemStatusChangẹId = table.Column<int>(type: "int", nullable: true),
                    ClaimLineItemStatusValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedMemberId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimStatusExceptionReasonCategoryId = table.Column<int>(type: "int", nullable: true),
                    ExceptionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionRemark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemarkCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ReasonCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ReasonDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemarkDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalNonAllowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAllowedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalMemberResponsibilityAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DeductibleAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CopayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CoinsuranceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CobAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PenalityAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineItemChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LineItemPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    DiagnosisCode = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    ServiceLineDenialReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateReceived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEntered = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiagnosisDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatePaid = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AuthorizationFound = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    AuthorizationStatusId = table.Column<int>(type: "int", nullable: true),
                    AuthorizationNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EligibilityInsurance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EligibilityPolicyNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    EligibilityFromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastActiveEligibleDateRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CobLastVerified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrimaryPayer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryPolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EligibilityStatus = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Icn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferringProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentCoverage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HippaStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingProviderNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateClaimFinalized = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CobaInsurerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CobaInsurerEffective = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_EligibilityFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_EligibilityTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_DeductibleFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_DeductibleToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartA_RemainingDeductible = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PartB_EligibilityFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_EligibilityTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_DeductibleFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_DeductibleTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PartB_RemainingDeductible = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtCapYearFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtCapYearTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtCapUsedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PtCapYearFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PtCapYearTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PtCapUsedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InputDataListIndex = table.Column<int>(type: "int", nullable: true),
                    InputDataFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalClaimPaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WriteoffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactions_AuthorizationStatuses_AuthorizationStatusId",
                        column: x => x.AuthorizationStatusId,
                        principalSchema: "dbo",
                        principalTable: "AuthorizationStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactions_ClaimLineItemStatuses_ClaimLineItemStatusId",
                        column: x => x.ClaimLineItemStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimLineItemStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactions_ClaimStatusBatchClaims_ClaimStatusBatchClaimId",
                        column: x => x.ClaimStatusBatchClaimId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusBatchClaims",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactions_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                        column: x => x.ClaimStatusExceptionReasonCategoryId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusExceptionReasonCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactions_ClaimStatusTransactionLineItemStatusChangẹs_ClaimStatusTransactionLineItemStatusChangẹId",
                        column: x => x.ClaimStatusTransactionLineItemStatusChangẹId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusTransactionLineItemStatusChangẹs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClaimStatusTransactions_ClaimStatuses_TotalClaimStatusId",
                        column: x => x.TotalClaimStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClaimStatusWorkstationNotes",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimStatusTransactionId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    NoteContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoteTs = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusWorkstationNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClaimStatusWorkstationNotes_ClaimStatusTransactions_ClaimStatusTransactionId",
                        column: x => x.ClaimStatusTransactionId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimStatusTransactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientApiIntegrationKeys",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApiIntegrationId = table.Column<int>(type: "int", nullable: false),
                    ApiKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ApiSecret = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ApiUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiVersion = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    ApiUsername = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    ApiPassword = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientApiIntegrationKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientApiIntegrationKeys_ApiIntegrations_ApiIntegrationId",
                        column: x => x.ApiIntegrationId,
                        principalSchema: "dbo",
                        principalTable: "ApiIntegrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientApplicationFeatures",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationFeatureId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientApplicationFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientApplicationFeatures_ApplicationFeatures_ApplicationFeatureId",
                        column: x => x.ApplicationFeatureId,
                        principalSchema: "dbo",
                        principalTable: "ApplicationFeatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientAuthTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthTypeId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAuthTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAuthTypes_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientCptCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduledFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CptCodeGroupId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LookupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeOfServiceId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCptCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientCptCodes_TypesOfService_TypeOfServiceId",
                        column: x => x.TypeOfServiceId,
                        principalSchema: "dbo",
                        principalTable: "TypesOfService",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientDocumentType",
                schema: "dbo",
                columns: table => new
                {
                    ClientsId = table.Column<int>(type: "int", nullable: false),
                    DocumentTypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientDocumentType", x => new { x.ClientsId, x.DocumentTypesId });
                    table.ForeignKey(
                        name: "FK_ClientDocumentType_DocumentTypes_DocumentTypesId",
                        column: x => x.DocumentTypesId,
                        principalSchema: "dbo",
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientEmployeeDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "ClientEmployeeKpis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    AverageDailyClaimsWorkedKpi = table.Column<int>(type: "int", nullable: true),
                    MonthlyCashCollectionsKpi = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEmployeeKpis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientInsuranceRpaConfigurations",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    RpaInsuranceId = table.Column<int>(type: "int", nullable: false),
                    TransactionTypeId = table.Column<int>(type: "int", nullable: false),
                    AuthTypeId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureReported = table.Column<bool>(type: "bit", nullable: false),
                    FailureMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryWarningReported = table.Column<bool>(type: "bit", nullable: false),
                    ReportFailureToEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsConfigInUse = table.Column<bool>(type: "bit", nullable: false),
                    UseOffHoursOnly = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInsuranceRpaConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInsuranceRpaConfigurations_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientInsuranceRpaConfigurations_RpaInsurances_RpaInsuranceId",
                        column: x => x.RpaInsuranceId,
                        principalSchema: "IntegratedServices",
                        principalTable: "RpaInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientInsuranceRpaConfigurations_TransactionTypes_TransactionTypeId",
                        column: x => x.TransactionTypeId,
                        principalSchema: "IntegratedServices",
                        principalTable: "TransactionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientInsurances",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LookupName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    FaxNumber = table.Column<long>(type: "bigint", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EcsId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RpaInsuranceId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInsurances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInsurances_RpaInsurances_RpaInsuranceId",
                        column: x => x.RpaInsuranceId,
                        principalSchema: "IntegratedServices",
                        principalTable: "RpaInsurances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientKpi",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DailyClaimCount = table.Column<int>(type: "int", nullable: true),
                    MonthlyCashCollection = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VolumeCredentialDenials = table.Column<int>(type: "int", nullable: true),
                    CredentialDenialsCashValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClaimDenialPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DemographicDenialPercentage = table.Column<int>(type: "int", nullable: true),
                    CodingDenialPercentage = table.Column<int>(type: "int", nullable: true),
                    AverageSubmitDays = table.Column<int>(type: "int", nullable: true),
                    AverageDaysInReceivables = table.Column<int>(type: "int", nullable: true),
                    AR90DaysInsurancePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AR90DaysSelfPayPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AR180DaysInsurancePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AR180DaysSelfPayPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CleanClaimRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientKpi", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientLocations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfficePhoneNumber = table.Column<long>(type: "bigint", maxLength: 14, nullable: true),
                    OfficeFaxNumber = table.Column<long>(type: "bigint", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    EligibilityLocationId = table.Column<int>(type: "int", nullable: true),
                    Npi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocations_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientLocationServiceTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthTypeId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocationServiceTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocationServiceTypes_AuthTypes_AuthTypeId",
                        column: x => x.AuthTypeId,
                        principalSchema: "dbo",
                        principalTable: "AuthTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientLocationServiceTypes_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPlacesOfService",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LookupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfServiceCodeId = table.Column<int>(type: "int", nullable: false),
                    Npi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPlacesOfService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPlacesOfService_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientPlacesOfService_PlaceOfServiceCodes_PlaceOfServiceCodeId",
                        column: x => x.PlaceOfServiceCodeId,
                        principalSchema: "dbo",
                        principalTable: "PlaceOfServiceCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientProviderLocations",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientLocationId = table.Column<int>(type: "int", nullable: false),
                    ClientProviderId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProviderLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProviderLocations_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientQuestionnaireCategories",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientQuestionnaireId = table.Column<int>(type: "int", nullable: false),
                    QuestionCategoryId = table.Column<int>(type: "int", nullable: false),
                    CategoryOrder = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientQuestionnaireCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientQuestionnaireCategories_QuestionCategories_QuestionCategoryId",
                        column: x => x.QuestionCategoryId,
                        principalSchema: "dbo",
                        principalTable: "QuestionCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientQuestionnaireCategoryQuestions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryQuestionOrder = table.Column<int>(type: "int", nullable: false),
                    ClientQuestionnaireCategoryId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientQuestionnaireCategoryQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientQuestionnaireCategoryQuestions_ClientQuestionnaireCategories_ClientQuestionnaireCategoryId",
                        column: x => x.ClientQuestionnaireCategoryId,
                        principalSchema: "dbo",
                        principalTable: "ClientQuestionnaireCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientQuestionnaireCategoryQuestions_QuestionCategories_QuestionCategoryId",
                        column: x => x.QuestionCategoryId,
                        principalSchema: "dbo",
                        principalTable: "QuestionCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientQuestionnaireCategoryQuestionOptions",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientQuestionnaireCategoryQuestionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDefaultAnswer = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientQuestionnaireCategoryQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientQuestionnaireCategoryQuestionOptions_ClientQuestionnaireCategoryQuestions_ClientQuestionnaireCategoryQuestionId",
                        column: x => x.ClientQuestionnaireCategoryQuestionId,
                        principalSchema: "dbo",
                        principalTable: "ClientQuestionnaireCategoryQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientQuestionnaires",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RelatedState = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientQuestionnaires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClientCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TaxId = table.Column<int>(type: "int", nullable: true),
                    NpiNumber = table.Column<int>(type: "int", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    PhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    FaxNumber = table.Column<long>(type: "bigint", nullable: true),
                    ClientQuestionnaireId = table.Column<int>(type: "int", nullable: true),
                    ClientLevelKpisId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Clients_ClientQuestionnaires_ClientQuestionnaireId",
                        column: x => x.ClientQuestionnaireId,
                        principalSchema: "dbo",
                        principalTable: "ClientQuestionnaires",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ClientReportFilters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
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
                name: "FlowLogEntries",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    FlowId = table.Column<int>(type: "int", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowLogEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowLogEntries_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FlowLogEntries_Flows_FlowId",
                        column: x => x.FlowId,
                        principalSchema: "IntegratedServices",
                        principalTable: "Flows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InputDocuments",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ByteLength = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputDocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputDocuments_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InputDocuments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InputDocuments_InputDocumentTypes_InputDocumentTypeId",
                        column: x => x.InputDocumentTypeId,
                        principalSchema: "IntegratedServices",
                        principalTable: "InputDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageNo = table.Column<int>(type: "int", nullable: false),
                    FromUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Resolved = table.Column<bool>(type: "bit", nullable: true),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorizationId = table.Column<int>(type: "int", nullable: false),
                    NoteUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoteTs = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    NoteContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Authorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalSchema: "dbo",
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SocialSecurityNumber = table.Column<int>(type: "int", nullable: true),
                    HomePhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    MobilePhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    OfficePhoneNumber = table.Column<long>(type: "bigint", nullable: true),
                    FaxNumber = table.Column<long>(type: "bigint", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: true),
                    GenderIdentityId = table.Column<int>(type: "int", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalSchema: "dbo",
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Persons_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Persons_GenderIdentities_GenderIdentityId",
                        column: x => x.GenderIdentityId,
                        principalSchema: "dbo",
                        principalTable: "GenderIdentities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClients",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AlphabeticalSplit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimCountRequired = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    EmployeeManagerId = table.Column<int>(type: "int", nullable: true),
                    EmployeeLevelId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_EmployeeLevels_EmployeeLevelId",
                        column: x => x.EmployeeLevelId,
                        principalTable: "EmployeeLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_EmployeeManagerId",
                        column: x => x.EmployeeManagerId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Employees_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    Credentials = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Npi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    SupervisingProviderId = table.Column<int>(type: "int", nullable: true),
                    Upin = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    TaxonomyCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    License = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Providers_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Providers_Providers_SupervisingProviderId",
                        column: x => x.SupervisingProviderId,
                        principalSchema: "dbo",
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Providers_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "dbo",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReferringProviders",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    Credentials = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Npi = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Upin = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    TaxId = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: true),
                    TaxonomyCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    License = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    ExternalId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReferringProviders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReferringProviders_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReferringProviders_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReferringProviders_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "dbo",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResponsibleParties",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, computedColumnSql: "CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)"),
                    ExternalId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    SocialSecurityNumber = table.Column<decimal>(type: "decimal(18,2)", maxLength: 9, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponsibleParties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResponsibleParties_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ResponsibleParties_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientUserApplicationReports",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserClientId = table.Column<int>(type: "int", nullable: false),
                    ApplicationReportId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUserApplicationReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientUserApplicationReports_ApplicationReports_ApplicationReportId",
                        column: x => x.ApplicationReportId,
                        principalSchema: "dbo",
                        principalTable: "ApplicationReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientUserApplicationReports_UserClients_UserClientId",
                        column: x => x.UserClientId,
                        principalSchema: "dbo",
                        principalTable: "UserClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeClientInsurances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClientInsurances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeClientInsurances_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeClientInsurances_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeClientLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClientLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeClientLocations_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeClientLocations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeClients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeClients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployeeClients_Employees_EmployeeId",
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
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
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
                name: "Patients",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false, computedColumnSql: "CHAR(65 + ID/260000) +  CHAR(65 + ID%260000/10000) + RIGHT('0000' + CAST(ID % 10000 AS VARCHAR),4)"),
                    ExternalId = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    AdministrativeGenderId = table.Column<int>(type: "int", nullable: false),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    InsurancePolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceGroupNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BenefitsCheckedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResponsiblePartyRelationshipToPatient = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    ResponsiblePartyId = table.Column<int>(type: "int", nullable: true),
                    PrimaryProviderId = table.Column<int>(type: "int", nullable: true),
                    ReferringProviderId = table.Column<int>(type: "int", nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_AdministrativeGenders_AdministrativeGenderId",
                        column: x => x.AdministrativeGenderId,
                        principalSchema: "dbo",
                        principalTable: "AdministrativeGenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Persons_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "dbo",
                        principalTable: "Persons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_Providers_PrimaryProviderId",
                        column: x => x.PrimaryProviderId,
                        principalSchema: "dbo",
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_ReferringProviders_ReferringProviderId",
                        column: x => x.ReferringProviderId,
                        principalSchema: "dbo",
                        principalTable: "ReferringProviders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Patients_ResponsibleParties_ResponsiblePartyId",
                        column: x => x.ResponsiblePartyId,
                        principalSchema: "dbo",
                        principalTable: "ResponsibleParties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EmployeeClaimStatusExceptionReasonCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    ClaimStatusExceptionReasonCategoryId = table.Column<int>(type: "int", nullable: true),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                name: "EmployeeClientAlphaSplits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    BeginAlpha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndAlpha = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClientAlphaSplits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeClientAlphaSplits_EmployeeClients_EmployeeClientId",
                        column: x => x.EmployeeClientId,
                        principalTable: "EmployeeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    AuthorizationId = table.Column<int>(type: "int", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: false),
                    DocumentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Authorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalSchema: "dbo",
                        principalTable: "Authorizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalSchema: "dbo",
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "InsuranceCards",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CardholderId = table.Column<int>(type: "int", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    GroupNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardholderRelationshipToPatient = table.Column<int>(type: "int", nullable: true),
                    MemberNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCoverageTypes = table.Column<byte>(type: "tinyint", nullable: true),
                    EffectiveStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InsuranceCardOrder = table.Column<int>(type: "int", nullable: true),
                    CopayAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceCards_Cardholders_CardholderId",
                        column: x => x.CardholderId,
                        principalSchema: "dbo",
                        principalTable: "Cardholders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceCards_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceCards_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InsuranceCards_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientQuestionnaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientQuestionnaireId = table.Column<int>(type: "int", nullable: false),
                    AuthorizationId = table.Column<int>(type: "int", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientQuestionnaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientQuestionnaire_Authorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalSchema: "dbo",
                        principalTable: "Authorizations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientQuestionnaire_ClientQuestionnaires_ClientQuestionnaireId",
                        column: x => x.ClientQuestionnaireId,
                        principalSchema: "dbo",
                        principalTable: "ClientQuestionnaires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientQuestionnaire_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientQuestionnaireAnswers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomAnswer = table.Column<bool>(type: "bit", nullable: false),
                    PatientQuestionnaireId = table.Column<int>(type: "int", nullable: false),
                    ClientQuestionnaireCategoryQuestionId = table.Column<int>(type: "int", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TenantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientQuestionnaireAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientQuestionnaireAnswers_ClientQuestionnaireCategoryQuestions_ClientQuestionnaireCategoryQuestionId",
                        column: x => x.ClientQuestionnaireCategoryQuestionId,
                        principalSchema: "dbo",
                        principalTable: "ClientQuestionnaireCategoryQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientQuestionnaireAnswers_PatientQuestionnaire_PatientQuestionnaireId",
                        column: x => x.PatientQuestionnaireId,
                        principalTable: "PatientQuestionnaire",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "DocumentTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Description", "IsDefault", "LastModifiedBy", "LastModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Daily Living Activities-20", false, null, null, "DLA20" },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Referral Document", false, null, null, "Referral" },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Insurance Card Document", false, null, null, "Insurance Card" },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Treatment Plan Document", false, null, null, "Treatment Plan" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "GenderIdentities",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "LastModifiedBy", "LastModifiedOn", "Name" },
                values: new object[,]
                {
                    { 0, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Unknown" },
                    { 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "TransgenderFemale" },
                    { 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "TransgenderMale" },
                    { 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "NonBinary" },
                    { 4, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Male" },
                    { 5, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Female" },
                    { 6, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Other" },
                    { 7, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "NonDisclose" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_ClientId",
                schema: "dbo",
                table: "Addresses",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_StateId",
                schema: "dbo",
                table: "Addresses",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationReports_ApplicationFeatureId",
                schema: "dbo",
                table: "ApplicationReports",
                column: "ApplicationFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationClientCptCode_AuthorizationId",
                table: "AuthorizationClientCptCode",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationClientCptCode_ClientCptCodeId",
                table: "AuthorizationClientCptCode",
                column: "ClientCptCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_AuthorizationStatusId",
                schema: "dbo",
                table: "Authorizations",
                column: "AuthorizationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_AuthTypeId",
                schema: "dbo",
                table: "Authorizations",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ClientId",
                schema: "dbo",
                table: "Authorizations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ClientLocationId",
                schema: "dbo",
                table: "Authorizations",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_ClientPlaceOfServiceId",
                schema: "dbo",
                table: "Authorizations",
                column: "ClientPlaceOfServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_PatientId",
                schema: "dbo",
                table: "Authorizations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_ClientId",
                schema: "dbo",
                table: "Cardholders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_PersonId",
                schema: "dbo",
                table: "Cardholders",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatches_AuthTypeId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatches_ChargeEntryRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches",
                column: "ChargeEntryRpaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_AuthTypeId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_ChargeEntryBatchId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "ChargeEntryBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_ChargeEntryRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "ChargeEntryRpaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_DbOperationId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "DbOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryRpaConfigurations_AuthTypeId",
                schema: "IntegratedServices",
                table: "ChargeEntryRpaConfigurations",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryRpaConfigurations_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryRpaConfigurations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryRpaConfigurations_RelativeDateRangeId",
                schema: "IntegratedServices",
                table: "ChargeEntryRpaConfigurations",
                column: "RelativeDateRangeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryRpaConfigurations_RpaTypeId",
                schema: "IntegratedServices",
                table: "ChargeEntryRpaConfigurations",
                column: "RpaTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryRpaConfigurations_TransactionTypeId",
                schema: "IntegratedServices",
                table: "ChargeEntryRpaConfigurations",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactionHistories_ChargeEntryTransactionId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                column: "ChargeEntryTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactionHistories_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactions_ChargeEntryBatchId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                column: "ChargeEntryBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactions_ChargeEntryTransactionEndDateTimeUtc",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                column: "ChargeEntryTransactionEndDateTimeUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactions_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactions_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaimRoot_ClientId",
                table: "ClaimStatusBatchClaimRoot",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaimRoot_ClientLocationId",
                table: "ClaimStatusBatchClaimRoot",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaimRoot_ClientProviderId",
                table: "ClaimStatusBatchClaimRoot",
                column: "ClientProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClaimBilledOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimBilledOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClaimStatusBatchClaimRootId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimStatusBatchClaimRootId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClaimStatusBatchId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimStatusBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClaimStatusTransactionId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimStatusTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClientProviderId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_EntryMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "EntryMd5Hash");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_PatientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_AbortedOnUtc",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "AbortedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_AssignedClientRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "AssignedClientRpaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_AssignedDateTimeUtc",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "AssignedDateTimeUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_InputDocumentId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "InputDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_IsDeleted",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_Priority",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "RpaInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_AbortedOnUtc",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AbortedOnUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_AssignedClientRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AssignedClientRpaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_AssignedDateTimeUtc",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AssignedDateTimeUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_ClaimStatusBatchId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "ClaimStatusBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_DbOperationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "DbOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusExceptionReasonCategoryMaps_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "ClaimStatusExceptionReasonCategoryMaps",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_AuthorizationStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "AuthorizationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "ClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_ClaimStatusTransactionId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "ClaimStatusTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_DbOperationId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "DbOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_TotalClaimStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "TotalClaimStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_DbOperationId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "DbOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_PreviousClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "PreviousClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_UpdatedClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "UpdatedClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_AuthorizationStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "AuthorizationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_ClaimStatusBatchClaimId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClaimStatusBatchClaimId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_ClaimStatusTransactionEndDateTimeUtc",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClaimStatusTransactionEndDateTimeUtc");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_ClaimStatusTransactionLineItemStatusChangẹId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClaimStatusTransactionLineItemStatusChangẹId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_TotalClaimStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "TotalClaimStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusWorkstationNotes_ClaimStatusTransactionId",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes",
                column: "ClaimStatusTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusWorkstationNotes_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusWorkstationNotes_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientApiIntegrationKeys_ApiIntegrationId",
                schema: "dbo",
                table: "ClientApiIntegrationKeys",
                column: "ApiIntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientApiIntegrationKeys_ClientId_ApiIntegrationId_ApiVersion",
                schema: "dbo",
                table: "ClientApiIntegrationKeys",
                columns: new[] { "ClientId", "ApiIntegrationId", "ApiVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_ClientApplicationFeatures_ApplicationFeatureId",
                schema: "dbo",
                table: "ClientApplicationFeatures",
                column: "ApplicationFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientApplicationFeatures_ClientId",
                schema: "dbo",
                table: "ClientApplicationFeatures",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAuthTypes_AuthTypeId",
                schema: "dbo",
                table: "ClientAuthTypes",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAuthTypes_ClientId",
                schema: "dbo",
                table: "ClientAuthTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCptCodes_ClientId",
                schema: "dbo",
                table: "ClientCptCodes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCptCodes_TypeOfServiceId",
                schema: "dbo",
                table: "ClientCptCodes",
                column: "TypeOfServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientDocumentType_DocumentTypesId",
                schema: "dbo",
                table: "ClientDocumentType",
                column: "DocumentTypesId");

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
                name: "IX_ClientInsuranceRpaConfigurations_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientInsuranceId_RpaInsuranceId_TransactionTypeId_AuthTypeId_ExternalId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                columns: new[] { "ClientInsuranceId", "RpaInsuranceId", "TransactionTypeId", "AuthTypeId", "ExternalId" },
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_RpaInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "RpaInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_TransactionTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "TransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_ClientId",
                schema: "dbo",
                table: "ClientInsurances",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_RpaInsuranceId",
                schema: "dbo",
                table: "ClientInsurances",
                column: "RpaInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientKpi_ClientId",
                schema: "dbo",
                table: "ClientKpi",
                column: "ClientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_AddressId",
                schema: "dbo",
                table: "ClientLocations",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_ClientId",
                schema: "dbo",
                table: "ClientLocations",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationServiceTypes_AuthTypeId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                column: "AuthTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationServiceTypes_ClientId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationServiceTypes_ClientLocationId_AuthTypeId_ClientId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                columns: new[] { "ClientLocationId", "AuthTypeId", "ClientId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlacesOfService_AddressId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlacesOfService_ClientId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlacesOfService_PlaceOfServiceCodeId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "PlaceOfServiceCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviderLocations_ClientLocationId_ClientProviderId",
                schema: "dbo",
                table: "ClientProviderLocations",
                columns: new[] { "ClientLocationId", "ClientProviderId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviderLocations_ClientProviderId",
                schema: "dbo",
                table: "ClientProviderLocations",
                column: "ClientProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientQuestionnaireCategories_ClientQuestionnaireId",
                schema: "dbo",
                table: "ClientQuestionnaireCategories",
                column: "ClientQuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientQuestionnaireCategories_QuestionCategoryId",
                schema: "dbo",
                table: "ClientQuestionnaireCategories",
                column: "QuestionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientQuestionnaireCategoryQuestionOptions_ClientQuestionnaireCategoryQuestionId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestionOptions",
                column: "ClientQuestionnaireCategoryQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientQuestionnaireCategoryQuestions_ClientQuestionnaireCategoryId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestions",
                column: "ClientQuestionnaireCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientQuestionnaireCategoryQuestions_QuestionCategoryId",
                schema: "dbo",
                table: "ClientQuestionnaireCategoryQuestions",
                column: "QuestionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientQuestionnaires_ClientId",
                schema: "dbo",
                table: "ClientQuestionnaires",
                column: "ClientId");

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
                name: "IX_Clients_AddressId",
                schema: "dbo",
                table: "Clients",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_ClientQuestionnaireId",
                schema: "dbo",
                table: "Clients",
                column: "ClientQuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserApplicationReports_ApplicationReportId",
                schema: "dbo",
                table: "ClientUserApplicationReports",
                column: "ApplicationReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserApplicationReports_UserClientId",
                schema: "dbo",
                table: "ClientUserApplicationReports",
                column: "UserClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcurrentAuthorization_InitialAuthorizationId",
                table: "ConcurrentAuthorization",
                column: "InitialAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcurrentAuthorization_SucceededAuthorizationId",
                table: "ConcurrentAuthorization",
                column: "SucceededAuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AuthorizationId",
                schema: "dbo",
                table: "Documents",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DocumentTypeId",
                schema: "dbo",
                table: "Documents",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PatientId",
                schema: "dbo",
                table: "Documents",
                column: "PatientId");

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
                name: "IX_EmployeeClientAlphaSplits_EmployeeClientId",
                table: "EmployeeClientAlphaSplits",
                column: "EmployeeClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientInsurances_ClientInsuranceId",
                table: "EmployeeClientInsurances",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientInsurances_EmployeeId",
                table: "EmployeeClientInsurances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientLocations_ClientLocationId",
                table: "EmployeeClientLocations",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientLocations_EmployeeId",
                table: "EmployeeClientLocations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClients_ClientId",
                table: "EmployeeClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClients_EmployeeId",
                table: "EmployeeClients",
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

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeLevelId",
                table: "Employees",
                column: "EmployeeLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeManagerId",
                table: "Employees",
                column: "EmployeeManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_PersonId",
                table: "Employees",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowLogEntries_ClientId",
                schema: "IntegratedServices",
                table: "FlowLogEntries",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowLogEntries_FlowId",
                schema: "IntegratedServices",
                table: "FlowLogEntries",
                column: "FlowId");

            migrationBuilder.CreateIndex(
                name: "IX_InputDocuments_ClientId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InputDocuments_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_InputDocuments_InputDocumentTypeId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "InputDocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_CardholderId",
                schema: "dbo",
                table: "InsuranceCards",
                column: "CardholderId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_ClientId",
                schema: "dbo",
                table: "InsuranceCards",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_ClientInsuranceId",
                schema: "dbo",
                table: "InsuranceCards",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_PatientId",
                schema: "dbo",
                table: "InsuranceCards",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ClientId",
                schema: "dbo",
                table: "Messages",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AuthorizationId",
                schema: "dbo",
                table: "Notes",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ClientId",
                schema: "dbo",
                table: "Notes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQuestionnaire_AuthorizationId",
                table: "PatientQuestionnaire",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQuestionnaire_ClientQuestionnaireId",
                table: "PatientQuestionnaire",
                column: "ClientQuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQuestionnaire_PatientId",
                table: "PatientQuestionnaire",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQuestionnaireAnswers_ClientQuestionnaireCategoryQuestionId",
                schema: "dbo",
                table: "PatientQuestionnaireAnswers",
                column: "ClientQuestionnaireCategoryQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientQuestionnaireAnswers_PatientQuestionnaireId",
                schema: "dbo",
                table: "PatientQuestionnaireAnswers",
                column: "PatientQuestionnaireId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_AdministrativeGenderId",
                schema: "dbo",
                table: "Patients",
                column: "AdministrativeGenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ClientId",
                schema: "dbo",
                table: "Patients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ClientInsuranceId",
                schema: "dbo",
                table: "Patients",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PersonId",
                schema: "dbo",
                table: "Patients",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_PrimaryProviderId",
                schema: "dbo",
                table: "Patients",
                column: "PrimaryProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ReferringProviderId",
                schema: "dbo",
                table: "Patients",
                column: "ReferringProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ResponsiblePartyId",
                schema: "dbo",
                table: "Patients",
                column: "ResponsiblePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_AddressId",
                schema: "dbo",
                table: "Persons",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_ClientId",
                schema: "dbo",
                table: "Persons",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_GenderIdentityId",
                schema: "dbo",
                table: "Persons",
                column: "GenderIdentityId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ClientId",
                schema: "dbo",
                table: "Providers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_PersonId",
                schema: "dbo",
                table: "Providers",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_SpecialtyId",
                schema: "dbo",
                table: "Providers",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_SupervisingProviderId",
                schema: "dbo",
                table: "Providers",
                column: "SupervisingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferringProviders_ClientId",
                schema: "dbo",
                table: "ReferringProviders",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferringProviders_PersonId",
                schema: "dbo",
                table: "ReferringProviders",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ReferringProviders_SpecialtyId",
                schema: "dbo",
                table: "ReferringProviders",
                column: "SpecialtyId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_ClientId",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_PersonId",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClients_ClientId",
                schema: "dbo",
                table: "UserClients",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Clients_ClientId",
                schema: "dbo",
                table: "Addresses",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationClientCptCode_Authorizations_AuthorizationId",
                table: "AuthorizationClientCptCode",
                column: "AuthorizationId",
                principalSchema: "dbo",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationClientCptCode_ClientCptCodes_ClientCptCodeId",
                table: "AuthorizationClientCptCode",
                column: "ClientCptCodeId",
                principalSchema: "dbo",
                principalTable: "ClientCptCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_ClientLocations_ClientLocationId",
                schema: "dbo",
                table: "Authorizations",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_ClientPlacesOfService_ClientPlaceOfServiceId",
                schema: "dbo",
                table: "Authorizations",
                column: "ClientPlaceOfServiceId",
                principalSchema: "dbo",
                principalTable: "ClientPlacesOfService",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Clients_ClientId",
                schema: "dbo",
                table: "Authorizations",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Authorizations_Patients_PatientId",
                schema: "dbo",
                table: "Authorizations",
                column: "PatientId",
                principalSchema: "dbo",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cardholders_Clients_ClientId",
                schema: "dbo",
                table: "Cardholders",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cardholders_Persons_PersonId",
                schema: "dbo",
                table: "Cardholders",
                column: "PersonId",
                principalSchema: "dbo",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryBatches_ChargeEntryRpaConfigurations_ChargeEntryRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches",
                column: "ChargeEntryRpaConfigurationId",
                principalSchema: "IntegratedServices",
                principalTable: "ChargeEntryRpaConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryBatchHistories_ChargeEntryRpaConfigurations_ChargeEntryRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "ChargeEntryRpaConfigurationId",
                principalSchema: "IntegratedServices",
                principalTable: "ChargeEntryRpaConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryBatchHistories_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryRpaConfigurations_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryRpaConfigurations",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryTransactionHistories_ChargeEntryTransactions_ChargeEntryTransactionId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                column: "ChargeEntryTransactionId",
                principalSchema: "IntegratedServices",
                principalTable: "ChargeEntryTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryTransactionHistories_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChargeEntryTransactions_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaimRoot_ClientLocations_ClientLocationId",
                table: "ClaimStatusBatchClaimRoot",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaimRoot_Clients_ClientId",
                table: "ClaimStatusBatchClaimRoot",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaimRoot_Providers_ClientProviderId",
                table: "ClaimStatusBatchClaimRoot",
                column: "ClientProviderId",
                principalSchema: "dbo",
                principalTable: "Providers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClaimStatusBatches_ClaimStatusBatchId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimStatusBatchId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimStatusBatches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClaimStatusTransactions_ClaimStatusTransactionId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimStatusTransactionId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimStatusTransactions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientLocations_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_Patients_PatientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "PatientId",
                principalSchema: "dbo",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_Providers_ClientProviderId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientProviderId",
                principalSchema: "dbo",
                principalTable: "Providers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_ClientInsuranceRpaConfigurations_AssignedClientRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "AssignedClientRpaConfigurationId",
                principalSchema: "IntegratedServices",
                principalTable: "ClientInsuranceRpaConfigurations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_InputDocuments_InputDocumentId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "InputDocumentId",
                principalSchema: "IntegratedServices",
                principalTable: "InputDocuments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchHistories_ClientInsuranceRpaConfigurations_AssignedClientRpaConfigurationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AssignedClientRpaConfigurationId",
                principalSchema: "IntegratedServices",
                principalTable: "ClientInsuranceRpaConfigurations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchHistories_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchHistories_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTransactionHistories_ClaimStatusTransactions_ClaimStatusTransactionId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "ClaimStatusTransactionId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimStatusTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTransactionHistories_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTransactionLineItemStatusChangẹs_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusTransactions_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusWorkstationNotes_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientApiIntegrationKeys_Clients_ClientId",
                schema: "dbo",
                table: "ClientApiIntegrationKeys",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientApplicationFeatures_Clients_ClientId",
                schema: "dbo",
                table: "ClientApplicationFeatures",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAuthTypes_Clients_ClientId",
                schema: "dbo",
                table: "ClientAuthTypes",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientCptCodes_Clients_ClientId",
                schema: "dbo",
                table: "ClientCptCodes",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientDocumentType_Clients_ClientsId",
                schema: "dbo",
                table: "ClientDocumentType",
                column: "ClientsId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientEmployeeDepartment_EmployeeClients_EmployeeClientId",
                table: "ClientEmployeeDepartment",
                column: "EmployeeClientId",
                principalTable: "EmployeeClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientEmployeeKpis_EmployeeClients_EmployeeClientId",
                table: "ClientEmployeeKpis",
                column: "EmployeeClientId",
                principalTable: "EmployeeClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientInsurances_ClientInsuranceId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsurances_Clients_ClientId",
                schema: "dbo",
                table: "ClientInsurances",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientKpi_Clients_ClientId",
                schema: "dbo",
                table: "ClientKpi",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocations_Clients_ClientId",
                schema: "dbo",
                table: "ClientLocations",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationServiceTypes_Clients_ClientId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientPlacesOfService_Clients_ClientId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientProviderLocations_Providers_ClientProviderId",
                schema: "dbo",
                table: "ClientProviderLocations",
                column: "ClientProviderId",
                principalSchema: "dbo",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientQuestionnaireCategories_ClientQuestionnaires_ClientQuestionnaireId",
                schema: "dbo",
                table: "ClientQuestionnaireCategories",
                column: "ClientQuestionnaireId",
                principalSchema: "dbo",
                principalTable: "ClientQuestionnaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientQuestionnaires_Clients_ClientId",
                schema: "dbo",
                table: "ClientQuestionnaires",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Clients_ClientId",
                schema: "dbo",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaimRoot_Clients_ClientId",
                table: "ClaimStatusBatchClaimRoot");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatches_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTransactionLineItemStatusChangẹs_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTransactions_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsurances_Clients_ClientId",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocations_Clients_ClientId",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientQuestionnaires_Clients_ClientId",
                schema: "dbo",
                table: "ClientQuestionnaires");

            migrationBuilder.DropForeignKey(
                name: "FK_InputDocuments_Clients_ClientId",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Clients_ClientId",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Persons_Clients_ClientId",
                schema: "dbo",
                table: "Persons");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Clients_ClientId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_ReferringProviders_Clients_ClientId",
                schema: "dbo",
                table: "ReferringProviders");

            migrationBuilder.DropForeignKey(
                name: "FK_ResponsibleParties_Clients_ClientId",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_States_StateId",
                schema: "dbo",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatches_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTransactions_AuthorizationStatuses_AuthorizationStatusId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaimRoot_ClientLocations_ClientLocationId",
                table: "ClaimStatusBatchClaimRoot");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientLocations_ClientLocationId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_Patients_PatientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Persons_PersonId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusTransactionLineItemStatusChangẹs_DbOperations_DbOperationId",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_TransactionTypes_TransactionTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaimRoot_Providers_ClientProviderId",
                table: "ClaimStatusBatchClaimRoot");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_Providers_ClientProviderId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClaimStatusBatchClaimRoot_ClaimStatusBatchClaimRootId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClaimStatusBatches_ClaimStatusBatchId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClaimStatusTransactions_ClaimStatusTransactionId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropTable(
                name: "AddressTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuditTrails");

            migrationBuilder.DropTable(
                name: "AuthorizationClientCptCode");

            migrationBuilder.DropTable(
                name: "ChargeEntryBatchHistories",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ChargeEntryTransactionHistories",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusBatchHistories",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusExceptionReasonCategoryMaps",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusTransactionHistories",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusWorkstationNotes",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClientApiIntegrationKeys",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientApplicationFeatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientAuthTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientDocumentType",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientEmployeeDepartment");

            migrationBuilder.DropTable(
                name: "ClientEmployeeKpis");

            migrationBuilder.DropTable(
                name: "ClientKpi",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientLocationServiceTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientProviderLocations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientQuestionnaireCategoryQuestionOptions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientReportFilters",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientUserApplicationReports",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ConcurrentAuthorization");

            migrationBuilder.DropTable(
                name: "CptCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeClaimStatusExceptionReasonCategories");

            migrationBuilder.DropTable(
                name: "EmployeeClientAlphaSplits");

            migrationBuilder.DropTable(
                name: "EmployeeClientInsurances");

            migrationBuilder.DropTable(
                name: "EmployeeClientLocations");

            migrationBuilder.DropTable(
                name: "EmployeeDepartments");

            migrationBuilder.DropTable(
                name: "FlowLogEntries",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "InsuranceCards",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Notes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PatientQuestionnaireAnswers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ReportCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserAlerts",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientCptCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ChargeEntryTransactions",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ApiIntegrations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Report",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ApplicationReports",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserClients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DocumentTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "EmployeeClients");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Flows",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "Cardholders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientQuestionnaireCategoryQuestions",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PatientQuestionnaire");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "TypesOfService",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ChargeEntryBatches",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ApplicationFeatures",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "ClientQuestionnaireCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Authorizations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ChargeEntryRpaConfigurations",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "EmployeeLevels");

            migrationBuilder.DropTable(
                name: "QuestionCategories",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientPlacesOfService",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "RelativeDateRange");

            migrationBuilder.DropTable(
                name: "RpaTypes",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "PlaceOfServiceCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientQuestionnaires",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "States",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuthTypes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AuthorizationStatuses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientLocations",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Patients",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AdministrativeGenders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ReferringProviders",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ResponsibleParties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "GenderIdentities",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "DbOperations",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "TransactionTypes",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "Providers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Specialties",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClaimStatusBatchClaimRoot");

            migrationBuilder.DropTable(
                name: "ClaimStatusBatches",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClientInsuranceRpaConfigurations",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "InputDocuments",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "InputDocumentTypes",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusTransactions",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusBatchClaims",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusExceptionReasonCategories",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatusTransactionLineItemStatusChangẹs",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClaimStatuses",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "ClientInsurances",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClaimLineItemStatuses",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "RpaInsurances",
                schema: "IntegratedServices");
        }
    }
}
