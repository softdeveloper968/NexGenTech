using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOutstandingBalancePatientLedgerCharge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "OutstandingBalance",
            //    schema: "dbo",
            //    table: "PatientLedgerPayments");

            migrationBuilder.AddColumn<decimal>(
                name: "OutstandingBalance",
                schema: "dbo",
                table: "PatientLedgerCharges",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OutstandingBalance",
                schema: "dbo",
                table: "PatientLedgerCharges");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "OutstandingBalance",
            //    schema: "dbo",
            //    table: "PatientLedgerPayments",
            //    type: "decimal(18,2)",
            //    nullable: true);
        }
    }
}
