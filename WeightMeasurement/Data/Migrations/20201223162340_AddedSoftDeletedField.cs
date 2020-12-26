using Microsoft.EntityFrameworkCore.Migrations;

namespace WeightMeasurement.Data.Migrations
{
    public partial class AddedSoftDeletedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SoftDeleted",
                table: "SubUserWeights",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoftDeleted",
                table: "SubUserWeights");
        }
    }
}
