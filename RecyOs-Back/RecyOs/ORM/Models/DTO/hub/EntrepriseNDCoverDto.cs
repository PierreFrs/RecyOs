using System;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.DTO.hub;

public class EntrepriseNDCoverDto
{
    public int Id { get; set; }
    public string CoverId { get; set; }
    public string NumeroContratPrimaire { get; set; }
    public string NumeroContratExtension { get; set; }
    public string NomPolice { get; set; }
    public string EhId { get; set; }
    public string RaisonSociale { get; set; }
    public string FormeJuridiqueCode { get; set; }
    public string SecteurActivite { get; set; }
    public string TypeIdentifiant { get; set; }
    public string Siren { get; set; }
    public string StatutEntreprise { get; set; }
    public string Statut { get; set; }
    public string TempsReponse { get; set; }
    public DateTime DateChangementPosition { get; set; }
    public DateTime DateDemande { get; set; }
    public string PeriodeRenouvellementOuverte { get; set; }
    public string NomRue { get; set; }
    public int CodePostal { get; set; }
    public string Ville { get; set; }
    public string Pays { get; set; }
    public DateTime DateExtraction { get; set; } 
    
#nullable enable
    public string? ReferenceClient { get; set; }
    public DateTime? DateSuppression { get; set; }
    public string? SeraRenouvele { get; set; }
    public DateTime? DateRenouvellementPrevue { get; set; }
    public DateTime? DateExpiration { get; set; }
#nullable disable
}