using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpeedrunHistoryVisualizerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddDateFavoritedToFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateFavorited",
                table: "Favorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateFavorited",
                table: "Favorites");
        }
    }
}
