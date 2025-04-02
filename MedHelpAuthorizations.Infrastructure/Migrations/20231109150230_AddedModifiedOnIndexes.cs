using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedModifiedOnIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Persons_CreatedOn",
                schema: "dbo",
                table: "Persons",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_LastModifiedOn",
                schema: "dbo",
                table: "Persons",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_CreatedOn",
                schema: "dbo",
                table: "InsuranceCards",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceCards_LastModifiedOn",
                schema: "dbo",
                table: "InsuranceCards",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InputDocuments_CreatedOn",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_InputDocuments_LastModifiedOn",
                schema: "IntegratedServices",
                table: "InputDocuments",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_CreatedOn",
                schema: "dbo",
                table: "ClientLocations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLocations_LastModifiedOn",
                schema: "dbo",
                table: "ClientLocations",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_CreatedOn",
                schema: "dbo",
                table: "ClientInsurances",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClientInsurances_LastModifiedOn",
                schema: "dbo",
                table: "ClientInsurances",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusWorkstationNotes_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactions_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusTransactionHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatches_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ClaimStatusBatchClaims_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactions_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactionHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryTransactionHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatchHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatches_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_ChargeEntryBatches_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_CreatedOn",
                schema: "dbo",
                table: "Cardholders",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Cardholders_LastModifiedOn",
                schema: "dbo",
                table: "Cardholders",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_CreatedOn",
                schema: "dbo",
                table: "Authorizations",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_LastModifiedOn",
                schema: "dbo",
                table: "Authorizations",
                column: "LastModifiedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Persons_CreatedOn",
                schema: "dbo",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Persons_LastModifiedOn",
                schema: "dbo",
                table: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCards_CreatedOn",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropIndex(
                name: "IX_InsuranceCards_LastModifiedOn",
                schema: "dbo",
                table: "InsuranceCards");

            migrationBuilder.DropIndex(
                name: "IX_InputDocuments_CreatedOn",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropIndex(
                name: "IX_InputDocuments_LastModifiedOn",
                schema: "IntegratedServices",
                table: "InputDocuments");

            migrationBuilder.DropIndex(
                name: "IX_ClientLocations_CreatedOn",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientLocations_LastModifiedOn",
                schema: "dbo",
                table: "ClientLocations");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsurances_CreatedOn",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_ClientInsurances_LastModifiedOn",
                schema: "dbo",
                table: "ClientInsurances");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusWorkstationNotes_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusWorkstationNotes");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTransactions_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTransactionLineItemStatusChangẹs_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionLineItemStatusChangẹs");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTransactionHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusTransactionHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatches_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatches");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_CreatedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropIndex(
                name: "IX_ClaimStatusBatchClaims_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ClaimStatusBatchClaims");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryTransactions_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryTransactionHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryTransactionHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryTransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryBatchHistories_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryBatchHistories_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatchHistories");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryBatches_CreatedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches");

            migrationBuilder.DropIndex(
                name: "IX_ChargeEntryBatches_LastModifiedOn",
                schema: "IntegratedServices",
                table: "ChargeEntryBatches");

            migrationBuilder.DropIndex(
                name: "IX_Cardholders_CreatedOn",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropIndex(
                name: "IX_Cardholders_LastModifiedOn",
                schema: "dbo",
                table: "Cardholders");

            migrationBuilder.DropIndex(
                name: "IX_Authorizations_CreatedOn",
                schema: "dbo",
                table: "Authorizations");

            migrationBuilder.DropIndex(
                name: "IX_Authorizations_LastModifiedOn",
                schema: "dbo",
                table: "Authorizations");
        }
    }
}
