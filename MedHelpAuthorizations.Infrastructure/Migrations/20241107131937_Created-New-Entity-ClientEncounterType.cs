using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedNewEntityClientEncounterType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientEncounterTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldClientAuthTypeId = table.Column<int>(type: "int", nullable: true),
                    OldAuthTypeId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientEncounterTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientEncounterTypes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientEncounterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientEncounterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientEncounterTypes_ClientId",
                table: "ClientEncounterTypes",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_ClientEncounterTypes_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientEncounterTypeId",
                principalTable: "ClientEncounterTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientEncounterTypes_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations",
                column: "ClientEncounterTypeId",
                principalTable: "ClientEncounterTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatches_ClientEncounterTypes_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientInsuranceRpaConfigurations_ClientEncounterTypes_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropTable(
                name: "ClientEncounterTypes");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsuranceRpaConfigurations_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatches_ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.DropColumn(
                name: "ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClientInsuranceRpaConfigurations");

            migrationBuilder.DropColumn(
                name: "ClientEncounterTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");
        }
    }
}
