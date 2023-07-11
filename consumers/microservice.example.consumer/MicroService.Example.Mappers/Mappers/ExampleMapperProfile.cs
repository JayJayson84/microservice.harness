using MicroService.Contracts;
using MicroService.Example.Domain.Models;
using AutoMapper;

namespace MicroService.Example.Mappers;

public class ExampleMapperProfile : Profile
{
    public ExampleMapperProfile() {
        MapContractToDomainModel();
    }

    private void MapContractToDomainModel() {
        CreateMap<IExampleContract, ExampleModel>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value));
    }
}