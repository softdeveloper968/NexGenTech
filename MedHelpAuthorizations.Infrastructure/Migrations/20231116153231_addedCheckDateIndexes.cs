using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCheckDateIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_CheckDate",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "CheckDate");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_DateOfServiceFrom",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "DateOfServiceFrom");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_DateOfServiceTo",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "DateOfServiceTo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTransactions_CheckDate",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_DateOfServiceFrom",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_DateOfServiceTo",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");
        }
    }
}
