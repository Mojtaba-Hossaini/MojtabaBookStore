using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations
{
    public partial class Edit_ForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Translators_Translators_TranslatroID",
                table: "Book_Translators");

            migrationBuilder.DropForeignKey(
                name: "FK_BookInfo_Publishers_PublisherID",
                table: "BookInfo");

            migrationBuilder.RenameColumn(
                name: "TranslatroID",
                table: "Book_Translators",
                newName: "TranslatorID");

            migrationBuilder.RenameIndex(
                name: "IX_Book_Translators_TranslatroID",
                table: "Book_Translators",
                newName: "IX_Book_Translators_TranslatorID");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherID",
                table: "BookInfo",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Translators_Translators_TranslatorID",
                table: "Book_Translators",
                column: "TranslatorID",
                principalTable: "Translators",
                principalColumn: "TranslatorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInfo_Publishers_PublisherID",
                table: "BookInfo",
                column: "PublisherID",
                principalTable: "Publishers",
                principalColumn: "PublisherID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Translators_Translators_TranslatorID",
                table: "Book_Translators");

            migrationBuilder.DropForeignKey(
                name: "FK_BookInfo_Publishers_PublisherID",
                table: "BookInfo");

            migrationBuilder.RenameColumn(
                name: "TranslatorID",
                table: "Book_Translators",
                newName: "TranslatroID");

            migrationBuilder.RenameIndex(
                name: "IX_Book_Translators_TranslatorID",
                table: "Book_Translators",
                newName: "IX_Book_Translators_TranslatroID");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherID",
                table: "BookInfo",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Translators_Translators_TranslatroID",
                table: "Book_Translators",
                column: "TranslatroID",
                principalTable: "Translators",
                principalColumn: "TranslatorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookInfo_Publishers_PublisherID",
                table: "BookInfo",
                column: "PublisherID",
                principalTable: "Publishers",
                principalColumn: "PublisherID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
