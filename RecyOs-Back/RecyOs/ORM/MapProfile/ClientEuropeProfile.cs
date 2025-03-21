using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class ClientEuropeProfile: Profile
{
    public ClientEuropeProfile()
    {
        CreateMap<ClientEurope, ClientEuropeDto>();
        CreateMap<ClientEuropeDto, ClientEurope>();
    }
}