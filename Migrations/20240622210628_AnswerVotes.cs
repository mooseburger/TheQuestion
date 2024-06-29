using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheQuestion.Migrations
{
    /// <inheritdoc />
    public partial class AnswerVotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnswerVote",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "integer", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    VoteDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnswerVote", x => new { x.AnswerId, x.IpAddress });
                    table.ForeignKey(
                        name: "FK_AnswerVote_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnswerVote");
        }
    }
}
