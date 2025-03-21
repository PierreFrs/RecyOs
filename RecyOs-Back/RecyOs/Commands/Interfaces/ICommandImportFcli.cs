//  ICommandImportFcli.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 12/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;

namespace RecyOs.Controllers;

public interface ICommandImportFcli
{
    /// <summary>
    /// Importe et normalise une liste d'établissements clients valides depuis le service _fCliService.
    /// </summary>
    /// <returns>
    /// Retourne true une fois que tous les établissements clients ont été traités et importés.
    /// </returns>
    /// <remarks>
    /// Le processus d'importation suit les étapes suivantes :
    /// 1. Obtention de la liste des établissements clients valides depuis le service _fCliService.
    /// 2. Pour chaque établissement client :
    ///    a. Si le SIRET n'est pas vide ou null, l'établissement est normalisé.
    ///    b. Si le SIRET normalisé n'est pas vide ou null, l'établissement normalisé est mappé en EtablissementClientExDto.
    ///    c. Le nouvel établissement mappé est créé dans le service _etablissementClientService.
    /// Les actions et les résultats sont enregistrés à l'aide d'un système de logging.
    /// Note : La méthode est asynchrone pour gérer efficacement les opérations IO et éviter le blocage du thread principal.
    /// </remarks>
    Task<bool> Import();
    
    /// <summary>
    /// Importe et normalise un établissement par son code si l'établissement est valide depuis le service _fCliService.
    /// </summary>
    /// <returns>
    /// Retourne true une fois que l'établissement client a été traité et importé.
    /// </returns>
    /// <remarks>
    /// Le processus d'importation suit les étapes suivantes :
    /// 1. Obtention de l'établissement client valide depuis le service _fCliService.
    /// 2. Si le SIRET n'est pas vide ou null, l'établissement est normalisé.
    /// 3. Si le SIRET normalisé n'est pas vide ou null, l'établissement normalisé est mappé en EtablissementClientExDto.
    /// 4. Le nouvel établissement mappé est créé dans le service _etablissementClientService.
    ///  Les actions et les résultats sont enregistrés à l'aide d'un système de logging.
    ///  Note : La méthode est asynchrone pour gérer efficacement les opérations IO et éviter le blocage du thread principal. 
    /// </remarks>
    Task<bool> Import(string code);
}