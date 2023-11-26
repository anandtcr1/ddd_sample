using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class AddUUpdateFileTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfileAttachment",
                table: "UserProfileAttachment");

            migrationBuilder.DropIndex(
                name: "IX_UserProfileAttachment_FileId",
                table: "UserProfileAttachment");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserProfileAttachment");

            migrationBuilder.RenameColumn(
                name: "BithDate",
                table: "UserProfile",
                newName: "BirthDate");

            migrationBuilder.AddColumn<int>(
                name: "ProfileCoverId",
                table: "UserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfileAttachment",
                table: "UserProfileAttachment",
                columns: new[] { "FileId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ProfileCoverId",
                table: "UserProfile",
                column: "ProfileCoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Files_ProfileCoverId",
                table: "UserProfile",
                column: "ProfileCoverId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Files_ProfileCoverId",
                table: "UserProfile");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfileAttachment",
                table: "UserProfileAttachment");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_ProfileCoverId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "ProfileCoverId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "UserProfile",
                newName: "BithDate");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserProfileAttachment",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfileAttachment",
                table: "UserProfileAttachment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfileAttachment_FileId",
                table: "UserProfileAttachment",
                column: "FileId");
        }
    }
}
