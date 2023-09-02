using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    public partial class ordenTrabajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_turno_vehiculo_VehiculoId",
                table: "turno");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehiculoId",
                table: "turno",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "ordenTrabajo",
                columns: table => new
                {
                    OrdenTrabajoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Km = table.Column<int>(type: "int", nullable: false),
                    Manifiesto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mecanico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehiculoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ordenTrabajo", x => x.OrdenTrabajoId);
                    table.ForeignKey(
                        name: "FK_ordenTrabajo_vehiculo_VehiculoId",
                        column: x => x.VehiculoId,
                        principalTable: "vehiculo",
                        principalColumn: "VehiculoId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ordenTrabajo_VehiculoId",
                table: "ordenTrabajo",
                column: "VehiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_turno_vehiculo_VehiculoId",
                table: "turno",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_turno_vehiculo_VehiculoId",
                table: "turno");

            migrationBuilder.DropTable(
                name: "ordenTrabajo");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehiculoId",
                table: "turno",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_turno_vehiculo_VehiculoId",
                table: "turno",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
