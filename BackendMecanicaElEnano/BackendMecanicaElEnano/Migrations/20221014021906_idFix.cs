using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    public partial class idFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoIdVehiculo",
                table: "presupuesto");

            migrationBuilder.DropForeignKey(
                name: "FK_repuesto_presupuesto_PresupuestoIdPresupuesto",
                table: "repuesto");

            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_presupuesto_PresupuestoIdPresupuesto",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoIdPresupuesto",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoIdVehiculo",
                table: "trabajo");

            migrationBuilder.RenameColumn(
                name: "IdVehiculo",
                table: "vehiculo",
                newName: "VehiculoId");

            migrationBuilder.RenameColumn(
                name: "VehiculoIdVehiculo",
                table: "trabajo",
                newName: "VehiculoId");

            migrationBuilder.RenameColumn(
                name: "IdPresupuesto",
                table: "trabajo",
                newName: "TrabajoId");

            migrationBuilder.RenameIndex(
                name: "IX_trabajo_VehiculoIdVehiculo",
                table: "trabajo",
                newName: "IX_trabajo_VehiculoId");

            migrationBuilder.RenameColumn(
                name: "TrabajoIdPresupuesto",
                table: "repuestoTrabajo",
                newName: "TrabajoId");

            migrationBuilder.RenameColumn(
                name: "PresupuestoIdPresupuesto",
                table: "repuestoTrabajo",
                newName: "PresupuestoId");

            migrationBuilder.RenameColumn(
                name: "IdRepuestoTrabajo",
                table: "repuestoTrabajo",
                newName: "RepuestoTrabajoId");

            migrationBuilder.RenameIndex(
                name: "IX_repuestoTrabajo_TrabajoIdPresupuesto",
                table: "repuestoTrabajo",
                newName: "IX_repuestoTrabajo_TrabajoId");

            migrationBuilder.RenameIndex(
                name: "IX_repuestoTrabajo_PresupuestoIdPresupuesto",
                table: "repuestoTrabajo",
                newName: "IX_repuestoTrabajo_PresupuestoId");

            migrationBuilder.RenameColumn(
                name: "PresupuestoIdPresupuesto",
                table: "repuesto",
                newName: "PresupuestoId");

            migrationBuilder.RenameColumn(
                name: "IdRepuesto",
                table: "repuesto",
                newName: "RepuestoId");

            migrationBuilder.RenameIndex(
                name: "IX_repuesto_PresupuestoIdPresupuesto",
                table: "repuesto",
                newName: "IX_repuesto_PresupuestoId");

            migrationBuilder.RenameColumn(
                name: "VehiculoIdVehiculo",
                table: "presupuesto",
                newName: "VehiculoId");

            migrationBuilder.RenameColumn(
                name: "IdPresupuesto",
                table: "presupuesto",
                newName: "PresupuestoId");

            migrationBuilder.RenameIndex(
                name: "IX_presupuesto_VehiculoIdVehiculo",
                table: "presupuesto",
                newName: "IX_presupuesto_VehiculoId");

            migrationBuilder.AddForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoId",
                table: "presupuesto",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_repuesto_presupuesto_PresupuestoId",
                table: "repuesto",
                column: "PresupuestoId",
                principalTable: "presupuesto",
                principalColumn: "PresupuestoId",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoId",
                table: "presupuesto");

            migrationBuilder.DropForeignKey(
                name: "FK_repuesto_presupuesto_PresupuestoId",
                table: "repuesto");

            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_presupuesto_PresupuestoId",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoId",
                table: "repuestoTrabajo");

            migrationBuilder.DropForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoId",
                table: "trabajo");

            migrationBuilder.RenameColumn(
                name: "VehiculoId",
                table: "vehiculo",
                newName: "IdVehiculo");

            migrationBuilder.RenameColumn(
                name: "VehiculoId",
                table: "trabajo",
                newName: "VehiculoIdVehiculo");

            migrationBuilder.RenameColumn(
                name: "TrabajoId",
                table: "trabajo",
                newName: "IdPresupuesto");

            migrationBuilder.RenameIndex(
                name: "IX_trabajo_VehiculoId",
                table: "trabajo",
                newName: "IX_trabajo_VehiculoIdVehiculo");

            migrationBuilder.RenameColumn(
                name: "TrabajoId",
                table: "repuestoTrabajo",
                newName: "TrabajoIdPresupuesto");

            migrationBuilder.RenameColumn(
                name: "PresupuestoId",
                table: "repuestoTrabajo",
                newName: "PresupuestoIdPresupuesto");

            migrationBuilder.RenameColumn(
                name: "RepuestoTrabajoId",
                table: "repuestoTrabajo",
                newName: "IdRepuestoTrabajo");

            migrationBuilder.RenameIndex(
                name: "IX_repuestoTrabajo_TrabajoId",
                table: "repuestoTrabajo",
                newName: "IX_repuestoTrabajo_TrabajoIdPresupuesto");

            migrationBuilder.RenameIndex(
                name: "IX_repuestoTrabajo_PresupuestoId",
                table: "repuestoTrabajo",
                newName: "IX_repuestoTrabajo_PresupuestoIdPresupuesto");

            migrationBuilder.RenameColumn(
                name: "PresupuestoId",
                table: "repuesto",
                newName: "PresupuestoIdPresupuesto");

            migrationBuilder.RenameColumn(
                name: "RepuestoId",
                table: "repuesto",
                newName: "IdRepuesto");

            migrationBuilder.RenameIndex(
                name: "IX_repuesto_PresupuestoId",
                table: "repuesto",
                newName: "IX_repuesto_PresupuestoIdPresupuesto");

            migrationBuilder.RenameColumn(
                name: "VehiculoId",
                table: "presupuesto",
                newName: "VehiculoIdVehiculo");

            migrationBuilder.RenameColumn(
                name: "PresupuestoId",
                table: "presupuesto",
                newName: "IdPresupuesto");

            migrationBuilder.RenameIndex(
                name: "IX_presupuesto_VehiculoId",
                table: "presupuesto",
                newName: "IX_presupuesto_VehiculoIdVehiculo");

            migrationBuilder.AddForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoIdVehiculo",
                table: "presupuesto",
                column: "VehiculoIdVehiculo",
                principalTable: "vehiculo",
                principalColumn: "IdVehiculo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_repuesto_presupuesto_PresupuestoIdPresupuesto",
                table: "repuesto",
                column: "PresupuestoIdPresupuesto",
                principalTable: "presupuesto",
                principalColumn: "IdPresupuesto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_repuestoTrabajo_presupuesto_PresupuestoIdPresupuesto",
                table: "repuestoTrabajo",
                column: "PresupuestoIdPresupuesto",
                principalTable: "presupuesto",
                principalColumn: "IdPresupuesto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_repuestoTrabajo_trabajo_TrabajoIdPresupuesto",
                table: "repuestoTrabajo",
                column: "TrabajoIdPresupuesto",
                principalTable: "trabajo",
                principalColumn: "IdPresupuesto");

            migrationBuilder.AddForeignKey(
                name: "FK_trabajo_vehiculo_VehiculoIdVehiculo",
                table: "trabajo",
                column: "VehiculoIdVehiculo",
                principalTable: "vehiculo",
                principalColumn: "IdVehiculo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
