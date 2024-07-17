using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateUsersEvaluations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersQuestionsQuizzes",
                table: "UsersQuestionsQuizzes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UsersQuestionsQuizzes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersQuestionsQuizzes",
                table: "UsersQuestionsQuizzes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UsersQuestionsQuizzes_QuizID_QuestionID_UserId",
                table: "UsersQuestionsQuizzes",
                columns: new[] { "QuizID", "QuestionID", "UserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersQuestionsQuizzes",
                table: "UsersQuestionsQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_UsersQuestionsQuizzes_QuizID_QuestionID_UserId",
                table: "UsersQuestionsQuizzes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UsersQuestionsQuizzes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersQuestionsQuizzes",
                table: "UsersQuestionsQuizzes",
                columns: new[] { "QuizID", "QuestionID", "UserId" });
        }
    }
}
