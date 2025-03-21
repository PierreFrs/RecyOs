// /** GridData.cs -
//  * ======================================================================0
//  * Crée par : Benjamin
//  * Fichier Crée le : 23/01/2021
//  * Fichier Modifié le : 23/01/2021
//  * Code développé pour le projet : Archimede.Common
//  */
using System.Collections.Generic;

namespace RecyOs.ORM.Entities;

public class GridData<TDto> where TDto : class, new()
{
    public Pagination Paginator { get; set; }
    public IEnumerable<TDto> Items { get; set; }
    public decimal? SommeTotal { get; set; }
}