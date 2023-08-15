using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GMB.DAL.Migrations
{
    public partial class AddedSloganColumnToSlidersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slogan",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slogan",
                table: "Sliders");
        }
    }
}
