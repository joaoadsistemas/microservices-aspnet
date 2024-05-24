using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeekShopping.OrderAPI.Migrations
{
    /// <inheritdoc />
    public partial class upddateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OrderTime",
                table: "Headers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderTime",
                table: "Headers");
        }
    }
}
