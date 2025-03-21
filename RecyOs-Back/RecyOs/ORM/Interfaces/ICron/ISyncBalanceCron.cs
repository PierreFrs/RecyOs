// ISyncBalanceCron.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 27/01/2025
// Fichier Modifié le : 27/01/2025
// Code développé pour le projet : RecyOs

using System.Threading.Tasks;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Interfaces.ICron;

public interface ISyncBalanceCron
{
   Task ExecuteAsync();
}