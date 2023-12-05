using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class newlestsss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceContents_Services_Service_Id",
                table: "ServiceContents");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRegistereds_ServicesPrice_Service_Price_Id",
                table: "ServiceRegistereds");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRegistereds_Users_User_Id",
                table: "ServiceRegistereds");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicesPrice_Services_Service_Id",
                table: "ServicesPrice");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicesPrice",
                table: "ServicesPrice");

            migrationBuilder.RenameTable(
                name: "ServicesPrice",
                newName: "ServicePrice");

            migrationBuilder.RenameColumn(
                name: "User_Id",
                table: "ServiceRegistereds",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Service_Price_Id",
                table: "ServiceRegistereds",
                newName: "Service_PriceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRegistereds_User_Id",
                table: "ServiceRegistereds",
                newName: "IX_ServiceRegistereds_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRegistereds_Service_Price_Id",
                table: "ServiceRegistereds",
                newName: "IX_ServiceRegistereds_Service_PriceId");

            migrationBuilder.RenameColumn(
                name: "Service_Id",
                table: "ServiceContents",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceContents_Service_Id",
                table: "ServiceContents",
                newName: "IX_ServiceContents_ServiceId");

            migrationBuilder.RenameColumn(
                name: "Service_Id",
                table: "ServicePrice",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_ServicesPrice_Service_Id",
                table: "ServicePrice",
                newName: "IX_ServicePrice_ServiceId");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "ServiceContents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicePrice",
                table: "ServicePrice",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 4, 22, 59, 20, 195, DateTimeKind.Local).AddTicks(4587), new DateTime(2023, 12, 4, 22, 59, 20, 195, DateTimeKind.Local).AddTicks(4571) });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceContents_Services_ServiceId",
                table: "ServiceContents",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePrice_Services_ServiceId",
                table: "ServicePrice",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRegistereds_ServicePrice_Service_PriceId",
                table: "ServiceRegistereds",
                column: "Service_PriceId",
                principalTable: "ServicePrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRegistereds_Users_UserId",
                table: "ServiceRegistereds",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceContents_Services_ServiceId",
                table: "ServiceContents");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePrice_Services_ServiceId",
                table: "ServicePrice");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRegistereds_ServicePrice_Service_PriceId",
                table: "ServiceRegistereds");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRegistereds_Users_UserId",
                table: "ServiceRegistereds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicePrice",
                table: "ServicePrice");

            migrationBuilder.DropColumn(
                name: "image",
                table: "ServiceContents");

            migrationBuilder.RenameTable(
                name: "ServicePrice",
                newName: "ServicesPrice");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ServiceRegistereds",
                newName: "User_Id");

            migrationBuilder.RenameColumn(
                name: "Service_PriceId",
                table: "ServiceRegistereds",
                newName: "Service_Price_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRegistereds_UserId",
                table: "ServiceRegistereds",
                newName: "IX_ServiceRegistereds_User_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceRegistereds_Service_PriceId",
                table: "ServiceRegistereds",
                newName: "IX_ServiceRegistereds_Service_Price_Id");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ServiceContents",
                newName: "Service_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ServiceContents_ServiceId",
                table: "ServiceContents",
                newName: "IX_ServiceContents_Service_Id");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "ServicesPrice",
                newName: "Service_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePrice_ServiceId",
                table: "ServicesPrice",
                newName: "IX_ServicesPrice_Service_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicesPrice",
                table: "ServicesPrice",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 12, 4, 20, 19, 25, 514, DateTimeKind.Local).AddTicks(694), new DateTime(2023, 12, 4, 20, 19, 25, 514, DateTimeKind.Local).AddTicks(672) });

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceContents_Services_Service_Id",
                table: "ServiceContents",
                column: "Service_Id",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRegistereds_ServicesPrice_Service_Price_Id",
                table: "ServiceRegistereds",
                column: "Service_Price_Id",
                principalTable: "ServicesPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRegistereds_Users_User_Id",
                table: "ServiceRegistereds",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicesPrice_Services_Service_Id",
                table: "ServicesPrice",
                column: "Service_Id",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
