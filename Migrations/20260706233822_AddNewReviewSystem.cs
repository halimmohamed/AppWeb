using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAppSafeJourney.Migrations
{
    /// <inheritdoc />
    public partial class AddNewReviewSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_GuideBookings_GuideBookingId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_GuideBookingId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "GuideBookingId",
                table: "Reviews",
                newName: "TouristId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DestinationId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "GuideBookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_DestinationId",
                table: "Reviews",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TouristId",
                table: "Reviews",
                column: "TouristId");

            migrationBuilder.CreateIndex(
                name: "IX_GuideBookings_ReviewId",
                table: "GuideBookings",
                column: "ReviewId");

            migrationBuilder.AddForeignKey(
                name: "FK_GuideBookings_Reviews_ReviewId",
                table: "GuideBookings",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Destinations_DestinationId",
                table: "Reviews",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_TouristId",
                table: "Reviews",
                column: "TouristId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuideBookings_Reviews_ReviewId",
                table: "GuideBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Destinations_DestinationId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_TouristId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_DestinationId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_TouristId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_GuideBookings_ReviewId",
                table: "GuideBookings");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "GuideBookings");

            migrationBuilder.RenameColumn(
                name: "TouristId",
                table: "Reviews",
                newName: "GuideBookingId");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_GuideBookingId",
                table: "Reviews",
                column: "GuideBookingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_GuideBookings_GuideBookingId",
                table: "Reviews",
                column: "GuideBookingId",
                principalTable: "GuideBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
