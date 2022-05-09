using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Boards.Migrations
{
    public partial class AdditionWorkItemStateSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkItemStates",
                column: "Value",
                value: "OnHold");

            migrationBuilder.InsertData(
                table: "WorkItemStates",
                column: "Value",
                value: "Rejected");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Value",
                keyValue: "OnHold");

            migrationBuilder.DeleteData(
                table: "WorkItemStates",
                keyColumn: "Value",
                keyValue: "Rejected");
        }
    }
}
