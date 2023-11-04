using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Data.Migrations
{
    public partial class SetSeedUserNormalizedUserEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "NormalizedEmail", "NormalizedUserName" },
                values: new object[] { "admin@example.com", "admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "NormalizedEmail", "NormalizedUserName" },
                values: new object[] { null, null });
        }
    }
}
