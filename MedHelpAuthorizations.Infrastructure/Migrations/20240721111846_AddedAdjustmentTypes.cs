using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdjustmentTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAdjustmentCodes_AdjustmentType_AdjustmentTypeId",
                schema: "dbo",
                table: "ClientAdjustmentCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdjustmentType",
                table: "AdjustmentType");

            migrationBuilder.RenameTable(
                name: "AuditTrails",
                newName: "AuditTrails",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "AdjustmentType",
                newName: "AdjustmentTypes",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdjustmentTypes",
                schema: "dbo",
                table: "AdjustmentTypes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentTypes_CreatedOn",
                schema: "dbo",
                table: "AdjustmentTypes",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_AdjustmentTypes_LastModifiedOn",
                schema: "dbo",
                table: "AdjustmentTypes",
                column: "LastModifiedOn");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAdjustmentCodes_AdjustmentTypes_AdjustmentTypeId",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "AdjustmentTypeId",
                principalSchema: "dbo",
                principalTable: "AdjustmentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAdjustmentCodes_AdjustmentTypes_AdjustmentTypeId",
                schema: "dbo",
                table: "ClientAdjustmentCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AdjustmentTypes",
                schema: "dbo",
                table: "AdjustmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_AdjustmentTypes_CreatedOn",
                schema: "dbo",
                table: "AdjustmentTypes");

            migrationBuilder.DropIndex(
                name: "IX_AdjustmentTypes_LastModifiedOn",
                schema: "dbo",
                table: "AdjustmentTypes");

            migrationBuilder.RenameTable(
                name: "AuditTrails",
                schema: "dbo",
                newName: "AuditTrails");

            migrationBuilder.RenameTable(
                name: "AdjustmentTypes",
                schema: "dbo",
                newName: "AdjustmentType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AdjustmentType",
                table: "AdjustmentType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAdjustmentCodes_AdjustmentType_AdjustmentTypeId",
                schema: "dbo",
                table: "ClientAdjustmentCodes",
                column: "AdjustmentTypeId",
                principalTable: "AdjustmentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
