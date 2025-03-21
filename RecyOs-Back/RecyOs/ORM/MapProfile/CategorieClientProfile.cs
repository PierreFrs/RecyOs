// CategorieClientProfile.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class CategorieClientProfile : Profile
{
    public CategorieClientProfile()
    {
        CreateMap<CategorieClient, CategorieClientDto>();
        CreateMap<CategorieClientDto, CategorieClient>();
    }
}