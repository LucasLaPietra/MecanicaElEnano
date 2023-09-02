using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.AutomapperProfiles
{
    public class OrdenTrabajoProfile : Profile
    {
        public OrdenTrabajoProfile()
        {
            CreateMap<OrdenTrabajoDto, OrdenTrabajo>();
            CreateMap<OrdenTrabajo, OrdenTrabajoDto>();
            CreateMap<UpdateOrdenTrabajoDto, OrdenTrabajo>();
        }
    }
}
