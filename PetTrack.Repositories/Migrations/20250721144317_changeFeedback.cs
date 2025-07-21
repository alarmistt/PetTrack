using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTrack.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class changeFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId1",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_BookingId1",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "BookingId1",
                table: "Feedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingId",
                table: "Feedbacks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookingId1",
                table: "Feedbacks",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId",
                table: "Feedbacks",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_BookingId1",
                table: "Feedbacks",
                column: "BookingId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId",
                table: "Feedbacks",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Bookings_BookingId1",
                table: "Feedbacks",
                column: "BookingId1",
                principalTable: "Bookings",
                principalColumn: "Id");
        }
    }
}
