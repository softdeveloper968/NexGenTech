using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MedHelpAuthorizations.Infrastructure.Identity.Migrations
{
    /// <inheritdoc />
    public partial class AddedIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "HashedPassword",
                schema: "Identity",
                table: "UsedPasswords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_CreatedOn",
                schema: "Identity",
                table: "UserLoginHistory",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_LastModifiedOn",
                schema: "Identity",
                table: "UserLoginHistory",
                column: "LastModifiedOn");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_LoginTime",
                schema: "Identity",
                table: "UserLoginHistory",
                column: "LoginTime");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginHistory_LogoutTime",
                schema: "Identity",
                table: "UserLoginHistory",
                column: "LogoutTime");

            migrationBuilder.CreateIndex(
                name: "IX_UsedPasswords_CreatedOn",
                schema: "Identity",
                table: "UsedPasswords",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_UsedPasswords_HashedPassword",
                schema: "Identity",
                table: "UsedPasswords",
                column: "HashedPassword");

            migrationBuilder.CreateIndex(
                name: "IX_UsedPasswords_LastModifiedOn",
                schema: "Identity",
                table: "UsedPasswords",
                column: "LastModifiedOn");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLoginHistory_CreatedOn",
                schema: "Identity",
                table: "UserLoginHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginHistory_LastModifiedOn",
                schema: "Identity",
                table: "UserLoginHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginHistory_LoginTime",
                schema: "Identity",
                table: "UserLoginHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginHistory_LogoutTime",
                schema: "Identity",
                table: "UserLoginHistory");

            migrationBuilder.DropIndex(
                name: "IX_UsedPasswords_CreatedOn",
                schema: "Identity",
                table: "UsedPasswords");

            migrationBuilder.DropIndex(
                name: "IX_UsedPasswords_HashedPassword",
                schema: "Identity",
                table: "UsedPasswords");

            migrationBuilder.DropIndex(
                name: "IX_UsedPasswords_LastModifiedOn",
                schema: "Identity",
                table: "UsedPasswords");

            migrationBuilder.AlterColumn<string>(
                name: "HashedPassword",
                schema: "Identity",
                table: "UsedPasswords",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
