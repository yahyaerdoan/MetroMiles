using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;
using MetroMiles.DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroMiles.ApplicationLayer.Features.Models.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Model, GetListModelListItemDto>()
            .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom(b => b.Brand.Name))
            .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom(f => f.Fuel.Name))
            .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom(t => t.Transmission.Name))
            .ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemDto>>().ReverseMap();

        CreateMap<Model, GetListByDynamicModelListItemDto>()
          .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom(b => b.Brand.Name))
          .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom(f => f.Fuel.Name))
          .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom(t => t.Transmission.Name))
          .ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemDto>>().ReverseMap();
    }
}
