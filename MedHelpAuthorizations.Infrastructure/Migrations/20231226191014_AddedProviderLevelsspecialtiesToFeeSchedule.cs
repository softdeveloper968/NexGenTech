using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedProviderLevelsspecialtiesToFeeSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProviderLevelId",
                schema: "dbo",
                table: "Providers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ClientFeeScheduleSpecialties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientFeeScheduleId = table.Column<int>(type: "int", nullable: false),
                    SpecialtyId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFeeScheduleSpecialties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFeeScheduleSpecialties_ClientFeeSchedules_ClientFeeScheduleId",
                        column: x => x.ClientFeeScheduleId,
                        principalTable: "ClientFeeSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFeeScheduleSpecialties_Specialties_SpecialtyId",
                        column: x => x.SpecialtyId,
                        principalSchema: "dbo",
                        principalTable: "Specialties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProviderLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProviderLevel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientFeeScheduleProviderLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientFeeScheduleId = table.Column<int>(type: "int", nullable: false),
                    ProviderLevelId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientFeeScheduleProviderLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientFeeScheduleProviderLevels_ClientFeeSchedules_ClientFeeScheduleId",
                        column: x => x.ClientFeeScheduleId,
                        principalTable: "ClientFeeSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientFeeScheduleProviderLevels_ProviderLevel_ProviderLevelId",
                        column: x => x.ProviderLevelId,
                        principalTable: "ProviderLevel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Providers_ProviderLevelId",
                schema: "dbo",
                table: "Providers",
                column: "ProviderLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeScheduleProviderLevels_ClientFeeScheduleId",
                table: "ClientFeeScheduleProviderLevels",
                column: "ClientFeeScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeScheduleProviderLevels_ProviderLevelId",
                table: "ClientFeeScheduleProviderLevels",
                column: "ProviderLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeScheduleSpecialties_ClientFeeScheduleId",
                table: "ClientFeeScheduleSpecialties",
                column: "ClientFeeScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientFeeScheduleSpecialties_SpecialtyId",
                table: "ClientFeeScheduleSpecialties",
                column: "SpecialtyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_ProviderLevel_ProviderLevelId",
                schema: "dbo",
                table: "Providers",
                column: "ProviderLevelId",
                principalTable: "ProviderLevel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_ProviderLevel_ProviderLevelId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropTable(
                name: "ClientFeeScheduleProviderLevels");

            migrationBuilder.DropTable(
                name: "ClientFeeScheduleSpecialties");

            migrationBuilder.DropTable(
                name: "ProviderLevel");

            migrationBuilder.DropIndex(
                name: "IX_Providers_ProviderLevelId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "ProviderLevelId",
                schema: "dbo",
                table: "Providers");
        }
    }
}
