using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(694), new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(679) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(705), new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(705) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(713), new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(712) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(721), new DateTime(2023, 12, 11, 17, 2, 34, 642, DateTimeKind.Local).AddTicks(715) });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "CreatedDate", "RoleId", "UpdatedDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8320), 1, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8309), 1 },
                    { 2, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8337), 1, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8337), 2 },
                    { 3, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8339), 1, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8339), 3 },
                    { 4, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8341), 1, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8340), 4 },
                    { 5, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8342), 1, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8342), 5 },
                    { 6, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8346), 4, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8345), 2 },
                    { 7, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8347), 3, new DateTime(2023, 12, 11, 17, 2, 35, 859, DateTimeKind.Local).AddTicks(8347), 3 }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 34, 643, DateTimeKind.Local).AddTicks(5034), "$2a$11$bIXdrfHXtXvcoBGadqizFOCEVWOVuIWv291xV5Wfcu6qUSZxmaEb.", new DateTime(2023, 12, 11, 17, 2, 34, 643, DateTimeKind.Local).AddTicks(5027) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 34, 883, DateTimeKind.Local).AddTicks(8013), "$2a$11$Aj/TdD8NyPfQEBPA.tWtXOVMu4gPzFREil205twnwrhIF2UWdunT6", new DateTime(2023, 12, 11, 17, 2, 34, 883, DateTimeKind.Local).AddTicks(7981) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 35, 119, DateTimeKind.Local).AddTicks(7301), "$2a$11$Zl/LyrJMkcrzhjGyxMBrCu07lrUDcekIKv7U8cwst5p.JBlIsql/6", new DateTime(2023, 12, 11, 17, 2, 35, 119, DateTimeKind.Local).AddTicks(7288) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 35, 362, DateTimeKind.Local).AddTicks(5309), "$2a$11$AEESQh4DgkWl/p9UjW8/vOjxNAea8UiT3wJymqEL1Ovit.GE7kcVq", new DateTime(2023, 12, 11, 17, 2, 35, 362, DateTimeKind.Local).AddTicks(5289) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 17, 2, 35, 603, DateTimeKind.Local).AddTicks(6456), "$2a$11$CHySSpJd/OD52o87dZhsqeeCa2EkGDYceaXbhuxrAeUxovlQXbmuS", new DateTime(2023, 12, 11, 17, 2, 35, 603, DateTimeKind.Local).AddTicks(6449) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8774), new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8760) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8781), new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8781) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8788), new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8788) });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8799), new DateTime(2023, 12, 11, 16, 56, 52, 395, DateTimeKind.Local).AddTicks(8790) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 397, DateTimeKind.Local).AddTicks(174), "$2a$11$Lr0xth2hkPYsWCPqTsLfq.zyfYOElA/lQLUDWqng.eQWD1HxxrCl.", new DateTime(2023, 12, 11, 16, 56, 52, 397, DateTimeKind.Local).AddTicks(164) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 596, DateTimeKind.Local).AddTicks(7718), "$2a$11$HazDg2InVbKup1PODuRks./foE3Y40rmzX77X037cWVfR7X5V8vpy", new DateTime(2023, 12, 11, 16, 56, 52, 596, DateTimeKind.Local).AddTicks(7702) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 52, 827, DateTimeKind.Local).AddTicks(9793), "$2a$11$eDAFT1a4ISRsStAkAYjQg.aJopsNlum53QHhFXSwDQUe8KfB7aXci", new DateTime(2023, 12, 11, 16, 56, 52, 827, DateTimeKind.Local).AddTicks(9768) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 53, 56, DateTimeKind.Local).AddTicks(1054), "$2a$11$Edr9T63KKvJ1.0df2PzxWuMAif0eTh4u0GjD3BqBlQjKBh4oXYvrO", new DateTime(2023, 12, 11, 16, 56, 53, 56, DateTimeKind.Local).AddTicks(1039) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "Password", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 11, 16, 56, 53, 274, DateTimeKind.Local).AddTicks(9468), "$2a$11$EwPuT0Kx/iCnxcDBUyeYP.1/XxNAGlHM1Xzf7zQjrbQUtN/TWJAt2", new DateTime(2023, 12, 11, 16, 56, 53, 274, DateTimeKind.Local).AddTicks(9461) });
        }
    }
}
