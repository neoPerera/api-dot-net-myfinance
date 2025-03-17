using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace INFRASTRUCTURE.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_trns",
                columns: table => new
                {
                    str_id = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    str_trn_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    str_trn_cat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    int_amount = table.Column<decimal>(type: "numeric", nullable: true),
                    dtm_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    str_reason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    str_user = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    str_account = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_trns", x => x.str_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_trns_str_id",
                table: "tbl_trns",
                column: "str_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_trns");
        }
    }
}
