using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MetroMiles.ApplicationLayer.Extensions.Mappings;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Cars.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Cars.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Cars.Queries.GetList;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Features.Cars.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Car, CreateCarCommand>().ReverseMap();
        CreateMap<Car, CreatedCarResponse>().ReverseMap();

        CreateMap<Car, GetByIdCarResponse>()
            .ForMember(destinationMember: d => d.ModelName, memberOptions: opt => opt.MapFrom((c, _) => c.Model.EnsureLoaded("Car.Model").Name))
            .ForMember(destinationMember: d => d.BrandName, memberOptions: opt => opt.MapFrom((c, _) => c.Model.EnsureLoaded("Car.Model").Brand.EnsureLoaded("Car.Model.Brand").Name))
            .ReverseMap();

        CreateMap<Car, GetListCarListItemDto>()
            .ForMember(destinationMember: d => d.ModelName, memberOptions: opt => opt.MapFrom((c, _) => c.Model.EnsureLoaded("Car.Model").Name))
            .ForMember(destinationMember: d => d.BrandName, memberOptions: opt => opt.MapFrom((c, _) => c.Model.EnsureLoaded("Car.Model").Brand.EnsureLoaded("Car.Model.Brand").Name))
            .ReverseMap();
        CreateMap<Paginate<Car>, GetListResponse<GetListCarListItemDto>>().ReverseMap();

        CreateMap<Car, UpdateCarCommand>().ReverseMap();
        CreateMap<Car, UpdatedCarResponse>().ReverseMap();

        CreateMap<Car, DeleteCarCommand>().ReverseMap();
        CreateMap<Car, DeletedCarResponse>().ReverseMap();

        CreateMap<Car, RestoreCarCommand>().ReverseMap();
        CreateMap<Car, RestoredCarResponse>().ReverseMap();
    }
}
