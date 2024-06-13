using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendMecanicaElEnano.Repositories
{
    public class VehiculoRepository:RepositoryBase<Vehiculo>, IVehiculoRepository
    {
        private readonly IMapper _mapper;
        internal MecanicaContext mecanicaContext;
        internal DbSet<Vehiculo> dbSet;
        public VehiculoRepository(MecanicaContext mecanicaContext, IMapper mapper)
        : base(mecanicaContext)
        {
            this._mapper = mapper;
            this.mecanicaContext = mecanicaContext;
            this.dbSet = mecanicaContext.Set <Vehiculo>();
        }

        public new IList<VehiculoDto> FindAll()
        {
            var vehiculos = this.mecanicaContext.Vehiculos!.ToList();
            return _mapper.Map<IList<VehiculoDto>>(vehiculos);
        }

        public async Task<VehiculoDto> FindByIdAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Vehiculo>(id);
            return _mapper.Map<VehiculoDto>(result);
        }

        public async Task<VehiculoDto> CreateAsync(CreateVehiculoDto createVehiculoDto)
        {
            var vehiculo = _mapper.Map<Vehiculo>(createVehiculoDto);
            var result = await this.mecanicaContext.AddAsync(vehiculo);
            await CommitAsync();
            var addedVehiculo = result.Entity;
            return _mapper.Map<VehiculoDto>(addedVehiculo);
        }

        public async Task<VehiculoDto> UpdateAsync(VehiculoDto vehiculoDto)
        {
            var result = await this.mecanicaContext.FindAsync<Vehiculo>(vehiculoDto.VehiculoId);
            if (result != null)
            {
                result.Direccion = vehiculoDto.Direccion;
                result.Cuit = vehiculoDto.Cuit;
                result.Modelo = vehiculoDto.Modelo;
                result.Telefono = vehiculoDto.Telefono;
                result.Cliente = vehiculoDto.Cliente;
                result.Mail = vehiculoDto.Mail;
                result.NumeroChasis = vehiculoDto.NumeroChasis;
                result.Patente = vehiculoDto.Patente;
            }
            await CommitAsync();
            return _mapper.Map<VehiculoDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Vehiculo>(id);
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
