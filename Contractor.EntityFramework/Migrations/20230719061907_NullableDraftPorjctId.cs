using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class NullableDraftPorjctId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_DraftProjects_DraftProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_DraftProjectId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "DraftProjectId",
                table: "Projects",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DraftProjectId",
                table: "Projects",
                column: "DraftProjectId",
                unique: true,
                filter: "[DraftProjectId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_DraftProjects_DraftProjectId",
                table: "Projects",
                column: "DraftProjectId",
                principalTable: "DraftProjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_DraftProjects_DraftProjectId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_DraftProjectId",
                table: "Projects");

            migrationBuilder.AlterColumn<int>(
                name: "DraftProjectId",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DraftProjectId",
                table: "Projects",
                column: "DraftProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_DraftProjects_DraftProjectId",
                table: "Projects",
                column: "DraftProjectId",
                principalTable: "DraftProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
