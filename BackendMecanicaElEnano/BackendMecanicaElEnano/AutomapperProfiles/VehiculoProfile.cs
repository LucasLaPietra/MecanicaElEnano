using AutoMapper;
using BackendMecanicaElEnano.Dto;
using BackendMecanicaElEnano.Models;

namespace BackendMecanicaElEnano.AutomapperProfiles
{
    public class VehiculoProfile : Profile
    {

        public VehiculoProfile()
        {
            CreateMap<VehiculoDto, Vehiculo>();
            CreateMap<Vehiculo, VehiculoDto>();
            CreateMap<CreateVehiculoDto, Vehiculo>();
        }
    }
}
