using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class newwwwwwwww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMessages_Users_RoomId",
                table: "RoomMessages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 6, 19, 8, 41, 437, DateTimeKind.Local).AddTicks(4572), new DateTime(2023, 12, 6, 19, 8, 41, 437, DateTimeKind.Local).AddTicks(4561) });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMessages_UserId",
                table: "RoomMessages",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMessages_Users_UserId",
                table: "RoomMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMessages_Users_UserId",
                table: "RoomMessages");

            migrationBuilder.DropIndex(
                name: "IX_RoomMessages_UserId",
                table: "RoomMessages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 6, 15, 10, 25, 297, DateTimeKind.Local).AddTicks(4832), new DateTime(2023, 12, 6, 15, 10, 25, 297, DateTimeKind.Local).AddTicks(4818) });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMessages_Users_RoomId",
                table: "RoomMessages",
                column: "RoomId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
