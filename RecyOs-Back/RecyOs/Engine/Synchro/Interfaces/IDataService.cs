//  IDataService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 17/05/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;

namespace RecyOs.Engine.Interfaces;

public interface IDataService<TDestinationDTO> where TDestinationDTO : class
{
    protected TDestinationDTO AddItem(TDestinationDTO item);
    protected TDestinationDTO UpdateItem(TDestinationDTO item);
    public IList<TDestinationDTO> AddItems(IList<TDestinationDTO> items);
    public IList<TDestinationDTO> UpdateItems(IList<TDestinationDTO> items);
}