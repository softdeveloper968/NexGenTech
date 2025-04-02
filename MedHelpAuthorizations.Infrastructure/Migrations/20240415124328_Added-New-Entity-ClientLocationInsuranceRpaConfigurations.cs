using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewEntityClientLocationInsuranceRpaConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientLocationInsuranceRpaConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientInsuranceRpaConfigurationId = table.Column<int>(type: "int", nullable: false),
                    ClienLocationId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLocationInsuranceRpaConfigurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceRpaConfigurations_ClientInsuranceRpaConfigurations_ClienLocationId",
                        column: x => x.ClienLocationId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClientInsuranceRpaConfigurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientLocationInsuranceRpaConfigurations_ClientLocations_ClienLocationId",
                        column: x => x.ClienLocationId,
                        principalSchema: "dbo",
                        principalTable: "ClientLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocationInsuranceRpaConfigurations_ClienLocationId",
                table: "ClientLocationInsuranceRpaConfigurations",
                column: "ClienLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientLocationInsuranceRpaConfigurations");
        }
    }
}
