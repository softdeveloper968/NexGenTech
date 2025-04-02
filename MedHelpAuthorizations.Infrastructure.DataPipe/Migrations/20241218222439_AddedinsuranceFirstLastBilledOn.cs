using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Migrations
{
    /// <inheritdoc />
    public partial class AddedinsuranceFirstLastBilledOn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InsuranceFirstBilledOn",
                table: "tbl_Charges",
                type: "varchar(24)",
                unicode: false,
                maxLength: 24,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InsuranceLastBilledOn",
                table: "tbl_Charges",
                type: "varchar(24)",
                unicode: false,
                maxLength: 24,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceFirstBilledOn",
                table: "tbl_Charges");

            migrationBuilder.DropColumn(
                name: "InsuranceLastBilledOn",
                table: "tbl_Charges");
        }
    }
}
