using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StudentEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Sections_SectionId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SectionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "StudentsSections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsSections", x => new { x.SectionId, x.UserId });
                    table.ForeignKey(
                        name: "FK_StudentsSections_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentsSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsSections_UserId",
                table: "StudentsSections",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsSections");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SectionId",
                table: "AspNetUsers",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Sections_SectionId",
                table: "AspNetUsers",
                column: "SectionId",
                principalTable: "Sections",
                principalColumn: "Id");
        }
    }
}
