using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAnswerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectIndex",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "Result",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "TrueFalse",
                table: "Answers",
                newName: "TrueFalseAnswer");

            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Answers",
                newName: "OptionD");

            migrationBuilder.RenameColumn(
                name: "Choices",
                table: "Answers",
                newName: "OptionC");

            migrationBuilder.AddColumn<string>(
                name: "AnswerText",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionA",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptionB",
                table: "Answers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerText",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "OptionA",
                table: "Answers");

            migrationBuilder.DropColumn(
                name: "OptionB",
                table: "Answers");

            migrationBuilder.RenameColumn(
                name: "TrueFalseAnswer",
                table: "Answers",
                newName: "TrueFalse");

            migrationBuilder.RenameColumn(
                name: "OptionD",
                table: "Answers",
                newName: "Text");

            migrationBuilder.RenameColumn(
                name: "OptionC",
                table: "Answers",
                newName: "Choices");

            migrationBuilder.AddColumn<int>(
                name: "CorrectIndex",
                table: "Answers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Answers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Result",
                table: "Answers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
