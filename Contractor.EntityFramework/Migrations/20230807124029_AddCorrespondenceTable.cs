using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrespondenceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CorrespondenceThread",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenceThread", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Correspondences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThreadId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Correspondences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Correspondences_CorrespondenceThread_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "CorrespondenceThread",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Correspondences_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CorrespondenceAccessDefinition",
                columns: table => new
                {
                    CorrespondenceId = table.Column<int>(type: "int", nullable: false),
                    AccessDefinitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenceAccessDefinition", x => new { x.CorrespondenceId, x.AccessDefinitionId });
                    table.ForeignKey(
                        name: "FK_CorrespondenceAccessDefinition_AccessDefinitions_AccessDefinitionId",
                        column: x => x.AccessDefinitionId,
                        principalTable: "AccessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CorrespondenceAccessDefinition_Correspondences_CorrespondenceId",
                        column: x => x.CorrespondenceId,
                        principalTable: "Correspondences",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CorrespondenceRecipient",
                columns: table => new
                {
                    CorrespondenceId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecipientType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorrespondenceRecipient", x => new { x.CorrespondenceId, x.RecipientId });
                    table.ForeignKey(
                        name: "FK_CorrespondenceRecipient_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CorrespondenceRecipient_Correspondences_CorrespondenceId",
                        column: x => x.CorrespondenceId,
                        principalTable: "Correspondences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceAccessDefinition_AccessDefinitionId",
                table: "CorrespondenceAccessDefinition",
                column: "AccessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_CorrespondenceRecipient_RecipientId",
                table: "CorrespondenceRecipient",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_ProjectId",
                table: "Correspondences",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondences_ThreadId",
                table: "Correspondences",
                column: "ThreadId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CorrespondenceAccessDefinition");

            migrationBuilder.DropTable(
                name: "CorrespondenceRecipient");

            migrationBuilder.DropTable(
                name: "Correspondences");

            migrationBuilder.DropTable(
                name: "CorrespondenceThread");
        }
    }
}
