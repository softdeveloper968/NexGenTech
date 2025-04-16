using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequiredAuthTyeIdBatchHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchHistories_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories");

            migrationBuilder.AlterColumn<int>(
                name: "AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchHistories_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AuthTypeId",
                principalSchema: "dbo",
                principalTable: "AuthTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchHistories_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories");

            migrationBuilder.AlterColumn<int>(
                name: "AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchHistories_AuthTypes_AuthTypeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "AuthTypeId",
                principalSchema: "dbo",
                principalTable: "AuthTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
