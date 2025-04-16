using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedErrorNCurlColumnInTx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurlScript",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurlScript",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurlScript",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropColumn(
                name: "CurlScript",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories");
        }
    }
}
