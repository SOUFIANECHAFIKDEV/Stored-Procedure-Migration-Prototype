using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoredProcedureMigrationPrototype.API2.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class _Auto_Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Orders",
                newName: "TotalPriceEdit");

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "TotalPriceEdit",
                table: "Orders",
                newName: "TotalPrice");
        }
    }
}
