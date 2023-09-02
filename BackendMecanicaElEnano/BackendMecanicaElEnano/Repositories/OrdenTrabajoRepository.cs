using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMecanicaElEnano.Repositories
{
    public class OrdenTrabajoRepository : RepositoryBase<OrdenTrabajo>, IOrdenTrabajoRepository
    {
        private readonly IMapper _mapper;
        private readonly IVehiculoRepository vehiculoRepository;
        internal MecanicaContext mecanicaContext;
        internal DbSet<OrdenTrabajo> dbSet;
        public OrdenTrabajoRepository(MecanicaContext mecanicaContext, IMapper mapper, IVehiculoRepository vehiculoRepository)
        : base(mecanicaContext)
        {
            this._mapper = mapper;
            this.mecanicaContext = mecanicaContext;
            this.dbSet = mecanicaContext.Set<OrdenTrabajo>();
            this.vehiculoRepository = vehiculoRepository;
        }

        public new IList<OrdenTrabajoDto> FindAll()
        {
            var ordenTrabajos = this.mecanicaContext.OrdenTrabajos!.ToList();
            return _mapper.Map<IList<OrdenTrabajoDto>>(ordenTrabajos);
        }

        public async Task<OrdenTrabajoDto> FindByIdAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<OrdenTrabajo>(id);
            return _mapper.Map<OrdenTrabajoDto>(result);
        }

        public async Task<OrdenTrabajoDto> CreateAsync(Guid vehiculoId)
        {
            var ordenTrabajo = new OrdenTrabajo()
            {
                VehiculoId = vehiculoId,
                Fecha = DateTime.Today,
                Km = 0,
                Manifiesto = string.Empty,
                Mecanico = string.Empty,
            };
            var result = await this.mecanicaContext.AddAsync(ordenTrabajo);
            await CommitAsync();
            var addedOrdenTrabajo = result.Entity;
            return _mapper.Map<OrdenTrabajoDto>(addedOrdenTrabajo);
        }

        public async Task<OrdenTrabajoDto> UpdateAsync(UpdateOrdenTrabajoDto ordenTrabajoDto)
        {
            var result = await this.mecanicaContext.FindAsync<OrdenTrabajo>(ordenTrabajoDto.OrdenTrabajoId);
            if (result != null)
            {
                result.Manifiesto = ordenTrabajoDto.Manifiesto;
                result.Mecanico = ordenTrabajoDto.Mecanico;
                result.Fecha = ordenTrabajoDto.Fecha;
                result.Km = ordenTrabajoDto.Km;
            }
            await CommitAsync();
            return _mapper.Map<OrdenTrabajoDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<OrdenTrabajo>(id);
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
