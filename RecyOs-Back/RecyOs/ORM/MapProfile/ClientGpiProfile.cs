using AutoMapper;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Hub.DTO;

namespace RecyOs.ORM.MapProfile;

public class ClientGpiProfile: Profile
{
    public ClientGpiProfile()
    {
        CreateMap<ClientGpi, ClientGpiDto>();
        CreateMap<ClientGpiDto, ClientGpi>();
    }
}