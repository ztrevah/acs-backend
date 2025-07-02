using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SystemBackend.Migrations
{
    /// <inheritdoc />
    public partial class addtimecolumnsforRoomMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledEndTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DisabledStartTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "RoomMembers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddCheckConstraint(
                name: "CHK_RoomMember_DisabledPeriodValid",
                table: "RoomMembers",
                sql: "(DisabledStartTime IS NULL AND DisabledEndTime IS NULL) OR (DisabledStartTime IS NOT NULL AND DisabledEndTime IS NOT NULL AND DisabledStartTime < DisabledEndTime)");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_RoomMember_DisabledPeriodWithinMain",
                table: "RoomMembers",
                sql: "(DisabledStartTime IS NULL AND DisabledEndTime IS NULL) OR (DisabledStartTime IS NOT NULL AND DisabledEndTime IS NOT NULL AND DisabledStartTime >= StartTime AND DisabledEndTime <= EndTime)");

            migrationBuilder.AddCheckConstraint(
                name: "CHK_RoomMember_MainPeriodValid",
                table: "RoomMembers",
                sql: "StartTime < EndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CHK_RoomMember_DisabledPeriodValid",
                table: "RoomMembers");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_RoomMember_DisabledPeriodWithinMain",
                table: "RoomMembers");

            migrationBuilder.DropCheckConstraint(
                name: "CHK_RoomMember_MainPeriodValid",
                table: "RoomMembers");

            migrationBuilder.DropColumn(
                name: "DisabledEndTime",
                table: "RoomMembers");

            migrationBuilder.DropColumn(
                name: "DisabledStartTime",
                table: "RoomMembers");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "RoomMembers");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "RoomMembers");
        }
    }
}
