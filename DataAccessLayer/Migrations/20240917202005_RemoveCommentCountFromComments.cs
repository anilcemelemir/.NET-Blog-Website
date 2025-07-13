using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class RemoveCommentCountFromComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "CommentCount",
            table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
