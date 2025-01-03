using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoredProcedureMigrationPrototype.API1.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20250103034201 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Category",
                table: "Products",
                newName: "CategoryEdit");

            migrationBuilder.AddColumn<string>(
                name: "Category2",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateRegistered = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropColumn(
                name: "Category2",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "CategoryEdit",
                table: "Products",
                newName: "Category");
        }
    }
}
