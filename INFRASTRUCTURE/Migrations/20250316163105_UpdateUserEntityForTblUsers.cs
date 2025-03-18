using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserEntityForTblUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "tbl_users");

            migrationBuilder.RenameIndex(
                name: "IX_Users_username",
                table: "tbl_users",
                newName: "IX_tbl_users_username");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_users",
                table: "tbl_users",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_users",
                table: "tbl_users");

            migrationBuilder.RenameTable(
                name: "tbl_users",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_users_username",
                table: "Users",
                newName: "IX_Users_username");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
