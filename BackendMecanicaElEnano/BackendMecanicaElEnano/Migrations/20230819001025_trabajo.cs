using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    public partial class trabajo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_presupuesto_PresupuestoId",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoId",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoId",
                table: "trabajo");

            migrationBuilder.DropIndex(
                name: "IX_repuestoTrabajo_PresupuestoId",
                table: "repuestoTrabajo");

            migrationBuilder.DropColumn(
                name: "PresupuestoId",
                table: "repuestoTrabajo");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehiculoId",
                table: "trabajo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "TrabajoId",
                table: "repuestoTrabajo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoId",
                table: "repuestoTrabajo",
                column: "TrabajoId",
                principalTable: "trabajo",
                principalColumn: "TrabajoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoId",
                table: "trabajo",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoId",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoId",
                table: "trabajo");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehiculoId",
                table: "trabajo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TrabajoId",
                table: "repuestoTrabajo",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PresupuestoId",
                table: "repuestoTrabajo",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_repuestoTrabajo_PresupuestoId",
                table: "repuestoTrabajo",
                column: "PresupuestoId");

            migrationBuilder.AddForeignKey(
                name: "FK_repuestoTrabajo_presupuesto_PresupuestoId",
                table: "repuestoTrabajo",
                column: "PresupuestoId",
                principalTable: "presupuesto",
                principalColumn: "PresupuestoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoId",
                table: "repuestoTrabajo",
                column: "TrabajoId",
                principalTable: "trabajo",
                principalColumn: "TrabajoId");

            migrationBuilder.AddForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoId",
                table: "trabajo",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
