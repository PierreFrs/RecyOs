// DocumentPdfDto.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 05/09/2023
// Fichier Modifié le : 12/09/2023
// Code développé pour le projet : RecyOs

using System;
using System.IO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.DTO;

public class DocumentPdfEuropeDto : TrackedDto
{
    public int FileSize { get; set; }
    public string FileName { get; set; }
    public string FileLocation { get; set; }
    public int TypeDocumentPdfId { get; set; }
    public int EtablissementClientEuropeID { get; set; }
    public TypeDocumentPdf TypeDocumentPdf { get; set; }
}