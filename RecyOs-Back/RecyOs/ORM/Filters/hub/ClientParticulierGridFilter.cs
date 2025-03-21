// ClientParticulierGridFilter.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 08/10/2024
// Fichier Modifié le : 08/10/2024
// Code développé pour le projet : RecyOs

namespace RecyOs.ORM.Filters.hub;

public class ClientParticulierGridFilter : BaseFilter
{
    public string FilterByNom { get; set; }
    public string FilterByCodeMkgt { get; set; }
    public string FilterByIdOdoo { get; set; }
    public string BadMail { get; set; }
    public string BadTel { get; set; }
}