using Microsoft.EntityFrameworkCore.Migrations;

namespace d6roleplayer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiceRollResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleplaySessionId = table.Column<string>(nullable: true),
                    Rolls = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    ResultMessage = table.Column<string>(nullable: true),
                    Success = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiceRollResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleplaySessions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleplaySessions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiceRollResults");

            migrationBuilder.DropTable(
                name: "RoleplaySessions");
        }
    }
}
