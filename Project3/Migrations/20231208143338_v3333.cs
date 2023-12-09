using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class v3333 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "ServiceContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMember",
                table: "RoomMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 8, 21, 33, 38, 782, DateTimeKind.Local).AddTicks(4615), new DateTime(2023, 12, 8, 21, 33, 38, 782, DateTimeKind.Local).AddTicks(4597) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "ServiceContents");

            migrationBuilder.DropColumn(
                name: "IsMember",
                table: "RoomMembers");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 6, 19, 8, 41, 437, DateTimeKind.Local).AddTicks(4572), new DateTime(2023, 12, 6, 19, 8, 41, 437, DateTimeKind.Local).AddTicks(4561) });
        }
    }
}
