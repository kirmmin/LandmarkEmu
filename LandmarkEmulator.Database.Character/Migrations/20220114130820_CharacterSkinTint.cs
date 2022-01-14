using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkEmulator.Database.Character.Migrations
{
    public partial class CharacterSkinTint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "skinTint",
                table: "character",
                type: "int(10) unsigned",
                nullable: false,
                defaultValue: 0u);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "skinTint",
                table: "character");
        }
    }
}
