using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMecanicaElEnano.Repositories
{
    public class PresupuestoRepository : RepositoryBase<Presupuesto>, IPresupuestoRepository
    {
        private readonly IMapper _mapper;
        private readonly IVehiculoRepository vehiculoRepository;
        internal MecanicaContext mecanicaContext;
        internal DbSet<Presupuesto> dbSet;
        public PresupuestoRepository(MecanicaContext mecanicaContext, IMapper mapper, IVehiculoRepository vehiculoRepository)
        : base(mecanicaContext)
        {
            this._mapper = mapper;
            this.mecanicaContext = mecanicaContext;
            this.dbSet = mecanicaContext.Set<Presupuesto>();
            this.vehiculoRepository = vehiculoRepository;
        }

        public new IList<PresupuestoDto> FindAll()
        {
            var presupuestos = this.mecanicaContext.Presupuestos!.ToList();
            return _mapper.Map<IList<PresupuestoDto>>(presupuestos);
        }

        public async Task<PresupuestoDto> FindByIdAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Presupuesto>(id);
            return _mapper.Map<PresupuestoDto>(result);
        }

        public async Task<PresupuestoDto> CreateAsync(Guid vehiculoId)
        {
            var presupuesto = new Presupuesto()
            {
                VehiculoId = vehiculoId,
                Fecha = DateTime.Today,
                ValidoHasta = DateTime.Today,
                Km = 0,
                TrabajoARealizar= string.Empty,
            };
            var result = await this.mecanicaContext.AddAsync(presupuesto);
            await CommitAsync();
            var addedPresupuesto = result.Entity;
            return _mapper.Map<PresupuestoDto>(addedPresupuesto);
        }

        public async Task<PresupuestoDto> UpdateAsync(UpdatePresupuestoDto presupuestoDto)
        {
            var result = await this.mecanicaContext.FindAsync<Presupuesto>(presupuestoDto.PresupuestoId);
            if (result != null)
            {
                result.TrabajoARealizar = presupuestoDto.TrabajoARealizar;
                result.Fecha = presupuestoDto.Fecha;
                result.Km = presupuestoDto.Km;
                result.ValidoHasta = presupuestoDto.ValidoHasta;
                // Handle Repuestos
                if(result.Repuestos != null && result.Repuestos.Any())
                {
                    var updatedRepuestos = _mapper.Map<List<Repuesto>>(presupuestoDto.Repuestos);
                    var existingRepuestos = result.Repuestos;

                    // Determine Repuestos to delete
                    var repuestosToDelete = existingRepuestos
                        .Where(existing => !updatedRepuestos.Any(updated => updated.RepuestoId == existing.RepuestoId))
                        .ToList();

                    // Remove the Repuestos to delete from the context
                    foreach (var repuesto in repuestosToDelete)
                    {
                        mecanicaContext.Repuestos.Remove(repuesto);
                    }

                    // Add or update the remaining Repuestos
                    foreach (var updatedRepuesto in updatedRepuestos)
                    {
                        var existingRepuesto = existingRepuestos
                            .FirstOrDefault(r => r.RepuestoId == updatedRepuesto.RepuestoId);

                        if (existingRepuesto != null)
                        {
                            // Update existing Repuesto
                            mecanicaContext.Entry(existingRepuesto).CurrentValues.SetValues(updatedRepuesto);
                        }
                        else
                        {
                            // Add new Repuesto
                            result.Repuestos.Add(updatedRepuesto);
                        }
                    }
                }
                else
                {
                    result.Repuestos = _mapper.Map<List<Repuesto>>(presupuestoDto.Repuestos);
                }
                
            }
            await CommitAsync();
            return _mapper.Map<PresupuestoDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Presupuesto>(id);
            if (result != null)
            {
                this.mecanicaContext.Remove(result);
            }
            await CommitAsync();
        }

        public async Task CommitAsync()
        {
            await mecanicaContext.SaveChangesAsync();
        }
    }
}
