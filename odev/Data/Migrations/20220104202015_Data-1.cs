using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Data1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Vehicles (Plate,LocationX,LocationY) VALUES " +
                "('33ADS456',5,9), ('38KRV856',9,12), ('34BCN486',9,2);" +
                "INSERT INTO Containers  (LocationX,LocationY) VALUES" +
                "(5,6),(9,7),(25,9),(45,65),(2,5),(9,15),(75,62),(12,16),(-4,-10),(-52,-55),(23,42),(-15,-32),(-10,50),(32,-10),(5,-44)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Vehicles;" +
                "DELETE FROM Containers ");
        }
    }
}
 