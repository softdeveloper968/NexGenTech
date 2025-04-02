using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedinsuranceFromPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_ClientInsurances_ClientInsuranceId",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_ClientInsuranceId",
                schema: "dbo",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ClientInsuranceId",
                schema: "dbo",
                table: "Patients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientInsuranceId",
                schema: "dbo",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ClientInsuranceId",
                schema: "dbo",
                table: "Patients",
                column: "ClientInsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_ClientInsurances_ClientInsuranceId",
                schema: "dbo",
                table: "Patients",
                column: "ClientInsuranceId",
                principalSchema: "dbo",
                principalTable: "ClientInsurances",
                principalColumn: "Id");
        }
    }
}
