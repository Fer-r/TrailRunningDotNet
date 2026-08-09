using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RacehubApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRaceGender : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "TrailRunnings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "TrailRunnings",
                type: "TEXT",
                nullable: true);
        }
    }
}
