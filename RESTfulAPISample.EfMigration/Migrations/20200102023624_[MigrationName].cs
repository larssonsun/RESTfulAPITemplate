using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RESTfulAPISample.EfMigration.Demo
{
    public partial class MigrationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "My_Product",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: false),
                    IsOnSale = table.Column<bool>(nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_My_Product", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_My_Product_Name",
                table: "My_Product",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "My_Product");
        }
    }
}
