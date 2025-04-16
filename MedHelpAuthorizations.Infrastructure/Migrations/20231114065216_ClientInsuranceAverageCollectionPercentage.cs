using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ClientInsuranceAverageCollectionPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientInsuranceAverageCollectionPercentages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    Quarter = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CollectionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientInsuranceAverageCollectionPercentages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientInsuranceAverageCollectionPercentages_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceAverageCollectionPercentages_ClientInsuranceId",
                table: "ClientInsuranceAverageCollectionPercentages",
                column: "ClientInsuranceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientInsuranceAverageCollectionPercentages");

        }
    }
}
