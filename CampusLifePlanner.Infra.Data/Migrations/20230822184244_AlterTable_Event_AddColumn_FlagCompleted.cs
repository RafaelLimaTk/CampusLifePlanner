using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusLifePlanner.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlterTable_Event_AddColumn_FlagCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Events",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Events");
        }
    }
}
