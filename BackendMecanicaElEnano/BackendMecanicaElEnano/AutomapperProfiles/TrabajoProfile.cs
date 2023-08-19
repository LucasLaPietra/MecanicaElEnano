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
            CreateMap<RepuestoTrabajoDto, RepuestoTrabajo>();
            CreateMap<RepuestoTrabajo, RepuestoTrabajoDto>();
            CreateMap<CreateTrabajoDto, Trabajo>();
            CreateMap<CreateRepuestoDto, Repuesto>();
            CreateMap<UpdateTrabajoDto, Trabajo>();
        }
    }
}
