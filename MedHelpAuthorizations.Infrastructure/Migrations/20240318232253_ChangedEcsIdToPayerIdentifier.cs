using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEcsIdToPayerIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EcsId",
                schema: "dbo",
                table: "ClientInsurances",
                newName: "PayerIdentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayerIdentifier",
                schema: "dbo",
                table: "ClientInsurances",
                newName: "EcsId");
        }
    }
}
