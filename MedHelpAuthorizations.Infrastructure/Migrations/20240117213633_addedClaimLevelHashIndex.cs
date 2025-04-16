using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedClaimLevelHashIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClaimLevelMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClaimLevelMd5Hash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_ClaimLevelMd5Hash",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");
        }
    }
}
