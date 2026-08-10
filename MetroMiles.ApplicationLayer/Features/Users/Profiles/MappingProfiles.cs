using AutoMapper;
using Core.ApplicationLayer.Responses.GetList;
using Core.PersistenceLayer.Pagings.Paging;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Delete;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Restore;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Update;
using MetroMiles.ApplicationLayer.Features.Users.Queries.GetById;
using MetroMiles.ApplicationLayer.Features.Users.Queries.GetList;
using MetroMiles.DomainLayer.Entities;

namespace MetroMiles.ApplicationLayer.Features.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();

        CreateMap<User, UpdateUserCommand>().ReverseMap();
        CreateMap<User, UpdatedUserResponse>().ReverseMap();

        CreateMap<User, GetByIdUserResponse>().ReverseMap();

        CreateMap<User, GetListUserListItemResponse>().ReverseMap();
        CreateMap<Paginate<User>, GetListResponse<GetListUserListItemResponse>>().ReverseMap();

        CreateMap<User, DeleteUserCommand>().ReverseMap();
        CreateMap<User, DeletedUserResponse>().ReverseMap();

        CreateMap<User, RestoreUserCommand>().ReverseMap();
        CreateMap<User, RestoredUserResponse>().ReverseMap();
    }
}
