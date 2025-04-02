using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addednewcolumnsinX12ClaimStatusCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimStatusCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_X12ClaimStatusCodes_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimStatusCodes",
                column: "ClaimStatusExceptionReasonCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_X12ClaimStatusCodes_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimStatusCodes",
                column: "ClaimStatusExceptionReasonCategoryId",
                principalSchema: "IntegratedServices",
                principalTable: "ClaimStatusExceptionReasonCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_X12ClaimStatusCodes_ClaimStatusExceptionReasonCategories_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimStatusCodes");

            migrationBuilder.DropIndex(
                name: "IX_X12ClaimStatusCodes_ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimStatusCodes");

            migrationBuilder.DropColumn(
                name: "ClaimStatusExceptionReasonCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimStatusCodes");
        }
    }
}
