using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.AutomapperProfiles
{
    public class PresupuestoProfile : Profile
    {
        public PresupuestoProfile()
        {
            CreateMap<PresupuestoDto, Presupuesto>();
            CreateMap<Presupuesto, PresupuestoDto>();
            CreateMap<RepuestoDto, Repuesto>();
            CreateMap<Repuesto, RepuestoDto>();
            CreateMap<CreatePresupuestoDto, Presupuesto>();
            CreateMap<CreateRepuestoDto, Repuesto>();
            CreateMap<UpdatePresupuestoDto, Presupuesto>();
        }
    }
}
