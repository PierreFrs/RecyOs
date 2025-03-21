/** EntrepriseBaseGridFilter.cs - Définition du modèle article
 * ======================================================================0
 * Crée par : Benjamin
 * Fichier Crée le : 20/03/2023
 * Fichier Modifié le : 20/03/2023
 * Code développé pour le projet : RecOS.EntrepriseBaseGridFilter
 */


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RecyOs.ORM.Filters.hub;

public class EntrepriseBaseGridFilter: BaseFilter
{ 
 public string FiltredBySiren { get; set; }
}