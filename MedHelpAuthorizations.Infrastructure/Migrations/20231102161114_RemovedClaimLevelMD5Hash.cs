using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedClaimLevelMD5Hash : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimLevelMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClaimLevelMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true,
                computedColumnSql: "CONVERT([varchar](34), HASHBYTES('MD5', CONCAT(TRIM(CONVERT(varchar(12), PatientId)), '|', UPPER(ClaimNumber), '|', UPPER(ClientId), '|', UPPER(ClientInsuranceId), '|', CONVERT(varchar(8),DateOfServiceFrom, 112), '|')), 1)",
                stored: true);
        }
    }
}
