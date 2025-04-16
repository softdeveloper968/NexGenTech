using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedSpellinginsuranceFirstLastBilledOnToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InsurancFirstBilledOn",
                schema: "dbo",
                table: "PatientLedgerCharges",
                newName: "InsuranceFirstBilledOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InsuranceFirstBilledOn",
                schema: "dbo",
                table: "PatientLedgerCharges",
                newName: "InsurancFirstBilledOn");
        }
    }
}
