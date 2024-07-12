using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMecanicaElEnano.Repositories
{
    public class TrabajoRepository : RepositoryBase<Trabajo>, ITrabajoRepository
    {
        private readonly IMapper _mapper;
        private readonly IVehiculoRepository vehiculoRepository;
        internal MecanicaContext mecanicaContext;
        internal DbSet<Trabajo> dbSet;
        public TrabajoRepository(MecanicaContext mecanicaContext, IMapper mapper, IVehiculoRepository vehiculoRepository)
        : base(mecanicaContext)
        {
            this._mapper = mapper;
            this.mecanicaContext = mecanicaContext;
            this.dbSet = mecanicaContext.Set<Trabajo>();
            this.vehiculoRepository = vehiculoRepository;
        }

        public new IList<TrabajoDto> FindAll()
        {
            var trabajos = this.mecanicaContext.Trabajos!.ToList();
            return _mapper.Map<IList<TrabajoDto>>(trabajos);
        }

        public async Task<TrabajoDto> FindByIdAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Trabajo>(id);
            return _mapper.Map<TrabajoDto>(result);
        }

        public async Task<TrabajoDto> CreateAsync(Guid vehiculoId)
        {
            var trabajo = new Trabajo()
            {
                VehiculoId = vehiculoId,
                Fecha = DateTime.Today,
                Km = 0,
                TrabajosPendientes = string.Empty,
                TrabajosRealizados = string.Empty,
            };
            var result = await this.mecanicaContext.AddAsync(trabajo);
            await CommitAsync();
            var addedTrabajo = result.Entity;
            return _mapper.Map<TrabajoDto>(addedTrabajo);
        }

        public async Task<TrabajoDto> UpdateAsync(UpdateTrabajoDto trabajoDto)
        {
            var result = await this.mecanicaContext.FindAsync<Trabajo>(trabajoDto.TrabajoId);
            if (result != null)
            {
                result.TrabajosPendientes = trabajoDto.TrabajosPendientes;
                result.TrabajosRealizados = trabajoDto.TrabajosRealizados;
                result.Fecha = trabajoDto.Fecha;
                result.Km = trabajoDto.Km;
                // Handle Repuestos
                if (result.Repuestos != null && result.Repuestos.Any())
                {
                    var updatedRepuestos = _mapper.Map<List<RepuestoTrabajo>>(trabajoDto.Repuestos);
                    var existingRepuestos = result.Repuestos;

                    // Determine Repuestos to delete
                    var repuestosToDelete = existingRepuestos
                        .Where(existing => !updatedRepuestos.Any(updated => updated.RepuestoTrabajoId == existing.RepuestoTrabajoId))
                        .ToList();

                    // Remove the Repuestos to delete from the context
                    foreach (var repuesto in repuestosToDelete)
                    {
                        mecanicaContext.RepuestoTrabajos.Remove(repuesto);
                    }

                    // Add or update the remaining Repuestos
                    foreach (var updatedRepuesto in updatedRepuestos)
                    {
                        var existingRepuesto = existingRepuestos
                            .FirstOrDefault(r => r.RepuestoTrabajoId == updatedRepuesto.RepuestoTrabajoId && updatedRepuesto.RepuestoTrabajoId != Guid.Empty);

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
                    result.Repuestos = _mapper.Map<List<RepuestoTrabajo>>(trabajoDto.Repuestos);
                }
            }
            await CommitAsync();
            return _mapper.Map<TrabajoDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Trabajo>(id);
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
