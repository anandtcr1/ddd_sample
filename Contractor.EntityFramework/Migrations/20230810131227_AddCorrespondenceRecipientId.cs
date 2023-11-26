using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrespondenceRecipientId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CorrespondenceRecipient",
                table: "CorrespondenceRecipient");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "CorrespondenceRecipient",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorrespondenceRecipient",
                table: "CorrespondenceRecipient",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceRecipient_CorrespondenceId",
                table: "CorrespondenceRecipient",
                column: "CorrespondenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CorrespondenceRecipient",
                table: "CorrespondenceRecipient");

            migrationBuilder.DropIndex(
                name: "IX_CorrespondenceRecipient_CorrespondenceId",
                table: "CorrespondenceRecipient");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "CorrespondenceRecipient");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CorrespondenceRecipient",
                table: "CorrespondenceRecipient",
                columns: new[] { "CorrespondenceId", "RecipientId" });
        }
    }
}
