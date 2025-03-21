using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleDto>()
            .ReverseMap();
    }
}