using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Fuels.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Fuels.Queries.GetList;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Features.Fuels.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Fuel, CreateFuelCommand>().ReverseMap();
        CreateMap<Fuel, CreatedFuelResponse>().ReverseMap();

        CreateMap<Fuel, GetByIdFuelResponse>().ReverseMap();

        CreateMap<Fuel, GetListFuelListItemResponse>().ReverseMap();
        CreateMap<Paginate<Fuel>, GetListResponse<GetListFuelListItemResponse>>().ReverseMap();

        CreateMap<Fuel, UpdateFuelCommand>().ReverseMap();
        CreateMap<Fuel, UpdatedFuelResponse>().ReverseMap();

        CreateMap<Fuel, DeleteFuelCommand>().ReverseMap();
        CreateMap<Fuel, DeletedFuelResponse>().ReverseMap();

        CreateMap<Fuel, RestoreFuelCommand>().ReverseMap();
        CreateMap<Fuel, RestoredFuelResponse>().ReverseMap();
    }
}
