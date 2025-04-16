using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedinsuranceFirstLastBilledOnToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "InsurancFirstBilledOn",
                schema: "dbo",
                table: "PatientLedgerCharges",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InsuranceLastBilledOn",
                schema: "dbo",
                table: "PatientLedgerCharges",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsurancFirstBilledOn",
                schema: "dbo",
                table: "PatientLedgerCharges");

            migrationBuilder.DropColumn(
                name: "InsuranceLastBilledOn",
                schema: "dbo",
                table: "PatientLedgerCharges");
        }
    }
}
