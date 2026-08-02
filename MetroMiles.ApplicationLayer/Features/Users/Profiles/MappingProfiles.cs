using AutoMapper;
using Core.SecurityLayer.Entities;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Create;
using MetroMiles.ApplicationLayer.Features.Users.Commands.Update;

namespace MetroMiles.ApplicationLayer.Features.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<User, CreateUserCommand>().ReverseMap();
        CreateMap<User, CreatedUserResponse>().ReverseMap();

        CreateMap<User, UpdateUserCommand>().ReverseMap();
        CreateMap<User, UpdatedUserResponse>().ReverseMap();
    }
}
