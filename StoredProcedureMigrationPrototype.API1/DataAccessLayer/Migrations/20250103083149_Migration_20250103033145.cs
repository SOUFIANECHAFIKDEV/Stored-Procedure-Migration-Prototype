using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoredProcedureMigrationPrototype.API1.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class Migration_20250103033145 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "ProductName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Name");
        }
    }
}
