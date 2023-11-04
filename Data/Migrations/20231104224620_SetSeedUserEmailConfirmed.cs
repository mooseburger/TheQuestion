using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Data.Migrations
{
    public partial class SetSeedUserEmailConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8cf618d1-9ae6-475e-8791-8badf12f99d4", true, "AQAAAAEAACcQAAAAEMuTng1EgJaSNjVSRClL6Rqpo9wOnkSmFtCHQitPIEgHcKKkqA6zxLuS1p3C1529dg==", "6d5e79c5-0bcd-4711-ad9b-91bc19ca4cf4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "b74ddd14-6340-4840-95c2-db12554843e5",
                columns: new[] { "ConcurrencyStamp", "EmailConfirmed", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f1ae3363-35cc-4aac-b00c-bdcfd3a1ccd0", false, "AQAAAAEAACcQAAAAEA2pnxpqm7VsBAp2TK+upQP/5fiOhQgXf2xzuUXa6YHzGZbIWyStg7YhWd8IbVFArQ==", "cfdbbc54-fda7-4cb5-b85f-12fe56559c11" });
        }
    }
}
