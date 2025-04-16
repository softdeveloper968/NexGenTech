using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeClientUserReportFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeClientUserReportFilter",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientUserReportFilterId = table.Column<int>(type: "int", nullable: false),
                    EmployeeClientId = table.Column<int>(type: "int", nullable: false),
                    BaseClientUserReportFilterId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeClientUserReportFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeClientUserReportFilter_ClientUserReportFilter_ClientUserReportFilterId",
                        column: x => x.ClientUserReportFilterId,
                        principalSchema: "dbo",
                        principalTable: "ClientUserReportFilter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_EmployeeClientUserReportFilter_EmployeeClients_EmployeeClientId",
                        column: x => x.EmployeeClientId,
                        principalTable: "EmployeeClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientUserReportFilter_ClientUserReportFilterId",
                schema: "dbo",
                table: "EmployeeClientUserReportFilter",
                column: "ClientUserReportFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeClientUserReportFilter_EmployeeClientId",
                schema: "dbo",
                table: "EmployeeClientUserReportFilter",
                column: "EmployeeClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeClientUserReportFilter",
                schema: "dbo");
        }
    }
}
