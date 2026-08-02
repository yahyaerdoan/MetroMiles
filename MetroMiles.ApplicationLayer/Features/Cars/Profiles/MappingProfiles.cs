using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
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
            .ForMember(destinationMember: d => d.ModelName, memberOptions: opt => opt.MapFrom((c, _) => c.Model != null ? c.Model.Name : throw new InvalidOperationException("Car.Model was not loaded — the query must Include(Model).")))
            .ForMember(destinationMember: d => d.BrandName, memberOptions: opt => opt.MapFrom((c, _) => c.Model != null && c.Model.Brand != null ? c.Model.Brand.Name : throw new InvalidOperationException("Car.Model.Brand was not loaded — the query must Include(Model).ThenInclude(Brand).")))
            .ReverseMap();

        CreateMap<Car, GetListCarListItemDto>()
            .ForMember(destinationMember: d => d.ModelName, memberOptions: opt => opt.MapFrom((c, _) => c.Model != null ? c.Model.Name : throw new InvalidOperationException("Car.Model was not loaded — the query must Include(Model).")))
            .ForMember(destinationMember: d => d.BrandName, memberOptions: opt => opt.MapFrom((c, _) => c.Model != null && c.Model.Brand != null ? c.Model.Brand.Name : throw new InvalidOperationException("Car.Model.Brand was not loaded — the query must Include(Model).ThenInclude(Brand).")))
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
