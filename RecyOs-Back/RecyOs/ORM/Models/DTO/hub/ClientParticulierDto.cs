// ClientParticulierDto.cs -
// ======================================================================0
// Crée par : pfraisse
// Fichier Crée le : 07/10/2024
// Fichier Modifié le : 07/10/2024
// Code développé pour le projet : RecyOs

using System;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.Models.DTO.hub;

public class ClientParticulierDto : BaseDto, IClientBalanceDto
{
#nullable enable
    public string? Nom { get; set; }
    public string? ClientName { get; set; }
    public string? Prenom { get; set; }
    public string? Titre { get; set; }
    public string? AdresseFacturation1 { get; set; }
    public string? AdresseFacturation2 { get; set; }
    public string? AdresseFacturation3 { get; set; }
    public string? CodePostalFacturation { get; set; }
    public string? VilleFacturation { get; set; }
    public string? PaysFacturation { get; set; }
    public string? EmailFacturation { get; set; }
    public string? TelephoneFacturation { get; set; }
    public string? PortableFacturation { get; set; }
    public string? ContactAlternatif { get; set; }
    public string? EmailAlternatif { get; set; }
    public string? TelephoneAlternatif { get; set; }
    public string? PortableAlternatif { get; set; }
    public int? ConditionReglement { get; set; }
    public int? ModeReglement { get; set; }
    public int? DelaiReglement { get; set; }
    public decimal? TauxTva { get; set; }
    public int? EncoursMax { get; set; }
    public string? CompteComptable { get; set; }
    public bool? ClientBloque { get; set; }
    public string? MotifBlocage { get; set; }
    public DateTime? DateBlocage { get; set; }
    public string? CodeMkgt { get; set; }
    public DateTime? DateCreMkgt { get; set; }
    public string? IdOdoo { get; set; }
    public DateTime? DateCreOdoo { get; set; }
    public string? CreateDate { get; set; }
    public string? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public int? IdShipperDashdoc { get; set; }
    public bool? NoBalance { get; set; }

#nullable disable

    public ClientParticulierDto()
    {
        this.ConditionReglement = 1;
        this.ModeReglement = 4;
        this.DelaiReglement = 0;
        this.TauxTva = 20;
        this.EncoursMax = 0;
        this.CompteComptable = "411003";
        this.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.ClientBloque = false;
    }
}