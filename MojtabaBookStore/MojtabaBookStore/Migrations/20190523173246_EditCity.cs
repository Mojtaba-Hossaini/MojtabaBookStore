using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations
{
    public partial class EditCity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provices_ProviceProvinceID",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "ProviceProvinceID",
                table: "Cities",
                newName: "ProvinceID");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProviceProvinceID",
                table: "Cities",
                newName: "IX_Cities_ProvinceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provices_ProvinceID",
                table: "Cities",
                column: "ProvinceID",
                principalTable: "Provices",
                principalColumn: "ProvinceID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Provices_ProvinceID",
                table: "Cities");

            migrationBuilder.RenameColumn(
                name: "ProvinceID",
                table: "Cities",
                newName: "ProviceProvinceID");

            migrationBuilder.RenameIndex(
                name: "IX_Cities_ProvinceID",
                table: "Cities",
                newName: "IX_Cities_ProviceProvinceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Provices_ProviceProvinceID",
                table: "Cities",
                column: "ProviceProvinceID",
                principalTable: "Provices",
                principalColumn: "ProvinceID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
