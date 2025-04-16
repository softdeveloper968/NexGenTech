using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedClientEndOfMonthTotal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientEndOfMonthTotals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    ARTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ARTotalAbove90Days = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ARTotalAbove180Days = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ARTotalVisits = table.Column<int>(type: "int", nullable: false),
                    ARTotalVisitsAbove90Days = table.Column<int>(type: "int", nullable: false),
                    ARTotalVisitsAbove180Days = table.Column<int>(type: "int", nullable: false),
                    MonthlyDaysInAR = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEndOfMonthTotals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEndOfMonthTotals_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientEndOfMonthTotals_ClientId",
                table: "ClientEndOfMonthTotals",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientEndOfMonthTotals");
        }
    }
}
