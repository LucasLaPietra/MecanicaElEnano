using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    public partial class nullablecolpresupuesto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoId",
                table: "presupuesto");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehiculoId",
                table: "presupuesto",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidoHasta",
                table: "presupuesto",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "TrabajoARealizar",
                table: "presupuesto",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Km",
                table: "presupuesto",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoId",
                table: "presupuesto",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoId",
                table: "presupuesto");

            migrationBuilder.AlterColumn<Guid>(
                name: "VehiculoId",
                table: "presupuesto",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidoHasta",
                table: "presupuesto",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TrabajoARealizar",
                table: "presupuesto",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Km",
                table: "presupuesto",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_presupuesto_vehiculo_VehiculoId",
                table: "presupuesto",
                column: "VehiculoId",
                principalTable: "vehiculo",
                principalColumn: "VehiculoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
