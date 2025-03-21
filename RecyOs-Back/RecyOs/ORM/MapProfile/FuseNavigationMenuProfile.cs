using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class FuseNavigationMenuProfile : Profile
{
    public FuseNavigationMenuProfile()
    {
        CreateMap<FuseNavigationItem, FuseNavigationItemDto>();
        CreateMap<FuseNavigationItemDto, FuseNavigationItem>();
    }
}