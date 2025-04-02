using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedUnmappedFeeScheduleCptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcedureCode",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");

            migrationBuilder.AddColumn<int>(
                name: "ClientCptCodeId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UnmappedFeeScheduleCpts_ClientCptCodeId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                column: "ClientCptCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_UnmappedFeeScheduleCpts_ClientCptCodes_ClientCptCodeId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                column: "ClientCptCodeId",
                principalSchema: "dbo",
                principalTable: "ClientCptCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UnmappedFeeScheduleCpts_ClientCptCodes_ClientCptCodeId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");

            migrationBuilder.DropIndex(
                name: "IX_UnmappedFeeScheduleCpts_ClientCptCodeId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");

            migrationBuilder.DropColumn(
                name: "ClientCptCodeId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");

            migrationBuilder.AddColumn<string>(
                name: "ProcedureCode",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
