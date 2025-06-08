using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetTrack.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class fixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookingStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_BookingTime",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_BookingNotifications_Status",
                table: "BookingNotifications");

            migrationBuilder.DropColumn(
                name: "BookingTime",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Channel",
                table: "BookingNotifications");

            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "BookingNotifications");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "BookingNotifications");

            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "BookingNotifications");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "BookingNotifications",
                newName: "Subject");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "AppointmentDate",
                table: "Bookings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AppointmentDate",
                table: "Bookings",
                column: "AppointmentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_AppointmentDate",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "Subject",
                table: "BookingNotifications",
                newName: "Status");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingTime",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Channel",
                table: "BookingNotifications",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "BookingNotifications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                table: "BookingNotifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "BookingNotifications",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookingStatusHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChangeReason = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ChangedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DeletedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastUpdatedTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    NewStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OldStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingStatusHistories_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_BookingTime",
                table: "Bookings",
                column: "BookingTime");

            migrationBuilder.CreateIndex(
                name: "IX_BookingNotifications_Status",
                table: "BookingNotifications",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStatusHistories_BookingId",
                table: "BookingStatusHistories",
                column: "BookingId");
        }
    }
}
