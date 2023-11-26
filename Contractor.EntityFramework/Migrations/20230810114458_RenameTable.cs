using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class RenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_CorrespondenceThread_ThreadId",
                table: "Correspondences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CorrespondenceThread",
                table: "CorrespondenceThread");

            migrationBuilder.RenameTable(
                name: "CorrespondenceThread",
                newName: "CorrespondenceThreads");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorrespondenceThreads",
                table: "CorrespondenceThreads",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_CorrespondenceThreads_ThreadId",
                table: "Correspondences",
                column: "ThreadId",
                principalTable: "CorrespondenceThreads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondences_CorrespondenceThreads_ThreadId",
                table: "Correspondences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CorrespondenceThreads",
                table: "CorrespondenceThreads");

            migrationBuilder.RenameTable(
                name: "CorrespondenceThreads",
                newName: "CorrespondenceThread");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorrespondenceThread",
                table: "CorrespondenceThread",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondences_CorrespondenceThread_ThreadId",
                table: "Correspondences",
                column: "ThreadId",
                principalTable: "CorrespondenceThread",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
