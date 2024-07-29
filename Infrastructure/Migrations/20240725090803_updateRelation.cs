using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Subjects_SubjectId",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Materials",
                newName: "SectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_SubjectId",
                table: "Materials",
                newName: "IX_Materials_SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Sections_SectionId",
                table: "Materials",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Sections_SectionId",
                table: "Materials");

            migrationBuilder.RenameColumn(
                name: "SectionId",
                table: "Materials",
                newName: "SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_SectionId",
                table: "Materials",
                newName: "IX_Materials_SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Subjects_SubjectId",
                table: "Materials",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
