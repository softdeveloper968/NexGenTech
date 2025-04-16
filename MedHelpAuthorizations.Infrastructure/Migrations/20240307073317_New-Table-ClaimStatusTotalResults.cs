using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewTableClaimStatusTotalResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClaimStatusTotalResults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientInsuranceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimLineItemStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimLineItemStatusId = table.Column<int>(type: "int", nullable: true),
                    ClaimStatusExceptionReasonCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientCptCodeId = table.Column<int>(type: "int", nullable: false),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ChargedSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmountSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AllowedAmountSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NonAllowedAmountSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    ClientProviderId = table.Column<int>(type: "int", nullable: true),
                    ProviderName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationNpi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WriteOffAmountSum = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BatchProcessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClaimStatusTotalResults", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClaimStatusTotalResults");
        }
    }
}
