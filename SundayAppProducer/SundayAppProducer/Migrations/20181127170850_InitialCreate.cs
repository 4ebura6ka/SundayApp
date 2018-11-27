using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SundayAppProducer.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Municipality",
                columns: table => new
                {
                    MunicipalityId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipality", x => x.MunicipalityId);
                });

            migrationBuilder.CreateTable(
                name: "Ranges",
                columns: table => new
                {
                    RangeId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MunicipalityId = table.Column<int>(nullable: true),
                    RangeEnd = table.Column<DateTime>(nullable: false),
                    RangeName = table.Column<string>(nullable: true),
                    RangeStart = table.Column<DateTime>(nullable: false),
                    RangeTax = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranges", x => x.RangeId);
                    table.ForeignKey(
                        name: "FK_Ranges_Municipality_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipality",
                        principalColumn: "MunicipalityId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ranges_MunicipalityId",
                table: "Ranges",
                column: "MunicipalityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ranges");

            migrationBuilder.DropTable(
                name: "Municipality");
        }
    }
}
