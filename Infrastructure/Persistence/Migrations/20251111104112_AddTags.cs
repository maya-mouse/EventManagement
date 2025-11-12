using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "EventParticipants",
                keyColumns: new[] { "EventId", "UserId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTags",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    TagId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTags", x => new { x.EventId, x.TagId });
                    table.ForeignKey(
                        name: "FK_EventTags_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventTags_TagId",
                table: "EventTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Users_Username",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "PasswordHash", "Username" },
                values: new object[,]
                {
                    { 1, "alice.org@event.com", "$2a$10$wU0T5zJ1z2z3z4z5z6z7z8z9z0z1z2z3z4z5z", "Alice" },
                    { 2, "bob.user@event.com", "$2a$10$xU0T5zJ1z2z3z4z5z6z7z8z9z0z1z2z3z4z5z", "Bob" }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Capacity", "DateTime", "Description", "HostId", "IsPublic", "Location", "Title" },
                values: new object[,]
                {
                    { 1, 100, new DateTime(2025, 12, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6617), "Largest tech gathering of the year.", 1, true, "Kyiv Expo Center", "Annual Tech Conference" },
                    { 2, 50, new DateTime(2025, 12, 17, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6624), "Hands-on coding session for contributors.", 1, true, "Online via Zoom", "Open Source Workshop" },
                    { 3, null, new DateTime(2025, 11, 17, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6627), "Discussion on classic literature.", 2, true, "Central Library", "Local Book Club Meeting" },
                    { 4, 10, new DateTime(2025, 11, 12, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6628), "Team building event (Private).", 1, false, "Italian Restaurant", "Private Team Dinner" }
                });

            migrationBuilder.InsertData(
                table: "EventParticipants",
                columns: new[] { "EventId", "UserId", "JoinDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6643) },
                    { 1, 2, new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6645) },
                    { 2, 1, new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6645) },
                    { 3, 2, new DateTime(2025, 11, 2, 10, 4, 31, 547, DateTimeKind.Utc).AddTicks(6646) }
                });
        }
    }
}
