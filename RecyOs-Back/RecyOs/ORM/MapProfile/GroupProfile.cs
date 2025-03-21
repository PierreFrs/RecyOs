using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Mapping;

public class GroupProfile : Profile
{
    public GroupProfile()
    {
        CreateMap<Group, GroupDto>().ReverseMap();
    }
} 