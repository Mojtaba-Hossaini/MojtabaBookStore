using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations
{
    public partial class Add_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name:"FK_Customers_AspUsers_CustomerID",
                table:"Customers",
                column:"CustomerID",
                principalTable:"AspNetUsers",
                principalColumn:"Id",
                onDelete: ReferentialAction.Restrict
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_AspUsers_CustomerID",
                table:"Customers"
                );
        }
    }
}
