using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniClinic.Migrations
{
    /// <inheritdoc />
    public partial class create_admin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password", "Role" },
                values: new object[] { 101, "admin@example.ba", "Super Admin", "IdDhTeM1sRWA2FD6wDmFPx9ZGJVBiDPdYiWFbftu5d/mXQmr0OMN7HL/ZEOZWcuB", 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 101);
        }
    }
}
