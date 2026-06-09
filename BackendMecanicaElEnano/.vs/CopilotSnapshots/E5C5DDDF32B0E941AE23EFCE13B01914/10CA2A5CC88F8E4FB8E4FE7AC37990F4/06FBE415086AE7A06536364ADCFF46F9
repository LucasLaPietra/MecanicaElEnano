using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Migrations;
using BackendMecanicaElEnano.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace BackendMecanicaElEnano.Repositories
{
    public class TurnoRepository : RepositoryBase<Turno>, ITurnoRepository
    {
        private readonly IMapper _mapper;
        internal MecanicaContext mecanicaContext;
        internal DbSet<Turno> dbSet;
        public TurnoRepository(MecanicaContext mecanicaContext, IMapper mapper)
        : base(mecanicaContext)
        {
            this._mapper = mapper;
            this.mecanicaContext = mecanicaContext;
            this.dbSet = mecanicaContext.Set<Turno>();
        }

        public new IList<TurnoDto> FindAll()
        {
            var Turnos = this.mecanicaContext.Turnos!.ToList();
            return _mapper.Map<IList<TurnoDto>>(Turnos);
        }

        public async Task<TurnoDto> FindByIdAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Turno>(id);
            return _mapper.Map<TurnoDto>(result);
        }

        public async Task<TurnoDto> CreateAsync(CreateTurnoDto createTurnoDto)
        {
            var turno = _mapper.Map<Turno>(createTurnoDto);
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            turno.FechayHora = TimeZoneInfo.ConvertTimeFromUtc(turno.FechayHora, timeZone);
            var result = await this.mecanicaContext.AddAsync(turno);
            await CommitAsync();
            var addedTurno = result.Entity;
            return _mapper.Map<TurnoDto>(addedTurno);
        }

        public async Task<TurnoDto> UpdateAsync(TurnoDto TurnoDto)
        {
            var result = await this.mecanicaContext.FindAsync<Turno>(TurnoDto.TurnoId);
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time");
            if (result != null)
            {
                result.FechayHora = TimeZoneInfo.ConvertTimeFromUtc(TurnoDto.FechayHora, timeZone);
                result.Detalle = TurnoDto.Detalle;
            }
            await CommitAsync();
            return _mapper.Map<TurnoDto>(result);
        }

        public async Task DeleteAsync(Guid id)
        {
            var result = await this.mecanicaContext.FindAsync<Turno>(id);
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
