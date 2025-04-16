using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenamedClientLocationInsuranceIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientLocationInsuranceIdentifiers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: false),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    Identifier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocationInsuranceIdentifiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceIdentifiers_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceIdentifiers_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceIdentifiers_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceIdentifiers_ClientId",
                schema: "dbo",
                table: "ClientLocationInsuranceIdentifiers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceIdentifiers_ClientInsuranceId",
                schema: "dbo",
                table: "ClientLocationInsuranceIdentifiers",
                column: "ClientInsuranceId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceIdentifiers_ClientLocationId_ClientInsuranceId_ClientId",
                schema: "dbo",
                table: "ClientLocationInsuranceIdentifiers",
                columns: new[] { "ClientLocationId", "ClientInsuranceId", "ClientId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientLocationInsuranceIdentifiers",
                schema: "dbo");
        }
    }
}
