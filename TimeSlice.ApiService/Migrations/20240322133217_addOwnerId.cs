using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeSlice.ApiService.Migrations
{
    /// <inheritdoc />
    public partial class addOwnerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "TimeboxEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "TimeboxEntries");
        }
    }
}
