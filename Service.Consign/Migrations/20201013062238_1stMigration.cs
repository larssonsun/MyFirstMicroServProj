using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.Consign.Migrations
{
    public partial class _1stMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consigns",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConsignNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consigns", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consigns");
        }
    }
}
