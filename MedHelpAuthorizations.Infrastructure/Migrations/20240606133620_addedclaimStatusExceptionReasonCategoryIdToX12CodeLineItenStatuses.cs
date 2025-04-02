using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedclaimStatusExceptionReasonCategoryIdToX12CodeLineItenStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_X12ClaimCodeLineItemStatuses_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_X12ClaimCodeLineItemStatuses_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses",
                column: "ClaimStatusExceptionReasonCategoryId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimStatusExceptionReasonCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_X12ClaimCodeLineItemStatuses_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_X12ClaimCodeLineItemStatuses_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");

            migrationBuilder.DropColumn(
                name: "ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCodeLineItemStatuses");
        }
    }
}
