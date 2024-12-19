using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniqloMVC5.Migrations
{
    /// <inheritdoc />
    public partial class updatecolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Count",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Count",
                table: "Products");
        }
    }
}
