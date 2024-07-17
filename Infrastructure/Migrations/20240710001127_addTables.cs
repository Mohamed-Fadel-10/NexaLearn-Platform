using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CorrectAnswer",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Question",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Question",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Option",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuestionId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShortText_CorrectAnswer",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShortText_QuestionId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrueFalse_CorrectAnswer",
                table: "Question",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrueFalse_QuestionId",
                table: "Question",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_QuestionId",
                table: "Question",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_ShortText_QuestionId",
                table: "Question",
                column: "ShortText_QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_TrueFalse_QuestionId",
                table: "Question",
                column: "TrueFalse_QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_QuestionId",
                table: "Question",
                column: "QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_ShortText_QuestionId",
                table: "Question",
                column: "ShortText_QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_TrueFalse_QuestionId",
                table: "Question",
                column: "TrueFalse_QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Question_QuestionId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Question_ShortText_QuestionId",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Question_TrueFalse_QuestionId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_QuestionId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_ShortText_QuestionId",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_TrueFalse_QuestionId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "CorrectAnswer",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Option",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "QuestionId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ShortText_CorrectAnswer",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "ShortText_QuestionId",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "TrueFalse_CorrectAnswer",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "TrueFalse_QuestionId",
                table: "Question");
        }
    }
}
