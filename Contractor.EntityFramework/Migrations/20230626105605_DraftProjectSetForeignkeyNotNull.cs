using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class DraftProjectSetForeignkeyNotNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftProjects_AspNetUsers_ConsultantId",
                table: "DraftProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftProjects_AspNetUsers_OwnerId",
                table: "DraftProjects");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "DraftProjects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConsultantId",
                table: "DraftProjects",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftProjects_AspNetUsers_ConsultantId",
                table: "DraftProjects",
                column: "ConsultantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_DraftProjects_AspNetUsers_OwnerId",
                table: "DraftProjects",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DraftProjects_AspNetUsers_ConsultantId",
                table: "DraftProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_DraftProjects_AspNetUsers_OwnerId",
                table: "DraftProjects");

            migrationBuilder.AlterColumn<string>(
                name: "OwnerId",
                table: "DraftProjects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ConsultantId",
                table: "DraftProjects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftProjects_AspNetUsers_ConsultantId",
                table: "DraftProjects",
                column: "ConsultantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DraftProjects_AspNetUsers_OwnerId",
                table: "DraftProjects",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
