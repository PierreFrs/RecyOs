using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class FuseNavigationItemBadgeProfile : Profile
{
    public FuseNavigationItemBadgeProfile()
    {
        CreateMap<FuseNavigationItemBadge, FuseNavigationItemBadgeDto>();
        CreateMap<FuseNavigationItemBadgeDto, FuseNavigationItemBadge>();
    }
}