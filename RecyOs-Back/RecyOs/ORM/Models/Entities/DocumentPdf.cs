// DocumentPdf.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 04/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Entities;

[Table("DocumentPdf")]
public class DocumentPdf : TrackedEntity
{
    [Column("FileSize")] 
    public int FileSize { get; set; }
    
    [Required]
    [Column("FileName")]
    [StringLength(255)]
    public string FileName { get; set; }
    
    [Column("FileLocation")]
    [StringLength(255)]
    public string FileLocation { get; set; }
    
    [Required]
    [ForeignKey("TypeDocumentPdfId")]
    public virtual TypeDocumentPdf TypeDocumentPdf { get; set; }
    
    [Column("TypeDocumentPdfId")]
    public int TypeDocumentPdfId { get; set; }
    
    [ForeignKey("EtablissementClientId")]
    public virtual EtablissementClient EtablissementClient { get; set; }
    
    [Column("EtablissementClientId")]
    public int EtablissementClientId { get; set; }
}