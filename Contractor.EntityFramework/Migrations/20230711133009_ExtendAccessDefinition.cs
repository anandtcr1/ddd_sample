using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class ExtendAccessDefinition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOwner",
                table: "AccessDefinitions",
                newName: "IsOriginal");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "AccessDefinitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OriginalId",
                table: "AccessDefinitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "AccessDefinitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AccessDefinitions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AccessDefinitions_FileId",
                table: "AccessDefinitions",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessDefinitions_OriginalId",
                table: "AccessDefinitions",
                column: "OriginalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessDefinitions_ParentId",
                table: "AccessDefinitions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessDefinitions_AccessDefinitions_OriginalId",
                table: "AccessDefinitions",
                column: "OriginalId",
                principalTable: "AccessDefinitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessDefinitions_AccessDefinitions_ParentId",
                table: "AccessDefinitions",
                column: "ParentId",
                principalTable: "AccessDefinitions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessDefinitions_Files_FileId",
                table: "AccessDefinitions",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessDefinitions_AccessDefinitions_OriginalId",
                table: "AccessDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessDefinitions_AccessDefinitions_ParentId",
                table: "AccessDefinitions");

            migrationBuilder.DropForeignKey(
                name: "FK_AccessDefinitions_Files_FileId",
                table: "AccessDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AccessDefinitions_FileId",
                table: "AccessDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AccessDefinitions_OriginalId",
                table: "AccessDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_AccessDefinitions_ParentId",
                table: "AccessDefinitions");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "AccessDefinitions");

            migrationBuilder.DropColumn(
                name: "OriginalId",
                table: "AccessDefinitions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "AccessDefinitions");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AccessDefinitions");

            migrationBuilder.RenameColumn(
                name: "IsOriginal",
                table: "AccessDefinitions",
                newName: "IsOwner");
        }
    }
}
