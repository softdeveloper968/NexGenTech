using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClientLocationIdInInputDocument : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientLocationId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InputDocuments_ClientLocationId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "ClientLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InputDocuments_ClientLocations_ClientLocationId",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "ClientLocationId",
                principalSchema: "dbo",
                principalTable: "ClientLocations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputDocuments_ClientLocations_ClientLocationId",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropIndex(
                name: "IX_InputDocuments_ClientLocationId",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropColumn(
                name: "ClientLocationId",
                schema: "IntegratedServices",
                table: "InputDocuments");
        }
    }
}
