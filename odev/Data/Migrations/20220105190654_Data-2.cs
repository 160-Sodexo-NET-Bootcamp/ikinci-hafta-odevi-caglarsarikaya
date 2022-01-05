using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Data2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Containers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Containers_VehicleId",
                table: "Containers",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Containers_Vehicles_VehicleId",
                table: "Containers",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Containers_Vehicles_VehicleId",
                table: "Containers");

            migrationBuilder.DropIndex(
                name: "IX_Containers_VehicleId",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Containers");
        }
    }
}
