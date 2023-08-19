﻿using AutoMapper;
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
                result.Repuestos = _mapper.Map<List<RepuestoTrabajo>>(trabajoDto.Repuestos);
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
