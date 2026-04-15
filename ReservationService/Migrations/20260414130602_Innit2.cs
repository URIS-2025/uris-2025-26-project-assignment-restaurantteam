using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ReservationService.Migrations
{
    /// <inheritdoc />
    public partial class Innit2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tables_TableIdTable",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_TableIdTable",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "TableIdTable",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_IdTable",
                table: "Reservations",
                column: "IdTable");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tables_IdTable",
                table: "Reservations",
                column: "IdTable",
                principalTable: "Tables",
                principalColumn: "IdTable",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Tables_IdTable",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_IdTable",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "TableIdTable",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_TableIdTable",
                table: "Reservations",
                column: "TableIdTable");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Tables_TableIdTable",
                table: "Reservations",
                column: "TableIdTable",
                principalTable: "Tables",
                principalColumn: "IdTable",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
