using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations
{
    public partial class AddSomeColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "BookInfo",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumOfPages",
                table: "BookInfo",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "Weight",
                table: "BookInfo",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "NumOfPages",
                table: "BookInfo");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "BookInfo");
        }
    }
}
