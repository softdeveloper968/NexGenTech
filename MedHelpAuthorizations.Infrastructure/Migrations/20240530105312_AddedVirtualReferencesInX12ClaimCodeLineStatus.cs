using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedVirtualReferencesInX12ClaimCodeLineStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_X12ClaimCodeLineItemStatuses_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                column: "ClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_X12ClaimCodeLineItemStatuses_X12ClaimCodeTypeId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                column: "X12ClaimCodeTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_X12ClaimCodeLineItemStatuses_ClaimLineItemStatuses_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                column: "ClaimLineItemStatusId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimLineItemStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_X12ClaimCodeLineItemStatuses_X12ClaimCodeTypes_X12ClaimCodeTypeId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                column: "X12ClaimCodeTypeId",
                principalSchema: "IntegratedServices",
                principalTable: "X12ClaimCodeTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_X12ClaimCodeLineItemStatuses_ClaimLineItemStatuses_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_X12ClaimCodeLineItemStatuses_X12ClaimCodeTypes_X12ClaimCodeTypeId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_X12ClaimCodeLineItemStatuses_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_X12ClaimCodeLineItemStatuses_X12ClaimCodeTypeId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");
        }
    }
}
