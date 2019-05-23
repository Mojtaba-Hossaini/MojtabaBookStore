using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations
{
    public partial class Edit_Provinces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provices_ProvinceID",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provices",
                table: "Provices");

            migrationBuilder.RenameTable(
                name: "Provices",
                newName: "Provinces");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces",
                column: "ProvinceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provinces_ProvinceID",
                table: "Cities",
                column: "ProvinceID",
                principalTable: "Provinces",
                principalColumn: "ProvinceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provinces_ProvinceID",
                table: "Cities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Provinces",
                table: "Provinces");

            migrationBuilder.RenameTable(
                name: "Provinces",
                newName: "Provices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Provices",
                table: "Provices",
                column: "ProvinceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provices_ProvinceID",
                table: "Cities",
                column: "ProvinceID",
                principalTable: "Provices",
                principalColumn: "ProvinceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
