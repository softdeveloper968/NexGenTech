using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSystemDefaultReportFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsSystemDefault",
                schema: "dbo",
                table: "ClientUserReportFilter",
                newName: "IsDeleted");

            migrationBuilder.AddColumn<int>(
                name: "SystemDefaultReportFilterId",
                schema: "dbo",
                table: "ClientUserReportFilter",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientUserReportFilter_SystemDefaultReportFilterId",
                schema: "dbo",
                table: "ClientUserReportFilter",
                column: "SystemDefaultReportFilterId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientUserReportFilter_SystemDefaultReportFilters_SystemDefaultReportFilterId",
                schema: "dbo",
                table: "ClientUserReportFilter",
                column: "SystemDefaultReportFilterId",
                principalSchema: "dbo",
                principalTable: "SystemDefaultReportFilters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientUserReportFilter_SystemDefaultReportFilters_SystemDefaultReportFilterId",
                schema: "dbo",
                table: "ClientUserReportFilter");

            migrationBuilder.DropIndex(
                name: "IX_ClientUserReportFilter_SystemDefaultReportFilterId",
                schema: "dbo",
                table: "ClientUserReportFilter");

            migrationBuilder.DropColumn(
                name: "SystemDefaultReportFilterId",
                schema: "dbo",
                table: "ClientUserReportFilter");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                schema: "dbo",
                table: "ClientUserReportFilter",
                newName: "IsSystemDefault");
        }
    }
}
