using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLibrary.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverImageBase64ToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImageBase64",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImageBase64",
                table: "Books");
        }
    }
}
