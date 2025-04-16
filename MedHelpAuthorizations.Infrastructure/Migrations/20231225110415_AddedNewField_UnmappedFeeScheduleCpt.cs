using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewField_UnmappedFeeScheduleCpt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReferencedDateOfServiceFrom",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferencedDateOfServiceFrom",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");

            migrationBuilder.DropColumn(
                name: "Year",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");
        }
    }
}
