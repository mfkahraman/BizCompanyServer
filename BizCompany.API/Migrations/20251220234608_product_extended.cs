using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizCompany.API.Migrations
{
    /// <inheritdoc />
    public partial class product_extended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProjectDate",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientName",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProjectDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProjectUrl",
                table: "Products");
        }
    }
}
