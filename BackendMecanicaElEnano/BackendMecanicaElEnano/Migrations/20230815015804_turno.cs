using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    public partial class turno : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "turno",
                columns: table => new
                {
                    TurnoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechayHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_turno", x => x.TurnoId);
                    table.ForeignKey(
                        name: "FK_turno_vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculo",
                        principalColumn: "VehiculoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_turno_VehiculoId",
                table: "turno",
                column: "VehiculoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "turno");
        }
    }
}
