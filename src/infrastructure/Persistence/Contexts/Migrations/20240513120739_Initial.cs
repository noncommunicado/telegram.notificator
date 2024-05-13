using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "group",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    sys_code = table.Column<string>(type: "text", nullable: false),
                    sys_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "message",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false),
                    disable_notification = table.Column<bool>(type: "boolean", nullable: false),
                    sys_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_message", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "group_member",
                columns: table => new
                {
                    chat_id = table.Column<long>(type: "bigint", nullable: false),
                    group_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sys_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_group_member", x => new { x.chat_id, x.group_id });
                    table.ForeignKey(
                        name: "fk_group_member_group_group_id",
                        column: x => x.group_id,
                        principalTable: "group",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_group_sys_code",
                table: "group",
                column: "sys_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_group_member_group_id",
                table: "group_member",
                column: "group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "group_member");

            migrationBuilder.DropTable(
                name: "message");

            migrationBuilder.DropTable(
                name: "group");
        }
    }
}
