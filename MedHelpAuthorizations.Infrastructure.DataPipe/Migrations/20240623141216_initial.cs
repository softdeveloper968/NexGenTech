using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_AdjustmentCodes",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Type = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_AdjustmentCodes", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_CardHolders",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardHolderId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Employer = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    DateOfBirth = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_CardHolders", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Charges",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChargeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponsiblePartyId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfServiceCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChargeAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientInsuranceCard1Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Insurance1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientInsuranceCard2Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Insurance2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientInsuranceCard3Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Insurance3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientFirstBillDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PatientLastBillDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    BilledToInsuranceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenderingProviderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceOfServiceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfServiceFrom = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    DateOfServiceTo = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    EntryDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Charges", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ClaimAdjustments",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimAdjustmentsId = table.Column<int>(type: "int", nullable: true),
                    ClaimChargeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimNumber = table.Column<int>(type: "int", nullable: true),
                    AdjustmentCodeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemittenceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    EntryDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ModifiedDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ClaimAdjustments", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ClaimPayments",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClaimPaymentId = table.Column<int>(type: "int", nullable: true),
                    ClaimChargeId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimNumber = table.Column<int>(type: "int", nullable: true),
                    RemittanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EntryDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ModifiedDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ClaimPayments", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Insurances",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AddressStreetLine1 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AddressStreetLine2 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Active = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Insurances", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Locations",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddressStreetLine1 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressStreetLine2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Locations", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_LogDetails",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PipeLineName = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    ActivityName = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tbl_LogD__5E5486485FE466E3", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PatientInsuranceCards",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    InsuranceId = table.Column<int>(type: "int", nullable: true),
                    CardHolderId = table.Column<int>(type: "int", nullable: true),
                    InsuranceCardOrder = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    GroupId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EffectiveStartDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    EffectiveEndDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    CoPay = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CoInsurance = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ActiveDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    InactiveDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    InactivePosition = table.Column<int>(type: "int", nullable: true),
                    CardHolderRelationship = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    PlanType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PatientInsuranceCards", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Patients",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    ResponsiblePartyId = table.Column<int>(type: "int", nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MiddleName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Gender = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    AddressStreetLine1 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    HomePhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    SocialSecurityNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PrimaryInsuranceId = table.Column<int>(type: "int", nullable: true),
                    RenderingProviderId = table.Column<int>(type: "int", nullable: true),
                    SecondaryInsuranceId = table.Column<int>(type: "int", nullable: true),
                    TertiaryInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CreatedOn = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    LastModifiedOn = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    ExternalId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Patients", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_PlaceOfServices",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceOfServiceId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    PlaceOfServiceCode = table.Column<int>(type: "int", nullable: true),
                    OfficePhone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    FaxPhone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    NPI = table.Column<long>(type: "bigint", nullable: true),
                    ExternalId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_PlaceOfServices", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ProviderLocations",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ProviderLocations", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Providers",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    State = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    OfficePhone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    LicenseNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    SocialSecurityNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TaxId = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    SpecialtyId = table.Column<int>(type: "int", nullable: true),
                    FirstName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsPhysiciansAssistant = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    Npi = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    FaxNumber = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    ExternalId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsActive = table.Column<string>(type: "char(1)", unicode: false, fixedLength: true, maxLength: 1, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Providers", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Remittances",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RemittanceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceId = table.Column<int>(type: "int", nullable: true),
                    UndistributedAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemittanceFormType = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    RemittanceSource = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    CheckDate = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Remittances", x => x.StgId);
                });

            migrationBuilder.CreateTable(
                name: "tbl_ResponsibleParties",
                columns: table => new
                {
                    StgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResponsiblePartiesId = table.Column<int>(type: "int", nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AddressStreetLine1 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    AddressStreetLine2 = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    City = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    State = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    MobilePhone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TenantClientString = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsProcessedSuccessfully = table.Column<bool>(type: "bit", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StgLastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StgLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_ResponsibleParties", x => x.StgId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_AdjustmentCodes");

            migrationBuilder.DropTable(
                name: "tbl_CardHolders");

            migrationBuilder.DropTable(
                name: "tbl_Charges");

            migrationBuilder.DropTable(
                name: "tbl_ClaimAdjustments");

            migrationBuilder.DropTable(
                name: "tbl_ClaimPayments");

            migrationBuilder.DropTable(
                name: "tbl_Insurances");

            migrationBuilder.DropTable(
                name: "tbl_Locations");

            migrationBuilder.DropTable(
                name: "tbl_LogDetails");

            migrationBuilder.DropTable(
                name: "tbl_PatientInsuranceCards");

            migrationBuilder.DropTable(
                name: "tbl_Patients");

            migrationBuilder.DropTable(
                name: "tbl_PlaceOfServices");

            migrationBuilder.DropTable(
                name: "tbl_ProviderLocations");

            migrationBuilder.DropTable(
                name: "tbl_Providers");

            migrationBuilder.DropTable(
                name: "tbl_Remittances");

            migrationBuilder.DropTable(
                name: "tbl_ResponsibleParties");
        }
    }
}
