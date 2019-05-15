using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations
{
    public partial class AddTranslator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Translators",
                columns: table => new
                {
                    TranslatorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Family = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translators", x => x.TranslatorID);
                });

            migrationBuilder.CreateTable(
                name: "Book_Translators",
                columns: table => new
                {
                    TranslatroID = table.Column<int>(nullable: false),
                    BookID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book_Translators", x => new { x.BookID, x.TranslatroID });
                    table.ForeignKey(
                        name: "FK_Book_Translators_BookInfo_BookID",
                        column: x => x.BookID,
                        principalTable: "BookInfo",
                        principalColumn: "BookID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Book_Translators_Translators_TranslatroID",
                        column: x => x.TranslatroID,
                        principalTable: "Translators",
                        principalColumn: "TranslatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_Translators_TranslatroID",
                table: "Book_Translators",
                column: "TranslatroID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book_Translators");

            migrationBuilder.DropTable(
                name: "Translators");
        }
    }
}
