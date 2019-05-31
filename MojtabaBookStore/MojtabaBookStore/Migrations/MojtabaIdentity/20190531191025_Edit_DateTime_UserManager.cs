using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MojtabaBookStore.Migrations.MojtabaIdentity
{
    public partial class Edit_DateTime_UserManager : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastVisitDateTime",
                table: "AppUsers",
                nullable: true,
                oldClrType: typeof(DateTime));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "LastVisitDateTime",
                table: "AppUsers",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
