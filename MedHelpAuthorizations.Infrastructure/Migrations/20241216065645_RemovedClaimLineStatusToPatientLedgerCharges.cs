using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedClaimLineStatusToPatientLedgerCharges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientLedgerCharges_ClaimLineItemStatuses_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges");

            migrationBuilder.DropIndex(
                name: "IX_PatientLedgerCharges_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges");

            migrationBuilder.DropColumn(
                name: "ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientLedgerCharges_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClaimLineItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientLedgerCharges_ClaimLineItemStatuses_ClaimLineItemStatusId",
                schema: "dbo",
                table: "PatientLedgerCharges",
                column: "ClaimLineItemStatusId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimLineItemStatuses",
                principalColumn: "Id");
        }
    }
}
