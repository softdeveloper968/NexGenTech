using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PatientLedgerChargeClientNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientLedgerChargeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientNotes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "dbo",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_PatientLedgerChargeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "PatientLedgerChargeId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientNotes_ClientId",
                table: "ClientNotes",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClaimStatusBatchClaims_PatientLedgerCharges_PatientLedgerChargeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "PatientLedgerChargeId",
                principalSchema: "dbo",
                principalTable: "PatientLedgerCharges",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_PatientLedgerCharges_PatientLedgerChargeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropTable(
                name: "ClientNotes");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_PatientLedgerChargeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropColumn(
                name: "PatientLedgerChargeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");
        }
    }
}
