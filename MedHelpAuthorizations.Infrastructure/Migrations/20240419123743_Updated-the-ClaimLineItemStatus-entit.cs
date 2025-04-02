using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedtheClaimLineItemStatusentit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClaimStatusTypeId",
                schema: "IntegratedServices",
                table: "ClaimLineItemStatuses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimLineItemStatuses_ClaimStatusTypeId",
                schema: "IntegratedServices",
                table: "ClaimLineItemStatuses",
                column: "ClaimStatusTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimLineItemStatuses_ClaimStatusTypes_ClaimStatusTypeId",
                schema: "IntegratedServices",
                table: "ClaimLineItemStatuses",
                column: "ClaimStatusTypeId",
                principalTable: "ClaimStatusTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimLineItemStatuses_ClaimStatusTypes_ClaimStatusTypeId",
                schema: "IntegratedServices",
                table: "ClaimLineItemStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ClaimLineItemStatuses_ClaimStatusTypeId",
                schema: "IntegratedServices",
                table: "ClaimLineItemStatuses");

            migrationBuilder.DropColumn(
                name: "ClaimStatusTypeId",
                schema: "IntegratedServices",
                table: "ClaimLineItemStatuses");
        }
    }
}
