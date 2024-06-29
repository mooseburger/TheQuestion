using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Migrations
{
    /// <inheritdoc />
    public partial class AnswerPopularity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rank",
                table: "Answers",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "TotalVotes",
                table: "Answers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "TotalVotes",
                table: "Answers");
        }
    }
}
