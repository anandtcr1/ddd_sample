using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class AddPages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "AspNetUserClaims",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "AspNetRoleClaims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_PageId",
                table: "AspNetUserClaims",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_PageId",
                table: "AspNetRoleClaims",
                column: "PageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Pages_PageId",
                table: "AspNetRoleClaims",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_Pages_PageId",
                table: "AspNetUserClaims",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Pages_PageId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_Pages_PageId",
                table: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_PageId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_PageId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "AspNetRoleClaims");
        }
    }
}
