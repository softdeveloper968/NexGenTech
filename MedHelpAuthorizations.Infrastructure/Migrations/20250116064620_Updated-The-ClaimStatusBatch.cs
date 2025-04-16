using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTheClaimStatusBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatches_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatches_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatches_Clients_ClientId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "ClientId",
                principalSchema: "dbo",
                principalTable: "Clients",
                principalColumn: "Id");
        }
    }
}
