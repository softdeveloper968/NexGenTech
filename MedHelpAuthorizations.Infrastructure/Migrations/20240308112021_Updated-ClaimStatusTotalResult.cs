using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedClaimStatusTotalResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ClaimReceivedDate",
                table: "ClaimStatusTotalResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfServiceFrom",
                table: "ClaimStatusTotalResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfServiceTo",
                table: "ClaimStatusTotalResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionDate",
                table: "ClaimStatusTotalResults",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimReceivedDate",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "DateOfServiceFrom",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "DateOfServiceTo",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "TransactionDate",
                table: "ClaimStatusTotalResults");
        }
    }
}
