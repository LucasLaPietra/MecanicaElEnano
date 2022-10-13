using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehiculo",
                columns: table => new
                {
                    IdVehiculo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Patente = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroChasis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Direccion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Mail = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Cuit = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehiculo", x => x.IdVehiculo);
                });

            migrationBuilder.CreateTable(
                name: "presupuesto",
                columns: table => new
                {
                    IdPresupuesto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidoHasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Km = table.Column<int>(type: "int", nullable: false),
                    TrabajoARealizar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehiculoIdVehiculo = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_presupuesto", x => x.IdPresupuesto);
                    table.ForeignKey(
                        name: "FK_presupuesto_vehiculo_VehiculoIdVehiculo",
                        column: x => x.VehiculoIdVehiculo,
                        principalTable: "vehiculo",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trabajo",
                columns: table => new
                {
                    IdPresupuesto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Km = table.Column<int>(type: "int", nullable: false),
                    TrabajosRealizados = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrabajosPendientes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehiculoIdVehiculo = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trabajo", x => x.IdPresupuesto);
                    table.ForeignKey(
                        name: "FK_trabajo_vehiculo_VehiculoIdVehiculo",
                        column: x => x.VehiculoIdVehiculo,
                        principalTable: "vehiculo",
                        principalColumn: "IdVehiculo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "repuesto",
                columns: table => new
                {
                    IdRepuesto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<float>(type: "real", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    PresupuestoIdPresupuesto = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repuesto", x => x.IdRepuesto);
                    table.ForeignKey(
                        name: "FK_repuesto_presupuesto_PresupuestoIdPresupuesto",
                        column: x => x.PresupuestoIdPresupuesto,
                        principalTable: "presupuesto",
                        principalColumn: "IdPresupuesto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "repuestoTrabajo",
                columns: table => new
                {
                    IdRepuestoTrabajo = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    precio = table.Column<float>(type: "real", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    PresupuestoIdPresupuesto = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrabajoIdPresupuesto = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repuestoTrabajo", x => x.IdRepuestoTrabajo);
                    table.ForeignKey(
                        name: "FK_repuestoTrabajo_presupuesto_PresupuestoIdPresupuesto",
                        column: x => x.PresupuestoIdPresupuesto,
                        principalTable: "presupuesto",
                        principalColumn: "IdPresupuesto",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_repuestoTrabajo_trabajo_TrabajoIdPresupuesto",
                        column: x => x.TrabajoIdPresupuesto,
                        principalTable: "trabajo",
                        principalColumn: "IdPresupuesto");
                });

            migrationBuilder.CreateIndex(
                name: "IX_presupuesto_VehiculoIdVehiculo",
                table: "presupuesto",
                column: "VehiculoIdVehiculo");

            migrationBuilder.CreateIndex(
                name: "IX_repuesto_PresupuestoIdPresupuesto",
                table: "repuesto",
                column: "PresupuestoIdPresupuesto");

            migrationBuilder.CreateIndex(
                name: "IX_repuestoTrabajo_PresupuestoIdPresupuesto",
                table: "repuestoTrabajo",
                column: "PresupuestoIdPresupuesto");

            migrationBuilder.CreateIndex(
                name: "IX_repuestoTrabajo_TrabajoIdPresupuesto",
                table: "repuestoTrabajo",
                column: "TrabajoIdPresupuesto");

            migrationBuilder.CreateIndex(
                name: "IX_trabajo_VehiculoIdVehiculo",
                table: "trabajo",
                column: "VehiculoIdVehiculo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "repuesto");

            migrationBuilder.DropTable(
                name: "repuestoTrabajo");

            migrationBuilder.DropTable(
                name: "presupuesto");

            migrationBuilder.DropTable(
                name: "trabajo");

            migrationBuilder.DropTable(
                name: "vehiculo");
        }
    }
}
