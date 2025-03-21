using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class EngineSyncStatusProfile : Profile
{
    public EngineSyncStatusProfile()
    {
        CreateMap<EngineSyncStatus, EngineSyncStatusDto>();
        CreateMap<EngineSyncStatusDto, EngineSyncStatus>();
    }
}