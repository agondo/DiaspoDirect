using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiaspoDirect.Migrations
{
    /// <inheritdoc />
    public partial class SyncFirstName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Column already exists in DB with NOT NULL; just make it nullable.
            migrationBuilder.Sql(@"ALTER TABLE ""AspNetUsers"" ALTER COLUMN ""FirstName"" DROP NOT NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");
        }
    }
}
