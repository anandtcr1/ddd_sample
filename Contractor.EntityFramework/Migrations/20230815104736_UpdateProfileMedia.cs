using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProfileMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Files_ProfileCoverId",
                table: "UserProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfile_Files_ProfilePictureId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_ProfileCoverId",
                table: "UserProfile");

            migrationBuilder.DropIndex(
                name: "IX_UserProfile_ProfilePictureId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "ProfileCoverId",
                table: "UserProfile");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "UserProfile");

            migrationBuilder.CreateTable(
                name: "ProfileAccessDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessDefinitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileAccessDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileAccessDefinition_AccessDefinitions_AccessDefinitionId",
                        column: x => x.AccessDefinitionId,
                        principalTable: "AccessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileAccessDefinition_UserProfile_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfile",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAccessDefinition_AccessDefinitionId",
                table: "ProfileAccessDefinition",
                column: "AccessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileAccessDefinition_UserId",
                table: "ProfileAccessDefinition",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileAccessDefinition");

            migrationBuilder.AddColumn<int>(
                name: "ProfileCoverId",
                table: "UserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureId",
                table: "UserProfile",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ProfileCoverId",
                table: "UserProfile",
                column: "ProfileCoverId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfile_ProfilePictureId",
                table: "UserProfile",
                column: "ProfilePictureId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Files_ProfileCoverId",
                table: "UserProfile",
                column: "ProfileCoverId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfile_Files_ProfilePictureId",
                table: "UserProfile",
                column: "ProfilePictureId",
                principalTable: "Files",
                principalColumn: "Id");
        }
    }
}
