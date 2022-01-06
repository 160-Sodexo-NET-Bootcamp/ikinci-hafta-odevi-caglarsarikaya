using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Data1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Containers",
                table: "Containers");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "Vehicle");

            migrationBuilder.RenameTable(
                name: "Containers",
                newName: "Container");

            migrationBuilder.AlterColumn<double>(
                name: "LocationY",
                table: "Vehicle",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "LocationX",
                table: "Vehicle",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "LocationY",
                table: "Container",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "LocationX",
                table: "Container",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Container",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Container",
                table: "Container",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Cluster",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContainerId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    LocationX = table.Column<double>(type: "float", nullable: false),
                    LocationY = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cluster", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Container_VehicleId",
                table: "Container",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Container_Vehicle_VehicleId",
                table: "Container",
                column: "VehicleId",
                principalTable: "Vehicle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("INSERT INTO Vehicle (Plate,LocationX,LocationY) VALUES " +
                "('33ADS456',5,9), ('38KRV856',9,12), ('34BCN486',9,2);" +
                "INSERT INTO Container  (LocationX,LocationY) VALUES" +
                "(5,6),(9,7),(25,9),(45,65),(2,5),(9,15),(75,62),(12,16),(-4,-10),(-52,-55),(23,42),(-15,-32),(-10,50),(32,-10),(5,-44)");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Container_Vehicle_VehicleId",
                table: "Container");

            migrationBuilder.DropTable(
                name: "Cluster");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicle",
                table: "Vehicle");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Container",
                table: "Container");

            migrationBuilder.DropIndex(
                name: "IX_Container_VehicleId",
                table: "Container");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Container");

            migrationBuilder.RenameTable(
                name: "Vehicle",
                newName: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Container",
                newName: "Containers");

            migrationBuilder.AlterColumn<int>(
                name: "LocationY",
                table: "Vehicles",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "LocationX",
                table: "Vehicles",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "LocationY",
                table: "Containers",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "LocationX",
                table: "Containers",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Containers",
                table: "Containers",
                column: "Id");

            migrationBuilder.Sql("DELETE FROM Vehicle;" +
                "DELETE FROM Container ");
        }
    }
}
