using Microsoft.EntityFrameworkCore.Migrations;

namespace Service.Sample.Migrations
{
    public partial class addColConsignNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConsignNo",
                table: "Samples",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsignNo",
                table: "Samples");
        }
    }
}
