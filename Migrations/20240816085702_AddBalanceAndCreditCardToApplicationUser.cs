using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBalanceAndCreditCardToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Buses");

            migrationBuilder.RenameColumn(
                name: "LicensePlate",
                table: "Buses",
                newName: "Operator");
        

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EstimatedArrival",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Line",
                table: "Buses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CreditCardNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "EstimatedArrival",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "Line",
                table: "Buses");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Operator",
                table: "Buses",
                newName: "LicensePlate");
            

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Buses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Buses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
