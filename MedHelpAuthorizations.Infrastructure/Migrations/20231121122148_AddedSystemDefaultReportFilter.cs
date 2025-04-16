using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedSystemDefaultReportFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.AddColumn<bool>(
                name: "IsSystemDefault",
                schema: "dbo",
                table: "ClientUserReportFilter",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "SystemDefaultReportFilters",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    FilterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilterConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemDefaultReportFilters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemDefaultReportFilterEmployeeRoles",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemDefaultReportFilterId = table.Column<int>(type: "int", nullable: false),
                    EmployeeRoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemDefaultReportFilterEmployeeRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemDefaultReportFilterEmployeeRoles_EmployeeRoles_EmployeeRoleId",
                        column: x => x.EmployeeRoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SystemDefaultReportFilterEmployeeRoles_SystemDefaultReportFilters_SystemDefaultReportFilterId",
                        column: x => x.SystemDefaultReportFilterId,
                        principalSchema: "dbo",
                        principalTable: "SystemDefaultReportFilters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SystemDefaultReportFilterEmployeeRoles_EmployeeRoleId",
                schema: "dbo",
                table: "SystemDefaultReportFilterEmployeeRoles",
                column: "EmployeeRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemDefaultReportFilterEmployeeRoles_SystemDefaultReportFilterId",
                schema: "dbo",
                table: "SystemDefaultReportFilterEmployeeRoles",
                column: "SystemDefaultReportFilterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemDefaultReportFilterEmployeeRoles",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SystemDefaultReportFilters",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "IsSystemDefault",
                schema: "dbo",
                table: "ClientUserReportFilter");

        }
    }
}
