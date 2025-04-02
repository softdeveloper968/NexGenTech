using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreatedApiClaimsMessageClaimLineitemStatusMapEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiClaimsMessageClaimLineitemStatusMaps",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimLineItemStatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiClaimsMessageClaimLineitemStatusMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiClaimsMessageClaimLineitemStatusMaps_ClaimLineItemStatuses_ClaimLineItemStatusId",
                        column: x => x.ClaimLineItemStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimLineItemStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiClaimsMessageClaimLineitemStatusMaps_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "ApiClaimsMessageClaimLineitemStatusMaps",
                column: "ClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiClaimsMessageClaimLineitemStatusMaps_Code",
                schema: "IntegratedServices",
                table: "ApiClaimsMessageClaimLineitemStatusMaps",
                column: "Code");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiClaimsMessageClaimLineitemStatusMaps",
                schema: "IntegratedServices");
        }
    }
}
