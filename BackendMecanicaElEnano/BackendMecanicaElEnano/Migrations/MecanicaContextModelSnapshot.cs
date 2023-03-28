﻿// <auto-generated />
using System;
using BackendMecanicaElEnano;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BackendMecanicaElEnano.Migrations
{
    [DbContext(typeof(MecanicaContext))]
    partial class MecanicaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Presupuesto", b =>
                {
                    b.Property<Guid>("PresupuestoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("Fecha")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<int?>("Km")
                        .HasColumnType("int");

                    b.Property<string>("TrabajoARealizar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ValidoHasta")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("VehiculoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PresupuestoId");

                    b.HasIndex("VehiculoId");

                    b.ToTable("presupuesto");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Repuesto", b =>
                {
                    b.Property<Guid>("RepuestoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PresupuestoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<float>("precio")
                        .HasColumnType("real");

                    b.HasKey("RepuestoId");

                    b.HasIndex("PresupuestoId");

                    b.ToTable("repuesto");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.RepuestoTrabajo", b =>
                {
                    b.Property<Guid>("RepuestoTrabajoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PresupuestoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<Guid?>("TrabajoId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<float>("precio")
                        .HasColumnType("real");

                    b.HasKey("RepuestoTrabajoId");

                    b.HasIndex("PresupuestoId");

                    b.HasIndex("TrabajoId");

                    b.ToTable("repuestoTrabajo");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Trabajo", b =>
                {
                    b.Property<Guid>("TrabajoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("Km")
                        .HasColumnType("int");

                    b.Property<string>("TrabajosPendientes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TrabajosRealizados")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("VehiculoId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TrabajoId");

                    b.HasIndex("VehiculoId");

                    b.ToTable("trabajo");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Vehiculo", b =>
                {
                    b.Property<Guid>("VehiculoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cuit")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)");

                    b.Property<string>("Direccion")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NumeroChasis")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patente")
                        .IsRequired()
                        .HasMaxLength(7)
                        .HasColumnType("nvarchar(7)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VehiculoId");

                    b.ToTable("vehiculo");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Presupuesto", b =>
                {
                    b.HasOne("BackendMecanicaElEnano.Models.Vehiculo", "Vehiculo")
                        .WithMany("Presupuestos")
                        .HasForeignKey("VehiculoId");

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Repuesto", b =>
                {
                    b.HasOne("BackendMecanicaElEnano.Models.Presupuesto", "Presupuesto")
                        .WithMany("Repuestos")
                        .HasForeignKey("PresupuestoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Presupuesto");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.RepuestoTrabajo", b =>
                {
                    b.HasOne("BackendMecanicaElEnano.Models.Presupuesto", "Presupuesto")
                        .WithMany()
                        .HasForeignKey("PresupuestoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BackendMecanicaElEnano.Models.Trabajo", null)
                        .WithMany("Repuestos")
                        .HasForeignKey("TrabajoId");

                    b.Navigation("Presupuesto");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Trabajo", b =>
                {
                    b.HasOne("BackendMecanicaElEnano.Models.Vehiculo", "Vehiculo")
                        .WithMany()
                        .HasForeignKey("VehiculoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Vehiculo");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Presupuesto", b =>
                {
                    b.Navigation("Repuestos");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Trabajo", b =>
                {
                    b.Navigation("Repuestos");
                });

            modelBuilder.Entity("BackendMecanicaElEnano.Models.Vehiculo", b =>
                {
                    b.Navigation("Presupuestos");
                });
#pragma warning restore 612, 618
        }
    }
}
