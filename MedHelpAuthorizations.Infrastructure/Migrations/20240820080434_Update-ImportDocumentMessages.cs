using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImportDocumentMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimStatusBatchClaimId",
                table: "ImportDocumentMessages",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImportDocumentMessages_ClaimStatusBatchClaimId",
                table: "ImportDocumentMessages",
                column: "ClaimStatusBatchClaimId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDocumentMessages_ClaimStatusBatchClaims_ClaimStatusBatchClaimId",
                table: "ImportDocumentMessages",
                column: "ClaimStatusBatchClaimId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimStatusBatchClaims",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDocumentMessages_ClaimStatusBatchClaims_ClaimStatusBatchClaimId",
                table: "ImportDocumentMessages");

            migrationBuilder.DropIndex(
                name: "IX_ImportDocumentMessages_ClaimStatusBatchClaimId",
                table: "ImportDocumentMessages");

            migrationBuilder.DropColumn(
                name: "ClaimStatusBatchClaimId",
                table: "ImportDocumentMessages");
        }
    }
}
