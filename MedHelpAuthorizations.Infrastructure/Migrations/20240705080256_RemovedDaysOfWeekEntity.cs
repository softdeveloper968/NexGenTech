using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedDaysOfWeekEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clientDaysOfOperation_DaysOfWeek_DayOfWeekId",
                table: "clientDaysOfOperation");

            migrationBuilder.DropTable(
                name: "DaysOfWeek");

            migrationBuilder.DropIndex(
                name: "IX_clientDaysOfOperation_DayOfWeekId",
                table: "clientDaysOfOperation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DaysOfWeek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaysOfWeek", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clientDaysOfOperation_DayOfWeekId",
                table: "clientDaysOfOperation",
                column: "DayOfWeekId");

            migrationBuilder.AddForeignKey(
                name: "FK_clientDaysOfOperation_DaysOfWeek_DayOfWeekId",
                table: "clientDaysOfOperation",
                column: "DayOfWeekId",
                principalTable: "DaysOfWeek",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
