using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addSectionIdToUsersQuestionQuizez : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SectionID",
                table: "UsersQuestionsQuizzes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UsersQuestionsQuizzes_SectionID",
                table: "UsersQuestionsQuizzes",
                column: "SectionID");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersQuestionsQuizzes_Sections_SectionID",
                table: "UsersQuestionsQuizzes",
                column: "SectionID",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersQuestionsQuizzes_Sections_SectionID",
                table: "UsersQuestionsQuizzes");

            migrationBuilder.DropIndex(
                name: "IX_UsersQuestionsQuizzes_SectionID",
                table: "UsersQuestionsQuizzes");

            migrationBuilder.DropColumn(
                name: "SectionID",
                table: "UsersQuestionsQuizzes");
        }
    }
}
