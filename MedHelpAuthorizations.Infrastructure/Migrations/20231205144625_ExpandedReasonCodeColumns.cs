using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExpandedReasonCodeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          
            migrationBuilder.AlterColumn<string>(
                name: "ReasonCode",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                type: "nvarchar(72)",
                maxLength: 72,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)",
                oldMaxLength: 24,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReasonCode",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                type: "nvarchar(72)",
                maxLength: 72,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(24)",
                oldMaxLength: 24,
                oldNullable: true);           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {           

            migrationBuilder.AlterColumn<string>(
                name: "ReasonCode",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(72)",
                oldMaxLength: 72,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReasonCode",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(72)",
                oldMaxLength: 72,
                oldNullable: true);
        }
    }
}
