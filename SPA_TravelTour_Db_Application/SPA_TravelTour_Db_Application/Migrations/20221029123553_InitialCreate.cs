using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPA_TravelTour_Db_Application.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourPackages",
                columns: table => new
                {
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageCategory = table.Column<int>(type: "int", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CostPerPerson = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TourTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourPackages", x => x.TourPackageId);
                });

            migrationBuilder.CreateTable(
                name: "TravelAgents",
                columns: table => new
                {
                    TravelAgentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AgentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AgentAddress = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelAgents", x => x.TravelAgentId);
                });

            migrationBuilder.CreateTable(
                name: "PackageFeatures",
                columns: table => new
                {
                    PackageFeatureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransportMode = table.Column<int>(type: "int", nullable: false),
                    HotelBooking = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageFeatures", x => x.PackageFeatureId);
                    table.ForeignKey(
                        name: "FK_PackageFeatures_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "TourPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tourists",
                columns: table => new
                {
                    TouristId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TouristName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BookingDate = table.Column<DateTime>(type: "date", nullable: false),
                    TouristOccupation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TouristPicture = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tourists", x => x.TouristId);
                    table.ForeignKey(
                        name: "FK_Tourists_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "TourPackageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgentTourPackages",
                columns: table => new
                {
                    TravelAgentId = table.Column<int>(type: "int", nullable: false),
                    TourPackageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgentTourPackages", x => new { x.TravelAgentId, x.TourPackageId });
                    table.ForeignKey(
                        name: "FK_AgentTourPackages_TourPackages_TourPackageId",
                        column: x => x.TourPackageId,
                        principalTable: "TourPackages",
                        principalColumn: "TourPackageId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgentTourPackages_TravelAgents_TravelAgentId",
                        column: x => x.TravelAgentId,
                        principalTable: "TravelAgents",
                        principalColumn: "TravelAgentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgentTourPackages_TourPackageId",
                table: "AgentTourPackages",
                column: "TourPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageFeatures_TourPackageId",
                table: "PackageFeatures",
                column: "TourPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Tourists_TourPackageId",
                table: "Tourists",
                column: "TourPackageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgentTourPackages");

            migrationBuilder.DropTable(
                name: "PackageFeatures");

            migrationBuilder.DropTable(
                name: "Tourists");

            migrationBuilder.DropTable(
                name: "TravelAgents");

            migrationBuilder.DropTable(
                name: "TourPackages");
        }
    }
}
