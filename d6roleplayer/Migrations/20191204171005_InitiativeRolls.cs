using Microsoft.EntityFrameworkCore.Migrations;

namespace d6roleplayer.Migrations
{
    public partial class InitiativeRolls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InitiativeRollResults",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleplaySessionId = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Roll = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InitiativeRollResults", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InitiativeRollResults");
        }
    }
}
