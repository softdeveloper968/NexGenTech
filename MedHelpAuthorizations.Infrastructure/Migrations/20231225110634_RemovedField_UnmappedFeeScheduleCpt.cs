using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedField_UnmappedFeeScheduleCpt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
