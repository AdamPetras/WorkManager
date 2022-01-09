using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkManager.DAL.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanySet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySet_UserSet_IdUser",
                        column: x => x.IdUser,
                        principalTable: "UserSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskGroupSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IdUser = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskGroupSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskGroupSet_UserSet_IdUser",
                        column: x => x.IdUser,
                        principalTable: "UserSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActualDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdCompany = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    WorkTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    PricePerHour = table.Column<double>(type: "double precision", nullable: false),
                    Pieces = table.Column<long>(type: "bigint", nullable: false),
                    PricePerPiece = table.Column<double>(type: "double precision", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSet_CompanySet_IdCompany",
                        column: x => x.IdCompany,
                        principalTable: "CompanySet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KanbanSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    StateOrder = table.Column<int>(type: "integer", nullable: false),
                    IconName = table.Column<string>(type: "text", nullable: true),
                    IdTaskGroup = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KanbanSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KanbanSet_TaskGroupSet_IdTaskGroup",
                        column: x => x.IdTaskGroup,
                        principalTable: "TaskGroupSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ActualDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TaskDoneDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdState = table.Column<Guid>(type: "uuid", nullable: false),
                    IdTaskGroup = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    WorkTime = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskSet_KanbanSet_IdState",
                        column: x => x.IdState,
                        principalTable: "KanbanSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskSet_TaskGroupSet_IdTaskGroup",
                        column: x => x.IdTaskGroup,
                        principalTable: "TaskGroupSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImageSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Path = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TaskId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImageSet_TaskSet_TaskId",
                        column: x => x.TaskId,
                        principalTable: "TaskSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanySet_IdUser",
                table: "CompanySet",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_ImageSet_TaskId",
                table: "ImageSet",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_KanbanSet_IdTaskGroup",
                table: "KanbanSet",
                column: "IdTaskGroup");

            migrationBuilder.CreateIndex(
                name: "IX_TaskGroupSet_IdUser",
                table: "TaskGroupSet",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSet_IdState",
                table: "TaskSet",
                column: "IdState");

            migrationBuilder.CreateIndex(
                name: "IX_TaskSet_IdTaskGroup",
                table: "TaskSet",
                column: "IdTaskGroup");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSet_IdCompany",
                table: "WorkSet",
                column: "IdCompany");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageSet");

            migrationBuilder.DropTable(
                name: "WorkSet");

            migrationBuilder.DropTable(
                name: "TaskSet");

            migrationBuilder.DropTable(
                name: "CompanySet");

            migrationBuilder.DropTable(
                name: "KanbanSet");

            migrationBuilder.DropTable(
                name: "TaskGroupSet");

            migrationBuilder.DropTable(
                name: "UserSet");
        }
    }
}
