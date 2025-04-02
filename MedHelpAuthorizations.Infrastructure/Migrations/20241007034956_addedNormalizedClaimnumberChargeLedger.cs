using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedNormalizedClaimnumberChargeLedger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Cardholders_Clients_ClientId",
            //    schema: "dbo",
            //    table: "Cardholders");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedClaimNumber",
                schema: "dbo",
                table: "PatientLedgerPayments",
                type: "nvarchar(max)",
                nullable: true);

            //migrationBuilder.AlterColumn<int>(
            //    name: "ClientId",
            //    schema: "dbo",
            //    table: "Cardholders",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Cardholders_Clients_ClientId",
            //    schema: "dbo",
            //    table: "Cardholders",
            //    column: "ClientId",
            //    principalSchema: "dbo",
            //    principalTable: "Clients",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Cardholders_Clients_ClientId",
            //    schema: "dbo",
            //    table: "Cardholders");

            migrationBuilder.DropColumn(
                name: "NormalizedClaimNumber",
                schema: "dbo",
                table: "PatientLedgerPayments");

            //migrationBuilder.AlterColumn<int>(
            //    name: "ClientId",
            //    schema: "dbo",
            //    table: "Cardholders",
            //    type: "int",
            //    nullable: true,
            //    oldClrType: typeof(int),
            //    oldType: "int");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Cardholders_Clients_ClientId",
            //    schema: "dbo",
            //    table: "Cardholders",
            //    column: "ClientId",
            //    principalSchema: "dbo",
            //    principalTable: "Clients",
            //    principalColumn: "Id");
        }
    }
}
