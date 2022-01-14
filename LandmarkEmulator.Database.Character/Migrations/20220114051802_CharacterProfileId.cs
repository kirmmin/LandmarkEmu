using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkEmulator.Database.Character.Migrations
{
    public partial class CharacterProfileId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "bodyType",
                table: "character",
                newName: "profileTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "profileTypeId",
                table: "character",
                newName: "bodyType");
        }
    }
}
