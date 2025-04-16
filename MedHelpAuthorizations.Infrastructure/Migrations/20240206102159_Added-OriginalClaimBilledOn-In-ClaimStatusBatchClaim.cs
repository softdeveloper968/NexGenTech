using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedOriginalClaimBilledOnInClaimStatusBatchClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OriginalClaimBilledOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalClaimBilledOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");
        }
    }
}
