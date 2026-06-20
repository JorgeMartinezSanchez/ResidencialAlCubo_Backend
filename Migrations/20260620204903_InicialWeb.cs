using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace rec_be.Migrations
{
    /// <inheritdoc />
    public partial class InicialWeb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "config",
                columns: table => new
                {
                    config_key = table.Column<string>(type: "text", nullable: false),
                    config_value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_config", x => x.config_key);
                });

            migrationBuilder.CreateTable(
                name: "guest",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    second_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    id_card = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guest", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room_type",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type_name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "service_contacts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    second_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    job_role = table.Column<string>(type: "text", nullable: false),
                    id_card = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    avialable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_service_contacts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "room",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_type_id = table.Column<int>(type: "integer", nullable: false),
                    room_number = table.Column<string>(type: "text", nullable: false),
                    occupied = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room", x => x.id);
                    table.ForeignKey(
                        name: "fk_room_room_types_room_type_id",
                        column: x => x.room_type_id,
                        principalTable: "room_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    room_id = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    check_in_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    check_out_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    total = table.Column<decimal>(type: "numeric", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_booking", x => x.id);
                    table.ForeignKey(
                        name: "fk_booking_room_room_id",
                        column: x => x.room_id,
                        principalTable: "room",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "late_check_out",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    extra_hours = table.Column<int>(type: "integer", nullable: false),
                    charge = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_late_check_out", x => x.id);
                    table.ForeignKey(
                        name: "fk_late_check_out_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "room_guest",
                columns: table => new
                {
                    booking_id = table.Column<int>(type: "integer", nullable: false),
                    guest_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_room_guest", x => new { x.booking_id, x.guest_id });
                    table.ForeignKey(
                        name: "fk_room_guest_booking_booking_id",
                        column: x => x.booking_id,
                        principalTable: "booking",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_room_guest_guest_guest_id",
                        column: x => x.guest_id,
                        principalTable: "guest",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_booking_room_id",
                table: "booking",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "ix_late_check_out_booking_id",
                table: "late_check_out",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_room_room_type_id",
                table: "room",
                column: "room_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_room_guest_guest_id",
                table: "room_guest",
                column: "guest_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "config");

            migrationBuilder.DropTable(
                name: "late_check_out");

            migrationBuilder.DropTable(
                name: "room_guest");

            migrationBuilder.DropTable(
                name: "service_contacts");

            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "guest");

            migrationBuilder.DropTable(
                name: "room");

            migrationBuilder.DropTable(
                name: "room_type");
        }
    }
}
