using Microsoft.EntityFrameworkCore.Migrations;

namespace Data.Migrations
{
    public partial class Data2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cluster",
                table: "Cluster");

            migrationBuilder.RenameTable(
                name: "Cluster",
                newName: "ContainerCluster");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ContainerCluster",
                table: "ContainerCluster",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ContainerCluster",
                table: "ContainerCluster");

            migrationBuilder.RenameTable(
                name: "ContainerCluster",
                newName: "Cluster");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cluster",
                table: "Cluster",
                column: "Id");
        }
    }
}
