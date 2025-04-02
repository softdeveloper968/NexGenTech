using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedMonthlySummaryEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyARData",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Receivables = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentageOfAR = table.Column<decimal>(type: "decimal(18,2)", nullable: false, computedColumnSql: "CASE WHEN TotalReceivables = 0 THEN 0 ELSE (Receivables * 100) / TotalReceivables END"),
                    DenialGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    DenialPercentageGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CptCodeId = table.Column<int>(type: "int", nullable: false),
                    TotalReceivables = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyARData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyARData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyCashCollectionData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Payment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollectionGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CptCodeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyCashCollectionData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyCashCollectionData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyDenialData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Charges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Denials = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PercentageOfCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DenialPercentageGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CptCodeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyDenialData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyDenialData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyReceivablesData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Receivables = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DaysInAR = table.Column<int>(type: "int", nullable: false),
                    PercentageOfCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DenialPercentageGoal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: true),
                    CptCodeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Change = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyReceivablesData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyReceivablesData_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyARData_ClientId",
                schema: "dbo",
                table: "MonthlyARData",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyCashCollectionData_ClientId",
                table: "MonthlyCashCollectionData",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyDenialData_ClientId",
                table: "MonthlyDenialData",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyReceivablesData_ClientId",
                table: "MonthlyReceivablesData",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthlyARData",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "MonthlyCashCollectionData");

            migrationBuilder.DropTable(
                name: "MonthlyDenialData");

            migrationBuilder.DropTable(
                name: "MonthlyReceivablesData");

            
        }
    }
}
