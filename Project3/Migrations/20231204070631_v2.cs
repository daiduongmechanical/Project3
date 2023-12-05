using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMessages_Rooms_RoomId1",
                table: "RoomMessages");

            migrationBuilder.DropIndex(
                name: "IX_RoomMessages_RoomId1",
                table: "RoomMessages");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "RoomMessages");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 4, 14, 6, 31, 160, DateTimeKind.Local).AddTicks(4630), new DateTime(2023, 12, 4, 14, 6, 31, 160, DateTimeKind.Local).AddTicks(4605) });

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMessages_Rooms_RoomId",
                table: "RoomMessages",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMessages_Rooms_RoomId",
                table: "RoomMessages");

            migrationBuilder.AddColumn<int>(
                name: "RoomId1",
                table: "RoomMessages",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 4, 13, 52, 49, 956, DateTimeKind.Local).AddTicks(1489), new DateTime(2023, 12, 4, 13, 52, 49, 956, DateTimeKind.Local).AddTicks(1476) });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMessages_RoomId1",
                table: "RoomMessages",
                column: "RoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMessages_Rooms_RoomId1",
                table: "RoomMessages",
                column: "RoomId1",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
