using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.AutomapperProfiles
{
    public class TurnoProfile : Profile
    {
        public TurnoProfile()
        {
            CreateMap<TurnoDto, Turno>();
            CreateMap<Turno, TurnoDto>();
            CreateMap<CreateTurnoDto, Turno>();
            CreateMap<UpdateTurnoDto, Turno>();
        }
    }
}
