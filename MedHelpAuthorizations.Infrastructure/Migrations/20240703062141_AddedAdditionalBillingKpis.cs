using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedAdditionalBillingKpis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BDRate",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CashCollections",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CollectionPercentage",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DaysInAR",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DenialRate",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Over90Days",
                schema: "dbo",
                table: "ClientKpi",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Visits",
                schema: "dbo",
                table: "ClientKpi",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BDRate",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "CashCollections",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "CollectionPercentage",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "DaysInAR",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "DenialRate",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "Over90Days",
                schema: "dbo",
                table: "ClientKpi");

            migrationBuilder.DropColumn(
                name: "Visits",
                schema: "dbo",
                table: "ClientKpi");
        }
    }
}
