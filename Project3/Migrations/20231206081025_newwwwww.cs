using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class newwwwww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMembers_Users_UserId",
                table: "RoomMembers");

            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_UserId",
                table: "RoomMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "RoomMembers");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 6, 15, 10, 25, 297, DateTimeKind.Local).AddTicks(4832), new DateTime(2023, 12, 6, 15, 10, 25, 297, DateTimeKind.Local).AddTicks(4818) });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_MemberId",
                table: "RoomMembers",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMembers_Users_MemberId",
                table: "RoomMembers",
                column: "MemberId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomMembers_Users_MemberId",
                table: "RoomMembers");

            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_MemberId",
                table: "RoomMembers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "RoomMembers",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 4, 22, 59, 20, 195, DateTimeKind.Local).AddTicks(4587), new DateTime(2023, 12, 4, 22, 59, 20, 195, DateTimeKind.Local).AddTicks(4571) });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_UserId",
                table: "RoomMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomMembers_Users_UserId",
                table: "RoomMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
