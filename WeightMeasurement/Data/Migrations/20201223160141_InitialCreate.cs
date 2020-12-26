using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeightMeasurement.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubUserWeights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubUserId = table.Column<int>(nullable: false),
                    Weight = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubUserWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubUserWeights_SubUsers_SubUserId",
                        column: x => x.SubUserId,
                        principalTable: "SubUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubUserWeights_SubUserId",
                table: "SubUserWeights",
                column: "SubUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubUserWeights");

            migrationBuilder.DropTable(
                name: "SubUsers");
        }
    }
}
