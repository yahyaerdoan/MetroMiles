using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Features.Models.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Model, GetByIdModelResponse>()
            .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom((m, _) => m.Brand != null ? m.Brand.Name : throw new InvalidOperationException("Model.Brand was not loaded — the query must Include(Brand).")))
            .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom((m, _) => m.Fuel != null ? m.Fuel.Name : throw new InvalidOperationException("Model.Fuel was not loaded — the query must Include(Fuel).")))
            .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom((m, _) => m.Transmission != null ? m.Transmission.Name : throw new InvalidOperationException("Model.Transmission was not loaded — the query must Include(Transmission).")))
            .ReverseMap();

        CreateMap<Model, GetListModelListItemDto>()
            .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom((m, _) => m.Brand != null ? m.Brand.Name : throw new InvalidOperationException("Model.Brand was not loaded — the query must Include(Brand).")))
            .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom((m, _) => m.Fuel != null ? m.Fuel.Name : throw new InvalidOperationException("Model.Fuel was not loaded — the query must Include(Fuel).")))
            .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom((m, _) => m.Transmission != null ? m.Transmission.Name : throw new InvalidOperationException("Model.Transmission was not loaded — the query must Include(Transmission).")))
            .ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListModelListItemDto>>().ReverseMap();

        CreateMap<Model, GetListByDynamicModelListItemDto>()
          .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom((m, _) => m.Brand != null ? m.Brand.Name : throw new InvalidOperationException("Model.Brand was not loaded — the query must Include(Brand).")))
          .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom((m, _) => m.Fuel != null ? m.Fuel.Name : throw new InvalidOperationException("Model.Fuel was not loaded — the query must Include(Fuel).")))
          .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom((m, _) => m.Transmission != null ? m.Transmission.Name : throw new InvalidOperationException("Model.Transmission was not loaded — the query must Include(Transmission).")))
          .ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemDto>>().ReverseMap();
    }
}
