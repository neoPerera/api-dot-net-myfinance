using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Forms",
                table: "Forms");

            migrationBuilder.RenameTable(
                name: "Forms",
                newName: "tbl_forms");

            migrationBuilder.RenameIndex(
                name: "IX_Forms_str_form_id",
                table: "tbl_forms",
                newName: "IX_tbl_forms_str_form_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tbl_forms",
                table: "tbl_forms",
                column: "str_form_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tbl_forms",
                table: "tbl_forms");

            migrationBuilder.RenameTable(
                name: "tbl_forms",
                newName: "Forms");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_forms_str_form_id",
                table: "Forms",
                newName: "IX_Forms_str_form_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Forms",
                table: "Forms",
                column: "str_form_id");
        }
    }
}
