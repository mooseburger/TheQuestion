using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Data.Migrations
{
    public partial class AnswerTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_AnswerStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AnswerStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AnswerStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "In Review" });

            migrationBuilder.InsertData(
                table: "AnswerStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Rejected" });

            migrationBuilder.InsertData(
                table: "AnswerStatuses",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Published" });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_Slug",
                table: "Answers",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Answers_StatusId",
                table: "Answers",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "AnswerStatuses");
        }
    }
}
