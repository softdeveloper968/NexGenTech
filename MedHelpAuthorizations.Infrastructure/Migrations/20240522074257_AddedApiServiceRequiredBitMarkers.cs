using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedApiServiceRequiredBitMarkers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GroupClaimLines",
                schema: "dbo",
                table: "ApiIntegrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireDateOfBirth",
                schema: "dbo",
                table: "ApiIntegrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePayerIdentifier",
                schema: "dbo",
                table: "ApiIntegrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequirePolicyNumber",
                schema: "dbo",
                table: "ApiIntegrations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RequireTaxId",
                schema: "dbo",
                table: "ApiIntegrations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupClaimLines",
                schema: "dbo",
                table: "ApiIntegrations");

            migrationBuilder.DropColumn(
                name: "RequireDateOfBirth",
                schema: "dbo",
                table: "ApiIntegrations");

            migrationBuilder.DropColumn(
                name: "RequirePayerIdentifier",
                schema: "dbo",
                table: "ApiIntegrations");

            migrationBuilder.DropColumn(
                name: "RequirePolicyNumber",
                schema: "dbo",
                table: "ApiIntegrations");

            migrationBuilder.DropColumn(
                name: "RequireTaxId",
                schema: "dbo",
                table: "ApiIntegrations");
        }
    }
}
