using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInhitance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "MultipleChoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultipleChoices_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShortText",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortText", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortText_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueFalse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorrectAnswer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueFalse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrueFalse_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoices_QuestionId",
                table: "MultipleChoices",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShortText_QuestionId",
                table: "ShortText",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueFalse_QuestionId",
                table: "TrueFalse",
                column: "QuestionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultipleChoices");

            migrationBuilder.DropTable(
                name: "ShortText");

            migrationBuilder.DropTable(
                name: "TrueFalse");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_ShortText_QuestionId",
                table: "Question",
                column: "ShortText_QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Question_TrueFalse_QuestionId",
                table: "Question",
                column: "TrueFalse_QuestionId",
                principalTable: "Question",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
