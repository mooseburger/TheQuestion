using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Data.Migrations
{
    public partial class SetSeedUserPass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f1ae3363-35cc-4aac-b00c-bdcfd3a1ccd0", "AQAAAAEAACcQAAAAEA2pnxpqm7VsBAp2TK+upQP/5fiOhQgXf2xzuUXa6YHzGZbIWyStg7YhWd8IbVFArQ==", "cfdbbc54-fda7-4cb5-b85f-12fe56559c11" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3b5468ab-f0a7-4350-a47e-a8facdf06165", null, "9cd078dc-7cba-4952-9a72-bc0febdc3bc0" });
        }
    }
}
