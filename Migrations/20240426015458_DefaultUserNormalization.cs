using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Migrations
{
    /// <inheritdoc />
    public partial class DefaultUserNormalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "NormalizedEmail", "NormalizedUserName" },
                values: new object[] { "ADMIN@EXAMPLE.COM", "ADMIN" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "NormalizedEmail", "NormalizedUserName" },
                values: new object[] { "admin@example.com", "admin" });
        }
    }
}
