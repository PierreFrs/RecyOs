// ClientParticulierProfile.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

using AutoMapper;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Models.DTO.hub;

namespace RecyOs.ORM.MapProfile;

public class ClientParticulierProfile : Profile
{
    public ClientParticulierProfile ()
    {
        CreateMap<ClientParticulier, ClientParticulierDto>().ReverseMap();
    }
}