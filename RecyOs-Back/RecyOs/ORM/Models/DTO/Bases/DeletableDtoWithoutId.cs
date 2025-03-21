// DeletableDtoWithoutId.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 13/08/2024
// Fichier Modifié le : 13/08/2024
// Code développé pour le projet : RecyOs

using System;

namespace RecyOs.ORM.DTO;

public class DeletableDtoWithoutId
{
    public bool IsDeleted { get; set; }
#nullable enable
    public DateTime? CreateDate { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
#nullable disable
}