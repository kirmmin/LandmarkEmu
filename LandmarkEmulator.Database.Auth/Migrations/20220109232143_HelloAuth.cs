using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LandmarkEmulator.Database.Auth.Migrations
{
    public partial class HelloAuth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "zone_server",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint(20) unsigned", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    nameId = table.Column<uint>(type: "int(10) unsigned", nullable: false, defaultValue: 62147u),
                    host = table.Column<string>(type: "varchar(64)", nullable: false, defaultValue: "127.0.0.1")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    port = table.Column<ushort>(type: "smallint(5) unsigned", nullable: false, defaultValue: (ushort)19000),
                    flags = table.Column<uint>(type: "int(10) unsigned", nullable: false, defaultValue: 0u)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zone_server", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "zone_server",
                columns: new[] { "id", "flags", "host", "nameId", "port" },
                values: new object[] { 1ul, 1u, "127.0.0.1", 62147u, (ushort)19000 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "zone_server");
        }
    }
}
