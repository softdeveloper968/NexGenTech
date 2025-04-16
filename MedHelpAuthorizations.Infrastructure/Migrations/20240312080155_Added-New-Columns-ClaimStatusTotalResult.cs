using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumnsClaimStatusTotalResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClaimLineItemStatus",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "ClaimStatusExceptionReasonCategory",
                table: "ClaimStatusTotalResults");

            migrationBuilder.AddColumn<DateTime>(
                name: "BilledOnDate",
                table: "ClaimStatusTotalResults",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClaimStatusExceptionReasonCategoryId",
                table: "ClaimStatusTotalResults",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientInsuranceId",
                table: "ClaimStatusTotalResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BilledOnDate",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "ClaimStatusExceptionReasonCategoryId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.DropColumn(
                name: "ClientInsuranceId",
                table: "ClaimStatusTotalResults");

            migrationBuilder.AddColumn<string>(
                name: "ClaimLineItemStatus",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimStatusExceptionReasonCategory",
                table: "ClaimStatusTotalResults",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
