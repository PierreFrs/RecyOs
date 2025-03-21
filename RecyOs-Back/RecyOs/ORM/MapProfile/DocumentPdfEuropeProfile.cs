// DocumentPdfProfile.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class DocumentPdfEuropeProfile : Profile
{
    public DocumentPdfEuropeProfile()
    {
        CreateMap<DocumentPdfEurope, DocumentPdfEuropeDto>();
        CreateMap<DocumentPdfEuropeDto, DocumentPdfEurope>();
    }
}