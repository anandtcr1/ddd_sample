using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Contractor.Migrations
{
    /// <inheritdoc />
    public partial class AddTenderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OpenDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CloseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tenders_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TenderAccessDefinition",
                columns: table => new
                {
                    TenderId = table.Column<int>(type: "int", nullable: false),
                    AccessDefinitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderAccessDefinition", x => new { x.TenderId, x.AccessDefinitionId });
                    table.ForeignKey(
                        name: "FK_TenderAccessDefinition_AccessDefinitions_AccessDefinitionId",
                        column: x => x.AccessDefinitionId,
                        principalTable: "AccessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenderAccessDefinition_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TenderInvitation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenderId = table.Column<int>(type: "int", nullable: false),
                    ContractorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenderInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TenderInvitation_AspNetUsers_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TenderInvitation_Tenders_TenderId",
                        column: x => x.TenderId,
                        principalTable: "Tenders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvitationAccessDefinition",
                columns: table => new
                {
                    TenderInvitationId = table.Column<int>(type: "int", nullable: false),
                    AccessDefinitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationAccessDefinition", x => new { x.TenderInvitationId, x.AccessDefinitionId });
                    table.ForeignKey(
                        name: "FK_InvitationAccessDefinition_AccessDefinitions_AccessDefinitionId",
                        column: x => x.AccessDefinitionId,
                        principalTable: "AccessDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvitationAccessDefinition_TenderInvitation_TenderInvitationId",
                        column: x => x.TenderInvitationId,
                        principalTable: "TenderInvitation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvitationAccessDefinition_AccessDefinitionId",
                table: "InvitationAccessDefinition",
                column: "AccessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderAccessDefinition_AccessDefinitionId",
                table: "TenderAccessDefinition",
                column: "AccessDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderInvitation_ContractorId",
                table: "TenderInvitation",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_TenderInvitation_TenderId",
                table: "TenderInvitation",
                column: "TenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Tenders_ProjectId",
                table: "Tenders",
                column: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvitationAccessDefinition");

            migrationBuilder.DropTable(
                name: "TenderAccessDefinition");

            migrationBuilder.DropTable(
                name: "TenderInvitation");

            migrationBuilder.DropTable(
                name: "Tenders");
        }
    }
}
