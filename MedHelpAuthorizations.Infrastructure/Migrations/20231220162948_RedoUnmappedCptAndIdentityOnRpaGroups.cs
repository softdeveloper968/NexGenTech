using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RedoUnmappedCptAndIdentityOnRpaGroups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "RpaInsuranceGroups",
                newName: "RpaInsuranceGroups",
                newSchema: "dbo");

            migrationBuilder.CreateTable(
                name: "UnmappedFeeScheduleCpts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientInsuranceId = table.Column<int>(type: "int", nullable: false),
                    DateOfServiceYear = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    BilledAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnmappedFeeScheduleCpts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UnmappedFeeScheduleCpts_ClientInsurances_ClientInsuranceId",
                        column: x => x.ClientInsuranceId,
                        principalSchema: "dbo",
                        principalTable: "ClientInsurances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UnmappedFeeScheduleCpts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UnmappedFeeScheduleCpts_ClientId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_UnmappedFeeScheduleCpts_ClientInsuranceId",
                schema: "dbo",
                table: "UnmappedFeeScheduleCpts",
                column: "ClientInsuranceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnmappedFeeScheduleCpts",
                schema: "dbo");

            migrationBuilder.RenameTable(
                name: "RpaInsuranceGroups",
                schema: "dbo",
                newName: "RpaInsuranceGroups");
        }
    }
}
