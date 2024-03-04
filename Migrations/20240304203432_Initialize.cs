using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABTestTracker.Migrations
{
    /// <inheritdoc />
    public partial class Initialize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "button_colors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    value = table.Column<string>(type: "varchar(10)", nullable: false),
                    share = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_button_colors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_token = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    value = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    share = table.Column<decimal>(type: "decimal(5,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_prices", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "experiment_button_colors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    button_color_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiment_button_colors", x => x.id);
                    table.ForeignKey(
                        name: "FK_experiment_button_colors_button_colors_button_color_id",
                        column: x => x.button_color_id,
                        principalTable: "button_colors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_experiment_button_colors_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "experiment_prices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    device_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    price_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_experiment_prices", x => x.id);
                    table.ForeignKey(
                        name: "FK_experiment_prices_devices_device_id",
                        column: x => x.device_id,
                        principalTable: "devices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_experiment_prices_prices_price_id",
                        column: x => x.price_id,
                        principalTable: "prices",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_button_colors_value",
                table: "button_colors",
                column: "value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_devices_device_token",
                table: "devices",
                column: "device_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_experiment_button_colors_button_color_id",
                table: "experiment_button_colors",
                column: "button_color_id");

            migrationBuilder.CreateIndex(
                name: "IX_experiment_button_colors_device_id",
                table: "experiment_button_colors",
                column: "device_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_experiment_prices_device_id",
                table: "experiment_prices",
                column: "device_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_experiment_prices_price_id",
                table: "experiment_prices",
                column: "price_id");

            migrationBuilder.CreateIndex(
                name: "IX_prices_value",
                table: "prices",
                column: "value",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "experiment_button_colors");

            migrationBuilder.DropTable(
                name: "experiment_prices");

            migrationBuilder.DropTable(
                name: "button_colors");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "prices");
        }
    }
}
