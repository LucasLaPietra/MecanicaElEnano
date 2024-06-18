using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.AutomapperProfiles
{
    public class TrabajoProfile : Profile
    {
        public TrabajoProfile()
        {
            CreateMap<TrabajoDto, Trabajo>();
            CreateMap<Trabajo, TrabajoDto>();
            CreateMap<RepuestoTrabajoDto, RepuestoTrabajo>().ForMember(r => r.RepuestoTrabajoId, opt => opt.MapFrom(src => src.RepuestoId));
            CreateMap<RepuestoTrabajo, RepuestoTrabajoDto>().ForMember(r => r.RepuestoId, opt => opt.MapFrom(src => src.RepuestoTrabajoId));
            CreateMap<CreateTrabajoDto, Trabajo>();
            CreateMap<CreateRepuestoDto, Repuesto>();
            CreateMap<UpdateTrabajoDto, Trabajo>();
        }
    }
}
