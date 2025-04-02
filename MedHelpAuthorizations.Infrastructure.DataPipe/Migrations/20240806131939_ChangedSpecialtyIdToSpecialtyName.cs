using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.DataPipe.Migrations
{
    /// <inheritdoc />
    public partial class ChangedSpecialtyIdToSpecialtyName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialtyId",
                table: "tbl_Providers");

            migrationBuilder.AddColumn<string>(
                name: "SpecialtyName",
                table: "tbl_Providers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialtyName",
                table: "tbl_Providers");

            migrationBuilder.AddColumn<int>(
                name: "SpecialtyId",
                table: "tbl_Providers",
                type: "int",
                nullable: true);
        }
    }
}
