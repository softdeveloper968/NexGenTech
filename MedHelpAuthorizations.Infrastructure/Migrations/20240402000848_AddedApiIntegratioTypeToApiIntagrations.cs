using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedApiIntegratioTypeToApiIntagrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApiIntegrationTypeId",
                schema: "dbo",
                table: "ApiIntegrations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApiIntegrationTypes",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiIntegrationTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiIntegrations_ApiIntegrationTypeId",
                schema: "dbo",
                table: "ApiIntegrations",
                column: "ApiIntegrationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApiIntegrations_ApiIntegrationTypes_ApiIntegrationTypeId",
                schema: "dbo",
                table: "ApiIntegrations",
                column: "ApiIntegrationTypeId",
                principalSchema: "dbo",
                principalTable: "ApiIntegrationTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApiIntegrations_ApiIntegrationTypes_ApiIntegrationTypeId",
                schema: "dbo",
                table: "ApiIntegrations");

            migrationBuilder.DropTable(
                name: "ApiIntegrationTypes",
                schema: "dbo");

            migrationBuilder.DropIndex(
                name: "IX_ApiIntegrations_ApiIntegrationTypeId",
                schema: "dbo",
                table: "ApiIntegrations");

            migrationBuilder.DropColumn(
                name: "ApiIntegrationTypeId",
                schema: "dbo",
                table: "ApiIntegrations");
        }
    }
}
