using System;
using System.Collections.Generic;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;

namespace RecyOs.ORM.DTO.hub;

public class ClientEuropeDto: IClientBalanceDto
{
        public int Id { get; set; }
        public string Vat { get; set; }
        #nullable enable
        public string? Nom { get; set; }
        public string? IdOdoo { get; set; }
        public string? CodeKerlog { get; set; }
        public string? CodeMkgt { get; set; }
        public string? CodeGpi { get; set; }
        public string? FrnCodeGpi { get; set; }
        public string? ContactFacturation { get; set; }
        public string? EmailFacturation { get; set; }
        public string? TelephoneFacturation { get; set; }
        public string? PortableFacturation { get; set; }
        public string? ContactAlternatif { get; set; }
        public string? EmailAlternatif { get; set; }
        public string? TelephoneAlternatif { get; set; }
        public string? PortableAlternatif { get; set; }
        public string? AdresseFacturation1 { get; set; }
        public string? AdresseFacturation2 { get; set; }
        public string? AdresseFacturation3 { get; set; }
        public string? CodePostalFacturation { get; set; }
        public string? VilleFacturation { get; set; }
        public string? PaysFacturation { get; set; }
        public int? ConditionReglement { get; set; }
        public int? ModeReglement { get; set; }
        public int? DelaiReglement { get; set; }
        public decimal? TauxTva { get; set; }
        public int? EncoursMax { get; set; }
        public string? CompteComptable { get; set; }
        public int? FrnConditionReglement { get; set; }
        public int? FrnModeReglement { get; set; }
        public int? FrnDelaiReglement { get; set; }
        public decimal? FrnTauxTva { get; set; }
        public int? FrnEncoursMax { get; set; }
        public string? FrnCompteComptable { get; set; }
        public string? Iban { get; set; }
        public string? Bic { get; set; }
        public bool? ClientBloque { get; set; }
        public string? MotifBlocage { get; set; }
        public DateTime? DateBlocage { get; set; }
        public bool? Radie { get; set; }
        public int? CategorieId { get; set; }
        public int? CommercialId { get; set; }
        public CommercialDto? Commercial { get; set; }
        public string? IdHubspot { get; set; }
        public int? GroupId { get; set; }
        public bool? NoBalance { get; set; }
        public int? IdDashdoc { get; set; }
        public int? IdShipperDashdoc { get; set; }
        public DateTime? DateCreDashdoc { get; set; }
#nullable disable
        public string CreateDate { get; set; }
        public string UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string DateCreMkgt { get; set; }
        public string DateCreOdoo { get; set; }
        public string DateCreGpi { get; set; }

        public bool Client { get; set; }
        public bool Fournisseur { get; set; }
        public ICollection<FactorClientEuropeBuDto> FactorClientEuropeBus { get; set; }
        public bool IsDeleted { get; set; }
}