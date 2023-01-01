using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPA_TravelTour_Db_Application.Migrations
{
    public partial class edited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Picture",
                table: "TravelAgents");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "TravelAgents",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "TravelAgents");

            migrationBuilder.AddColumn<string>(
                name: "Picture",
                table: "TravelAgents",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }
    }
}
