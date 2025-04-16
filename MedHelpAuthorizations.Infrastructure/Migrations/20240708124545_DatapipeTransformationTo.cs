using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DatapipeTransformationTo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ResponsibleParties_Clients_ClientId",
            //    schema: "dbo",
            //    table: "ResponsibleParties");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ClientId",
            //    schema: "dbo",
            //    table: "ResponsibleParties",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ResponsibleParties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "ResponsibleParties",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ResponsibleParties",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "Providers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "Providers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "Providers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "Patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "Patients",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "InsuranceCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "InsuranceCards",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "InsuranceCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientProviderLocations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientProviderLocations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientProviderLocations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientPlacesOfService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientPlacesOfService",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientLocations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientLocations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientLocations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientInsurances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientInsurances",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientInsurances",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "Cardholders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DfExternalId",
                schema: "dbo",
                table: "Cardholders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "Cardholders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AdjustmentType",
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
                    table.PrimaryKey("PK_AdjustmentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientRemittances",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    UndistributedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: true),
                    RemittanceFormType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RemittanceSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DfExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DfCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DfLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRemittances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRemittances_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientRemittances_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ClientRemittances_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientRemittances_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PatientLedgerCharges",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DfExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    ResponsiblePartyId = table.Column<int>(type: "int", nullable: false),
                    ClientCptCodeId = table.Column<int>(type: "int", nullable: false),
                    ClientPlaceOfServiceId = table.Column<int>(type: "int", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceCard1Id = table.Column<int>(type: "int", nullable: true),
                    InsuranceCard2Id = table.Column<int>(type: "int", nullable: true),
                    InsuranceCard3Id = table.Column<int>(type: "int", nullable: true),
                    BilledToInsuranceCardId = table.Column<int>(type: "int", nullable: true),
                    PatientFirstBillDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientLastBillDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BilledToClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    RenderingProviderId = table.Column<int>(type: "int", nullable: true),
                    Modifier1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Modifier4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IcdCode4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfServiceFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateOfServiceTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    DfCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DfLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLedgerCharges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_ClientCptCodes_ClientCptCodeId",
                        column: x => x.ClientCptCodeId,
                        principalSchema: "dbo",
                        principalTable: "ClientCptCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_ClientPlacesOfService_ClientPlaceOfServiceId",
                        column: x => x.ClientPlaceOfServiceId,
                        principalSchema: "dbo",
                        principalTable: "ClientPlacesOfService",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_InsuranceCards_BilledToInsuranceCardId",
                        column: x => x.BilledToInsuranceCardId,
                        principalSchema: "dbo",
                        principalTable: "InsuranceCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_InsuranceCards_InsuranceCard1Id",
                        column: x => x.InsuranceCard1Id,
                        principalSchema: "dbo",
                        principalTable: "InsuranceCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_InsuranceCards_InsuranceCard2Id",
                        column: x => x.InsuranceCard2Id,
                        principalSchema: "dbo",
                        principalTable: "InsuranceCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_InsuranceCards_InsuranceCard3Id",
                        column: x => x.InsuranceCard3Id,
                        principalSchema: "dbo",
                        principalTable: "InsuranceCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_Patients_PatientId",
                        column: x => x.PatientId,
                        principalSchema: "dbo",
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_Providers_RenderingProviderId",
                        column: x => x.RenderingProviderId,
                        principalSchema: "dbo",
                        principalTable: "Providers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerCharges_ResponsibleParties_ResponsiblePartyId",
                        column: x => x.ResponsiblePartyId,
                        principalSchema: "dbo",
                        principalTable: "ResponsibleParties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientAdjustmentCodes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustmentTypeId = table.Column<int>(type: "int", nullable: false),
                    DfExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DfCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DfLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientAdjustmentCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientAdjustmentCodes_AdjustmentType_AdjustmentTypeId",
                        column: x => x.AdjustmentTypeId,
                        principalTable: "AdjustmentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientAdjustmentCodes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientLedgerPayments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DfExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientLedgerChargeId = table.Column<int>(type: "int", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientRemittanceId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DfCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DfLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLedgerPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLedgerPayments_ClientRemittances_ClientRemittanceId",
                        column: x => x.ClientRemittanceId,
                        principalSchema: "dbo",
                        principalTable: "ClientRemittances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerPayments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLedgerPayments_PatientLedgerCharges_PatientLedgerChargeId",
                        column: x => x.PatientLedgerChargeId,
                        principalSchema: "dbo",
                        principalTable: "PatientLedgerCharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientLedgerAdjustments",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientAdjustmentCodeId = table.Column<int>(type: "int", nullable: false),
                    DfExternalId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PatientLedgerChargeId = table.Column<int>(type: "int", nullable: false),
                    ClaimNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientRemittanceId = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DfCreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DfLastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientLedgerAdjustments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientLedgerAdjustments_ClientAdjustmentCodes_ClientAdjustmentCodeId",
                        column: x => x.ClientAdjustmentCodeId,
                        principalSchema: "dbo",
                        principalTable: "ClientAdjustmentCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientLedgerAdjustments_ClientRemittances_ClientRemittanceId",
                        column: x => x.ClientRemittanceId,
                        principalSchema: "dbo",
                        principalTable: "ClientRemittances",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PatientLedgerAdjustments_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientLedgerAdjustments_PatientLedgerCharges_PatientLedgerChargeId",
                        column: x => x.PatientLedgerChargeId,
                        principalSchema: "dbo",
                        principalTable: "PatientLedgerCharges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_CreatedOn",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_DfCreatedOn",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_DfExternalId",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_DfLastModifiedOn",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "DfLastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ResponsibleParties_LastModifiedOn",
                schema: "dbo",
                table: "ResponsibleParties",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_CreatedOn",
                schema: "dbo",
                table: "Providers",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_DfCreatedOn",
                schema: "dbo",
                table: "Providers",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_DfExternalId",
                schema: "dbo",
                table: "Providers",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_DfLastModifiedOn",
                schema: "dbo",
                table: "Providers",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Providers_LastModifiedOn",
            //    schema: "dbo",
            //    table: "Providers",
            //    column: "LastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Patients_CreatedOn",
            //    schema: "dbo",
            //    table: "Patients",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DfCreatedOn",
                schema: "dbo",
                table: "Patients",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DfExternalId",
                schema: "dbo",
                table: "Patients",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_DfLastModifiedOn",
                schema: "dbo",
                table: "Patients",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Patients_LastModifiedOn",
            //    schema: "dbo",
            //    table: "Patients",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_DfCreatedOn",
                schema: "dbo",
                table: "InsuranceCards",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_DfExternalId",
                schema: "dbo",
                table: "InsuranceCards",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_DfLastModifiedOn",
                schema: "dbo",
                table: "InsuranceCards",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientProviderLocations_CreatedOn",
            //    schema: "dbo",
            //    table: "ClientProviderLocations",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviderLocations_DfCreatedOn",
                schema: "dbo",
                table: "ClientProviderLocations",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviderLocations_DfExternalId",
                schema: "dbo",
                table: "ClientProviderLocations",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProviderLocations_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientProviderLocations",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientProviderLocations_LastModifiedOn",
            //    schema: "dbo",
            //    table: "ClientProviderLocations",
            //    column: "LastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientPlacesOfService_CreatedOn",
            //    schema: "dbo",
            //    table: "ClientPlacesOfService",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlacesOfService_DfCreatedOn",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlacesOfService_DfExternalId",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlacesOfService_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientPlacesOfService",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientPlacesOfService_LastModifiedOn",
            //    schema: "dbo",
            //    table: "ClientPlacesOfService",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_DfCreatedOn",
                schema: "dbo",
                table: "ClientLocations",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_DfExternalId",
                schema: "dbo",
                table: "ClientLocations",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientLocations",
                column: "DfLastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_DfCreatedOn",
                schema: "dbo",
                table: "ClientInsurances",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_DfExternalId",
                schema: "dbo",
                table: "ClientInsurances",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientInsurances",
                column: "DfLastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_DfCreatedOn",
                schema: "dbo",
                table: "Cardholders",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_DfExternalId",
                schema: "dbo",
                table: "Cardholders",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_DfLastModifiedOn",
                schema: "dbo",
                table: "Cardholders",
                column: "DfLastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAdjustmentCodes_AdjustmentTypeId",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "AdjustmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAdjustmentCodes_ClientId",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "ClientId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientAdjustmentCodes_CreatedOn",
            //    schema: "dbo",
            //    table: "ClientAdjustmentCodes",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAdjustmentCodes_DfCreatedOn",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAdjustmentCodes_DfExternalId",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientAdjustmentCodes_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientAdjustmentCodes_LastModifiedOn",
            //    schema: "dbo",
            //    table: "ClientAdjustmentCodes",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_ClientId",
                schema: "dbo",
                table: "ClientRemittances",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_ClientInsuranceId",
                schema: "dbo",
                table: "ClientRemittances",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_ClientLocationId",
                schema: "dbo",
                table: "ClientRemittances",
                column: "ClientLocationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientRemittances_CreatedOn",
            //    schema: "dbo",
            //    table: "ClientRemittances",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_DfCreatedOn",
                schema: "dbo",
                table: "ClientRemittances",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_DfExternalId",
                schema: "dbo",
                table: "ClientRemittances",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientRemittances",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClientRemittances_LastModifiedOn",
            //    schema: "dbo",
            //    table: "ClientRemittances",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRemittances_PatientId",
                schema: "dbo",
                table: "ClientRemittances",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_ClientAdjustmentCodeId",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "ClientAdjustmentCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_ClientId",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_ClientRemittanceId",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "ClientRemittanceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientLedgerAdjustments_CreatedOn",
            //    schema: "dbo",
            //    table: "PatientLedgerAdjustments",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_DfCreatedOn",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_DfExternalId",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_DfLastModifiedOn",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientLedgerAdjustments_LastModifiedOn",
            //    schema: "dbo",
            //    table: "PatientLedgerAdjustments",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerAdjustments_PatientLedgerChargeId",
                schema: "dbo",
                table: "PatientLedgerAdjustments",
                column: "PatientLedgerChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_BilledToInsuranceCardId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "BilledToInsuranceCardId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ClientCptCodeId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClientCptCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ClientId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ClientLocationId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClientLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ClientPlaceOfServiceId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClientPlaceOfServiceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientLedgerCharges_CreatedOn",
            //    schema: "dbo",
            //    table: "PatientLedgerCharges",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_DfCreatedOn",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_DfExternalId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_DfLastModifiedOn",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "DfLastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_InsuranceCard1Id",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "InsuranceCard1Id");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_InsuranceCard2Id",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "InsuranceCard2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_InsuranceCard3Id",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "InsuranceCard3Id");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientLedgerCharges_LastModifiedOn",
            //    schema: "dbo",
            //    table: "PatientLedgerCharges",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_PatientId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_RenderingProviderId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "RenderingProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ResponsiblePartyId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ResponsiblePartyId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerPayments_ClientId",
                schema: "dbo",
                table: "PatientLedgerPayments",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerPayments_ClientRemittanceId",
                schema: "dbo",
                table: "PatientLedgerPayments",
                column: "ClientRemittanceId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientLedgerPayments_CreatedOn",
            //    schema: "dbo",
            //    table: "PatientLedgerPayments",
            //    column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerPayments_DfCreatedOn",
                schema: "dbo",
                table: "PatientLedgerPayments",
                column: "DfCreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerPayments_DfExternalId",
                schema: "dbo",
                table: "PatientLedgerPayments",
                column: "DfExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerPayments_DfLastModifiedOn",
                schema: "dbo",
                table: "PatientLedgerPayments",
                column: "DfLastModifiedOn");

            //migrationBuilder.CreateIndex(
            //    name: "IX_PatientLedgerPayments_LastModifiedOn",
            //    schema: "dbo",
            //    table: "PatientLedgerPayments",
            //    column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerPayments_PatientLedgerChargeId",
                schema: "dbo",
                table: "PatientLedgerPayments",
                column: "PatientLedgerChargeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ResponsibleParties_Clients_ClientId",
            //    schema: "dbo",
            //    table: "ResponsibleParties",
            //    column: "ClientId",
            //    principalSchema: "dbo",
            //    principalTable: "Clients",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_ResponsibleParties_Clients_ClientId",
            //    schema: "dbo",
            //    table: "ResponsibleParties");

            migrationBuilder.DropTable(
                name: "PatientLedgerAdjustments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PatientLedgerPayments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientAdjustmentCodes",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ClientRemittances",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "PatientLedgerCharges",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AdjustmentType");

            migrationBuilder.DropIndex(
                name: "IX_ResponsibleParties_CreatedOn",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropIndex(
                name: "IX_ResponsibleParties_DfCreatedOn",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropIndex(
                name: "IX_ResponsibleParties_DfExternalId",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropIndex(
                name: "IX_ResponsibleParties_DfLastModifiedOn",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropIndex(
                name: "IX_ResponsibleParties_LastModifiedOn",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropIndex(
                name: "IX_Providers_CreatedOn",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_DfCreatedOn",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_DfExternalId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropIndex(
                name: "IX_Providers_DfLastModifiedOn",
                schema: "dbo",
                table: "Providers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Providers_LastModifiedOn",
            //    schema: "dbo",
            //    table: "Providers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Patients_CreatedOn",
            //    schema: "dbo",
            //    table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DfCreatedOn",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DfExternalId",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_DfLastModifiedOn",
                schema: "dbo",
                table: "Patients");

            //migrationBuilder.DropIndex(
            //    name: "IX_Patients_LastModifiedOn",
            //    schema: "dbo",
            //    table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCards_DfCreatedOn",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCards_DfExternalId",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCards_DfLastModifiedOn",
                schema: "dbo",
                table: "InsuranceCards");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClientProviderLocations_CreatedOn",
            //    schema: "dbo",
            //    table: "ClientProviderLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientProviderLocations_DfCreatedOn",
                schema: "dbo",
                table: "ClientProviderLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientProviderLocations_DfExternalId",
                schema: "dbo",
                table: "ClientProviderLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientProviderLocations_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientProviderLocations");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClientProviderLocations_LastModifiedOn",
            //    schema: "dbo",
            //    table: "ClientProviderLocations");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClientPlacesOfService_CreatedOn",
            //    schema: "dbo",
            //    table: "ClientPlacesOfService");

            migrationBuilder.DropIndex(
                name: "IX_ClientPlacesOfService_DfCreatedOn",
                schema: "dbo",
                table: "ClientPlacesOfService");

            migrationBuilder.DropIndex(
                name: "IX_ClientPlacesOfService_DfExternalId",
                schema: "dbo",
                table: "ClientPlacesOfService");

            migrationBuilder.DropIndex(
                name: "IX_ClientPlacesOfService_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientPlacesOfService");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClientPlacesOfService_LastModifiedOn",
            //    schema: "dbo",
            //    table: "ClientPlacesOfService");

            migrationBuilder.DropIndex(
                name: "IX_ClientLocations_DfCreatedOn",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientLocations_DfExternalId",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientLocations_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsurances_DfCreatedOn",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsurances_DfExternalId",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsurances_DfLastModifiedOn",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_Cardholders_DfCreatedOn",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropIndex(
                name: "IX_Cardholders_DfExternalId",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropIndex(
                name: "IX_Cardholders_DfLastModifiedOn",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ResponsibleParties");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientProviderLocations");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientProviderLocations");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientProviderLocations");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientPlacesOfService");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientPlacesOfService");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientPlacesOfService");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropColumn(
                name: "DfCreatedOn",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropColumn(
                name: "DfExternalId",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropColumn(
                name: "DfLastModifiedOn",
                schema: "dbo",
                table: "Cardholders");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ClientId",
            //    schema: "dbo",
            //    table: "ResponsibleParties",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ResponsibleParties_Clients_ClientId",
            //    schema: "dbo",
            //    table: "ResponsibleParties",
            //    column: "ClientId",
            //    principalSchema: "dbo",
            //    principalTable: "Clients",
            //    principalColumn: "Id");
        }
    }
}
