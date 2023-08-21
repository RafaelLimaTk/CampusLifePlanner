using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CampusLifePlanner.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class ImgUse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgPath",
                table: "AspNetUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgPath",
                table: "AspNetUsers");
        }
    }
}
