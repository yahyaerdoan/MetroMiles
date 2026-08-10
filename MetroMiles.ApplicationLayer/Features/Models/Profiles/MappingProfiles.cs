using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MetroMiles.ApplicationLayer.Extensions.Mappings;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Models.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetList;
using MetroMiles.ApplicationLayer.Features.Models.Queries.GetListByDynamicQuery;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Features.Models.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Model, CreateModelCommand>().ReverseMap();
        CreateMap<Model, CreatedModelResponse>().ReverseMap();

        CreateMap<Model, UpdateModelCommand>().ReverseMap();
        CreateMap<Model, UpdatedModelResponse>().ReverseMap();

        CreateMap<Model, DeleteModelCommand>().ReverseMap();
        CreateMap<Model, DeletedModelResponse>().ReverseMap();

        CreateMap<Model, RestoreModelCommand>().ReverseMap();
        CreateMap<Model, RestoredModelResponse>().ReverseMap();

        CreateMap<Model, GetByIdModelResponse>()
            .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom((m, _) => m.Brand.EnsureLoaded("Model.Brand").Name))
            .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom((m, _) => m.Fuel.EnsureLoaded("Model.Fuel").Name))
            .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom((m, _) => m.Transmission.EnsureLoaded("Model.Transmission").Name))
            .ReverseMap();

        CreateMap<Model, GetListModelListItemResponse>()
            .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom((m, _) => m.Brand.EnsureLoaded("Model.Brand").Name))
            .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom((m, _) => m.Fuel.EnsureLoaded("Model.Fuel").Name))
            .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom((m, _) => m.Transmission.EnsureLoaded("Model.Transmission").Name))
            .ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListModelListItemResponse>>().ReverseMap();

        CreateMap<Model, GetListByDynamicModelListItemResponse>()
          .ForMember(destinationMember: b => b.BrandName, memberOptions: opt => opt.MapFrom((m, _) => m.Brand.EnsureLoaded("Model.Brand").Name))
          .ForMember(destinationMember: f => f.FuelName, memberOptions: opt => opt.MapFrom((m, _) => m.Fuel.EnsureLoaded("Model.Fuel").Name))
          .ForMember(destinationMember: t => t.TransmissionName, memberOptions: opt => opt.MapFrom((m, _) => m.Transmission.EnsureLoaded("Model.Transmission").Name))
          .ReverseMap();
        CreateMap<Paginate<Model>, GetListResponse<GetListByDynamicModelListItemResponse>>().ReverseMap();
    }
}
