//  BaseDataService.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using RecyOs.Engine.Interfaces;
using NLog;

namespace RecyOs.Engine.Services;

public abstract class BaseDataService<TDestinationDto> : IDataService<TDestinationDto> where TDestinationDto : class
{
    public abstract TDestinationDto AddItem(TDestinationDto item);
    public abstract TDestinationDto UpdateItem(TDestinationDto item);

    /*
     * Méthode : AddItems
     *
     * Description :
     * Cette méthode ajoute une liste d'éléments de type TDestinationDto à la base de données en utilisant la méthode AddItem
     * pour chaque élément. Elle retourne un booléen pour indiquer si tous les éléments ont été ajoutés avec succès.
     *
     * Paramètres :
     * - items : IList<TDestinationDto> : Liste d'éléments de type TDestinationDto à ajouter à la base de données
     *
     * Retour : bool
     * - true si tous les éléments ont été ajoutés avec succès à la base de données
     * - false si au moins un élément n'a pas été ajouté avec succès
     *
     * Fonctionnement :
     * 1. Initialise la valeur de retour à true.
     * 2. Parcourt la liste des éléments passés en paramètre.
     * 3. Pour chaque élément, appelle la méthode AddItem avec l'élément en paramètre.
     * 4. Si l'appel de la méthode AddItem retourne false, met à jour la valeur de retour à false.
     * 5. Retourne la valeur de retour après avoir parcouru tous les éléments de la liste.
     */
    public IList<TDestinationDto> AddItems(IList<TDestinationDto> items)
    {
        IList<TDestinationDto> valRetour = new List<TDestinationDto>();
        foreach (TDestinationDto item in items)
        {
            TDestinationDto res = AddItem(item);
            if (res != null)
            {
                valRetour.Add(res);
            }
        }
        return valRetour;
    }

    /*
     * Méthode : UpdateItems
     *
     * Description :
     * Cette méthode met à jour une liste d'éléments de type TDestinationDto dans la base de données en utilisant la méthode
     * UpdateItem pour chaque élément. Elle retourne un booléen pour indiquer si tous les éléments ont été mis à jour avec succès.
     *
     * Paramètres :
     * - items : IList<TDestinationDto> : Liste d'éléments de type TDestinationDto à mettre à jour dans la base de données
     *
     * Retour : bool
     * - true si tous les éléments ont été mis à jour avec succès dans la base de données
     * - false si au moins un élément n'a pas été mis à jour avec succès
     *
     * Fonctionnement :
     * 1. Initialise la valeur de retour à true.
     * 2. Parcourt la liste des éléments passés en paramètre.
     * 3. Pour chaque élément, appelle la méthode UpdateItem avec l'élément en paramètre.
     * 4. Si l'appel de la méthode UpdateItem retourne false, met à jour la valeur de retour à false.
     * 5. Retourne la valeur de retour après avoir parcouru tous les éléments de la liste.
     */
    public IList<TDestinationDto> UpdateItems(IList<TDestinationDto> items)
    {
        IList<TDestinationDto> valRetour = new List<TDestinationDto>();
        foreach (TDestinationDto item in items)
        {
            TDestinationDto res = UpdateItem(item);
            if (res != null)
            {
                valRetour.Add(res);
            }
        }
        return valRetour;
    }
}