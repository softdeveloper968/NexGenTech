using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTheEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientCptCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientCptCodes_ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "ClientCptCodeId",
                principalSchema: "dbo",
                principalTable: "ClientCptCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientCptCodes_ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropColumn(
                name: "ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");
        }
    }
}
