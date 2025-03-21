// /** IrPropertyProfile.cs -
//  * ======================================================================
//  * Crée par : Benjamin
//  * Fichier Crée le : 16/07/2023
//  * Fichier Modifié le : 16/07/2023
//  * Code développé pour le projet : RecyOs
//  */
using AutoMapper;
using RecyOs.OdooDB.DTO;
using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Mappers;

public class IrPropertyProfile : Profile
{
    public IrPropertyProfile()
    {
        CreateMap<IrPropertyOdooModel, IrPropertyDto>();
        CreateMap<IrPropertyDto, IrPropertyOdooModel>();
    }
}