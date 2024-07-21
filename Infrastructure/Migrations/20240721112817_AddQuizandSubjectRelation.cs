using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuizandSubjectRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Sections_SectionId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Quiz",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_SectionId",
                table: "Quiz",
                newName: "IX_Quiz_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Subjects_SubjectId",
                table: "Quiz",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Quiz_Subjects_SubjectId",
                table: "Quiz");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Quiz",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Quiz_SubjectId",
                table: "Quiz",
                newName: "IX_Quiz_SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Quiz_Sections_SectionId",
                table: "Quiz",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
