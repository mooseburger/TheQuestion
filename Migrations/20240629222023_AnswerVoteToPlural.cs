using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Migrations
{
    /// <inheritdoc />
    public partial class AnswerVoteToPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerVote_Answers_AnswerId",
                table: "AnswerVote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerVote",
                table: "AnswerVote");

            migrationBuilder.RenameTable(
                name: "AnswerVote",
                newName: "AnswerVotes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerVotes",
                table: "AnswerVotes",
                columns: new[] { "AnswerId", "IpAddress" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerVotes_Answers_AnswerId",
                table: "AnswerVotes",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnswerVotes_Answers_AnswerId",
                table: "AnswerVotes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AnswerVotes",
                table: "AnswerVotes");

            migrationBuilder.RenameTable(
                name: "AnswerVotes",
                newName: "AnswerVote");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnswerVote",
                table: "AnswerVote",
                columns: new[] { "AnswerId", "IpAddress" });

            migrationBuilder.AddForeignKey(
                name: "FK_AnswerVote_Answers_AnswerId",
                table: "AnswerVote",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
