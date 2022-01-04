using Api.DTO;
using AutoMapper;
using Data.Domain.Entities;

namespace Api.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Container, ContainerDTO>();
            CreateMap<ContainerDTO, Container>();
            CreateMap<Vehicle, VehicleDTO>();
            CreateMap<VehicleDTO, Vehicle>();
        }
    }
}
