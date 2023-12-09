using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class bbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 9, 9, 2, 41, 777, DateTimeKind.Local).AddTicks(6430), new DateTime(2023, 12, 9, 9, 2, 41, 777, DateTimeKind.Local).AddTicks(6414) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Avatar", "CreatedDate", "Description", "Dob", "Email", "IsBlocked", "Password", "Phone", "UpdatedDate", "UserName", "Verified" },
                values: new object[,]
                {
                    { 5, null, "default.jpg", new DateTime(2023, 12, 9, 9, 2, 41, 778, DateTimeKind.Local).AddTicks(7955), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vainhoo@gmail.com", false, "$2a$11$zBCCImjNJrZiiX6k5qPEgu2yU9wlRJOs7h.p8Lg7qCz6/oc6/HPHu", "+84234456678", new DateTime(2023, 12, 9, 9, 2, 41, 778, DateTimeKind.Local).AddTicks(7949), "xhaka", true },
                    { 6, null, "default.jpg", new DateTime(2023, 12, 9, 9, 2, 41, 981, DateTimeKind.Local).AddTicks(5776), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vainhoo@gmail.com", false, "$2a$11$hmX3qOT0LHd5UCpA6m588ObGV4/LU3n.OVYet5O3RI7v7p97uc2UC", "+8477885566", new DateTime(2023, 12, 9, 9, 2, 41, 981, DateTimeKind.Local).AddTicks(5755), "saka", true },
                    { 7, null, "default.jpg", new DateTime(2023, 12, 9, 9, 2, 42, 183, DateTimeKind.Local).AddTicks(840), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vainhoo@gmail.com", false, "$2a$11$tG71xIqt1ADXcQg3hqW8OeLS7cqF8HG7ybQhwIdeYLszftZWsO8F.", "+84987765543", new DateTime(2023, 12, 9, 9, 2, 42, 183, DateTimeKind.Local).AddTicks(812), "rose", true },
                    { 8, null, "default.jpg", new DateTime(2023, 12, 9, 9, 2, 42, 377, DateTimeKind.Local).AddTicks(2923), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vainhoo@gmail.com", false, "$2a$11$ShC5/ojREsfdduaad3EtJue0z3X1HuMkNjgaRNAKQwvXGEGwljlae", "+8422665544", new DateTime(2023, 12, 9, 9, 2, 42, 377, DateTimeKind.Local).AddTicks(2905), "atetar", true },
                    { 9, null, "default.jpg", new DateTime(2023, 12, 9, 9, 2, 42, 564, DateTimeKind.Local).AddTicks(6929), null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "vainhoo@gmail.com", false, "$2a$11$BL2bDiW/XOe08E.3qTXMXOP7LmUnp04LzhRv.7pVqA6qxB/BcQfBC", "+84987865454", new DateTime(2023, 12, 9, 9, 2, 42, 564, DateTimeKind.Local).AddTicks(6900), "enketia", true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 8, 21, 33, 38, 782, DateTimeKind.Local).AddTicks(4615), new DateTime(2023, 12, 8, 21, 33, 38, 782, DateTimeKind.Local).AddTicks(4597) });
        }
    }
}
