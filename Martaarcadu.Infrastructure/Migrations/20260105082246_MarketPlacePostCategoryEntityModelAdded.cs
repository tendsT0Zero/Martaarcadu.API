using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Martaarcadu.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MarketPlacePostCategoryEntityModelAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketPlacePostCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketPlacePostCategories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketPlacePostCategories");
        }
    }
}
