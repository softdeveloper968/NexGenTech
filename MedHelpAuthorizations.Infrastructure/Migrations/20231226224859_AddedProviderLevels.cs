using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedProviderLevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientFeeScheduleProviderLevels_ProviderLevel_ProviderLevelId",
                table: "ClientFeeScheduleProviderLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_ProviderLevel_ProviderLevelId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderLevel",
                table: "ProviderLevel");

            migrationBuilder.RenameTable(
                name: "ProviderLevel",
                newName: "ProviderLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderLevels",
                table: "ProviderLevels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientFeeScheduleProviderLevels_ProviderLevels_ProviderLevelId",
                table: "ClientFeeScheduleProviderLevels",
                column: "ProviderLevelId",
                principalTable: "ProviderLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_ProviderLevels_ProviderLevelId",
                schema: "dbo",
                table: "Providers",
                column: "ProviderLevelId",
                principalTable: "ProviderLevels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientFeeScheduleProviderLevels_ProviderLevels_ProviderLevelId",
                table: "ClientFeeScheduleProviderLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_Providers_ProviderLevels_ProviderLevelId",
                schema: "dbo",
                table: "Providers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProviderLevels",
                table: "ProviderLevels");

            migrationBuilder.RenameTable(
                name: "ProviderLevels",
                newName: "ProviderLevel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProviderLevel",
                table: "ProviderLevel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientFeeScheduleProviderLevels_ProviderLevel_ProviderLevelId",
                table: "ClientFeeScheduleProviderLevels",
                column: "ProviderLevelId",
                principalTable: "ProviderLevel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_ProviderLevel_ProviderLevelId",
                schema: "dbo",
                table: "Providers",
                column: "ProviderLevelId",
                principalTable: "ProviderLevel",
                principalColumn: "Id");
        }
    }
}
