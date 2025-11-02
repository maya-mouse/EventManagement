using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinDateEventParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JoinDate",
                table: "EventParticipants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 1, 1 },
                column: "JoinDate",
                value: new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6643));

            migrationBuilder.UpdateData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 1, 2 },
                column: "JoinDate",
                value: new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6645));

            migrationBuilder.UpdateData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 2, 1 },
                column: "JoinDate",
                value: new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6645));

            migrationBuilder.UpdateData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 3, 2 },
                column: "JoinDate",
                value: new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6646));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 12, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6617));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 12, 17, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6624));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 11, 17, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6627));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateTime",
                value: new DateTime(2025, 11, 12, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6628));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinDate",
                table: "EventParticipants");

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2025, 11, 29, 19, 11, 19, 312, DateTimeKind.Utc).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2025, 12, 14, 19, 11, 19, 312, DateTimeKind.Utc).AddTicks(1904));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateTime",
                value: new DateTime(2025, 11, 14, 19, 11, 19, 312, DateTimeKind.Utc).AddTicks(1906));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateTime",
                value: new DateTime(2025, 11, 9, 19, 11, 19, 312, DateTimeKind.Utc).AddTicks(1907));
        }
    }
}
