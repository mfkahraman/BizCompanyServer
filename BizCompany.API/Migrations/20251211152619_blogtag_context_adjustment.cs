using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BizCompany.API.Migrations
{
    /// <inheritdoc />
    public partial class blogtag_context_adjustment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags");

            // Drop the existing Id column
            migrationBuilder.DropColumn(
                name: "Id",
                table: "BlogTags");

            // Recreate the Id column with IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "BlogTags",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_BlogId",
                table: "BlogTags",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags");

            migrationBuilder.DropIndex(
                name: "IX_BlogTags_BlogId",
                table: "BlogTags");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "BlogTags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogTags",
                table: "BlogTags",
                columns: new[] { "BlogId", "TagId" });
        }
    }
}
