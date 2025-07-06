using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemBackend.Migrations
{
    /// <inheritdoc />
    public partial class removeuniqueconstraintroomIdmemberIdofroommembersaddindexusedatetime23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_MemberId_RoomId",
                table: "RoomMembers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "RoomMembers",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "RoomMembers",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DisabledStartTime",
                table: "RoomMembers",
                type: "datetime2(3)",
                precision: 3,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DisabledEndTime",
                table: "RoomMembers",
                type: "datetime2(3)",
                precision: 3,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredAt",
                table: "RefreshTokens",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Logs",
                type: "datetime2(3)",
                precision: 3,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_DisabledStartTime_DisabledEndTime",
                table: "RoomMembers",
                columns: new[] { "DisabledStartTime", "DisabledEndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_RoomId_MemberId",
                table: "RoomMembers",
                columns: new[] { "RoomId", "MemberId" });

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_StartTime_EndTime",
                table: "RoomMembers",
                columns: new[] { "StartTime", "EndTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_DisabledStartTime_DisabledEndTime",
                table: "RoomMembers");

            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_RoomId_MemberId",
                table: "RoomMembers");

            migrationBuilder.DropIndex(
                name: "IX_RoomMembers_StartTime_EndTime",
                table: "RoomMembers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DisabledStartTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DisabledEndTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiredAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Logs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(3)",
                oldPrecision: 3);

            migrationBuilder.CreateIndex(
                name: "IX_RoomMembers_MemberId_RoomId",
                table: "RoomMembers",
                columns: new[] { "MemberId", "RoomId" },
                unique: true);
        }
    }
}
