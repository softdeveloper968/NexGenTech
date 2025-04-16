using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedX12ClaimCodesClaimLineItemStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "X12ClaimCategories",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_X12ClaimCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "X12ClaimCategoryCodeLineItemStatuses",
                schema: "IntegratedServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    X12ClaimCategoryId = table.Column<int>(type: "int", nullable: false),
                    ClaimLineItemStatusId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_X12ClaimCategoryCodeLineItemStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_X12ClaimCategoryCodeLineItemStatuses_ClaimLineItemStatuses_ClaimLineItemStatusId",
                        column: x => x.ClaimLineItemStatusId,
                        principalSchema: "IntegratedServices",
                        principalTable: "ClaimLineItemStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_X12ClaimCategoryCodeLineItemStatuses_X12ClaimCategories_X12ClaimCategoryId",
                        column: x => x.X12ClaimCategoryId,
                        principalSchema: "IntegratedServices",
                        principalTable: "X12ClaimCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_X12ClaimCategoryCodeLineItemStatuses_ClaimLineItemStatusId",
                schema: "IntegratedServices",
                table: "X12ClaimCategoryCodeLineItemStatuses",
                column: "ClaimLineItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_X12ClaimCategoryCodeLineItemStatuses_X12ClaimCategoryId",
                schema: "IntegratedServices",
                table: "X12ClaimCategoryCodeLineItemStatuses",
                column: "X12ClaimCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "X12ClaimCategoryCodeLineItemStatuses",
                schema: "IntegratedServices");

            migrationBuilder.DropTable(
                name: "X12ClaimCategories",
                schema: "IntegratedServices");
        }
    }
}
