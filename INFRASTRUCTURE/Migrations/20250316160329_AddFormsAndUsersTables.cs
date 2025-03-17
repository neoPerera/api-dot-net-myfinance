using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddFormsAndUsersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "Users",
                newName: "IX_Users_username");

            migrationBuilder.CreateTable(
                name: "Forms",
                columns: table => new
                {
                    str_form_id = table.Column<string>(type: "text", nullable: false),
                    str_form_name = table.Column<string>(type: "text", nullable: false),
                    str_menu_id = table.Column<string>(type: "text", nullable: false),
                    str_icon = table.Column<string>(type: "text", nullable: false),
                    str_link = table.Column<string>(type: "text", nullable: false),
                    str_component = table.Column<string>(type: "text", nullable: false),
                    str_isMenu = table.Column<char>(type: "character(1)", nullable: false),
                    str_active = table.Column<char>(type: "character(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.str_form_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forms_str_form_id",
                table: "Forms",
                column: "str_form_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forms");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameIndex(
                name: "IX_Users_username",
                table: "Users",
                newName: "IX_Users_Username");
        }
    }
}
