using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedClientLocationTypeOfService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationServiceTypes_AuthTypes_AuthTypeId",
                schema: "dbo",
                table: "ClientLocationServiceTypes");

            migrationBuilder.RenameColumn(
                name: "AuthTypeId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                newName: "TypeOfServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLocationServiceTypes_ClientLocationId_AuthTypeId_ClientId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                newName: "IX_ClientLocationServiceTypes_ClientLocationId_TypeOfServiceId_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLocationServiceTypes_AuthTypeId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                newName: "IX_ClientLocationServiceTypes_TypeOfServiceId");

            //migrationBuilder.AddColumn<int>(
            //    name: "ClientCptCodeId",
            //    schema: "IntegratedServices",
            //    table: "ClaimStatusBatchClaims",
            //    type: "int",
            //    nullable: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_ClaimStatusBatchClaims_ClientCptCodeId",
            //    schema: "IntegratedServices",
            //    table: "ClaimStatusBatchClaims",
            //    column: "ClientCptCodeId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_ClaimStatusBatchClaims_ClientCptCodes_ClientCptCodeId",
            //    schema: "IntegratedServices",
            //    table: "ClaimStatusBatchClaims",
            //    column: "ClientCptCodeId",
            //    principalSchema: "dbo",
            //    principalTable: "ClientCptCodes",
            //    principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationServiceTypes_TypesOfService_TypeOfServiceId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                column: "TypeOfServiceId",
                principalSchema: "dbo",
                principalTable: "TypesOfService",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClaimStatusBatchClaims_ClientCptCodes_ClientCptCodeId",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientLocationServiceTypes_TypesOfService_TypeOfServiceId",
                schema: "dbo",
                table: "ClientLocationServiceTypes");

            //migrationBuilder.DropIndex(
            //    name: "IX_ClaimStatusBatchClaims_ClientCptCodeId",
            //    schema: "IntegratedServices",
            //    table: "ClaimStatusBatchClaims");

            //migrationBuilder.DropColumn(
            //    name: "ClientCptCodeId",
            //    schema: "IntegratedServices",
            //    table: "ClaimStatusBatchClaims");

            migrationBuilder.RenameColumn(
                name: "TypeOfServiceId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                newName: "AuthTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLocationServiceTypes_TypeOfServiceId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                newName: "IX_ClientLocationServiceTypes_AuthTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLocationServiceTypes_ClientLocationId_TypeOfServiceId_ClientId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                newName: "IX_ClientLocationServiceTypes_ClientLocationId_AuthTypeId_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLocationServiceTypes_AuthTypes_AuthTypeId",
                schema: "dbo",
                table: "ClientLocationServiceTypes",
                column: "AuthTypeId",
                principalSchema: "dbo",
                principalTable: "AuthTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
