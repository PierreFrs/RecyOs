// TemporaryNdCoverExportDto.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 03/03/2025
// Fichier Modifié le : 03/03/2025
// Code développé pour le projet : RecyOs

using RecyOs.ORM.DTO;

namespace RecyOs.ORM.Models.DTO;

public class TemporaryNdCoverExportDto : TrackedDto
{
    public int FileRow { get; set; }
    public string Siren { get; set; }
}