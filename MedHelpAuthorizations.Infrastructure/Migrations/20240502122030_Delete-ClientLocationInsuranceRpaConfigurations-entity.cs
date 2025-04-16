using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteClientLocationInsuranceRpaConfigurationsentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientLocationInsuranceRpaConfigurations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientLocationInsuranceRpaConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientInsuranceRpaConfigurationId = table.Column<int>(type: "int", nullable: false),
                    ClientLocationId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocationInsuranceRpaConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurations_ClientInsuranceRpaConfigurationId",
                        column: x => x.ClientInsuranceRpaConfigurationId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClientInsuranceRpaConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClientLocationId",
                        column: x => x.ClientLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientInsuranceRpaConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClientLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClientLocationId");
        }
    }
}
