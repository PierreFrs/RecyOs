//  EtablissementMkgtDto.cs -
// ======================================================================0
// Crée par : Benjamin
// Fichier Crée le : 05/08/2023
// Fichier Modifié le : 08/04/2024
// Code développé pour le projet : RecyOs

namespace RecyOs.Engine.Modules.Mkgt;

public class EtablissementMkgtDto
{
    public string code { get; set; }
    public string nom { get; set; }
    public string adr1 { get; set; }
    public string adr2 { get; set; }
    public string adr3 { get; set; }
    public string cp { get; set; }
    public string ville { get; set; }
    public string pays { get; set; }
    public string ape { get; set; }
    public string intracom { get; set; }
    public string siret { get; set; }
    public string tpSoc { get; set; }  // Forme juridique
    public string modReg { get; set; }
    public string modRegF { get; set; }
    public decimal tva { get; set; }
    public decimal tvaF { get; set; }
    public string secteur { get; set; }
    public string smTva { get; set; }
    public string codPay { get; set; }
#nullable enable
    public string? email1 { get; set; }
    public string? email2 { get; set; }
    public string? t2 { get; set; }
    public string? t3 { get; set; }
    public string? ptb2 { get; set; }
    public string? ptb3 { get; set; }
    public string? intl2 { get; set; }
    public string? intl3 { get; set; }
    public decimal? encours { get; set; }
    public string? rib { get; set; }
    public string? cptFac { get; set; }
    public string? cptAch { get; set; }
    public string? dateCre { get; set; }
    public string? dateMdf { get; set; }
    public string? frnCli { get; set; }
    public string? cc { get; set; }
    public string? fam { get; set; }
#nullable disable
}