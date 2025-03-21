//  ICommandImportCouverture.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace RecyOs.Controllers;

public interface ICommandImportCouverture
{
    Task<bool> Import(IFormFile file);
    Task<bool> CheckFormat(IFormFile file);
}