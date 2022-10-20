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

        public async Task<PresupuestoDto> CreateAsync(CreatePresupuestoDto createPresupuestoDto)
        {
            var presupuesto = _mapper.Map<Presupuesto>(createPresupuestoDto);
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
                result.Repuestos = _mapper.Map<List<Repuesto>>(presupuestoDto.Repuestos);
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
