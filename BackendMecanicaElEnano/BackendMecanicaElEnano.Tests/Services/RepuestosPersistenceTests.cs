using AutoMapper;
using BackendMecanicaElEnano.AutomapperProfiles;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using BackendMecanicaElEnano.Repositories;
using BackendMecanicaElEnano.Services;
using BackendMecanicaElEnano.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace BackendMecanicaElEnano.Tests.Services;

public class RepuestosPersistenceTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task UpdatePersistsAddedRemovedAndEditedParts(bool isTrabajo)
    {
        using var context = new MecanicaContext(new DbContextOptionsBuilder<MecanicaContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
        var mapper = new MapperConfiguration(c => {
            c.AddProfile<PresupuestoProfile>();
            c.AddProfile<TrabajoProfile>();
        }).CreateMapper();
        var unit = new Mock<IUnitOfWork>();
        unit.SetupGet(u => u.Presupuestos).Returns(new PresupuestoRepository(context));
        unit.SetupGet(u => u.Trabajos).Returns(new TrabajoRepository(context));
        unit.Setup(u => u.CommitAsync()).Returns(() => context.SaveChangesAsync());
        var id = Guid.NewGuid();
        var keptId = Guid.NewGuid();
        var removedId = Guid.NewGuid();
        if (isTrabajo)
        {
            context.Trabajos!.Add(new Trabajo { TrabajoId = id, Fecha = DateTime.Today,
                TrabajosRealizados = "Service", TrabajosPendientes = "",
                Repuestos = new List<RepuestoTrabajo> {
                    new() { RepuestoTrabajoId = keptId, Descripcion = "Filtro", Cantidad = 1 },
                    new() { RepuestoTrabajoId = removedId, Descripcion = "Quitar", Cantidad = 1 }
                }});
        }
        else
        {
            context.Presupuestos!.Add(new Presupuesto { PresupuestoId = id, Fecha = DateTime.Today,
                Repuestos = new List<Repuesto> {
                    new() { RepuestoId = keptId, Descripcion = "Filtro", Cantidad = 1 },
                    new() { RepuestoId = removedId, Descripcion = "Quitar", Cantidad = 1 }
                }});
        }
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
        if (isTrabajo)
        {
            var service = new TrabajoService(unit.Object, mapper, NullLogger<TrabajoService>.Instance, context);
            var dto = new UpdateTrabajoDto { TrabajoId = id, Fecha = DateTime.Today,
                TrabajosRealizados = "Service", TrabajosPendientes = "",
                Repuestos = new List<RepuestoTrabajoDto> {
                    new() { RepuestoId = keptId, Descripcion = "Filtro editado", Cantidad = 2 },
                    new() { Descripcion = "Nuevo", Cantidad = 1 }
                }};
            Assert.True((await service.UpdateAsync(dto)).IsSuccess);
            context.ChangeTracker.Clear();
            var parts = await context.RepuestoTrabajos!.ToListAsync();
            Assert.Equal(2, parts.Count);
            Assert.Contains(parts, p => p.RepuestoTrabajoId == keptId && p.Cantidad == 2);
            Assert.Contains(parts, p => p.Descripcion == "Nuevo" && p.RepuestoTrabajoId != Guid.Empty);
            dto.Repuestos.Clear();
            Assert.True((await service.UpdateAsync(dto)).IsSuccess);
            context.ChangeTracker.Clear();
            Assert.Empty(await context.RepuestoTrabajos.ToListAsync());
        }
        else
        {
            var service = new PresupuestoService(unit.Object, mapper, NullLogger<PresupuestoService>.Instance);
            var dto = new UpdatePresupuestoDto { PresupuestoId = id, Fecha = DateTime.Today,
                ValidoHasta = DateTime.Today, Repuestos = new List<RepuestoDto> {
                    new() { RepuestoId = keptId, Descripcion = "Filtro editado", Cantidad = 2 },
                    new() { Descripcion = "Nuevo", Cantidad = 1 }
                }};
            Assert.True((await service.UpdateAsync(dto)).IsSuccess);
            context.ChangeTracker.Clear();
            var parts = await context.Repuestos!.ToListAsync();
            Assert.Equal(2, parts.Count);
            Assert.Contains(parts, p => p.RepuestoId == keptId && p.Cantidad == 2);
            Assert.Contains(parts, p => p.Descripcion == "Nuevo" && p.RepuestoId != Guid.Empty);
            dto.Repuestos.Clear();
            Assert.True((await service.UpdateAsync(dto)).IsSuccess);
            context.ChangeTracker.Clear();
            Assert.Empty(await context.Repuestos.ToListAsync());
        }
    }
}
