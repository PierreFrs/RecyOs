using System;
using AutoMapper;
using RecyOs.Engine.Modules.Mkgt;
using RecyOs.MKGT_DB.Entities;

namespace RecyOs.MKGT_DB.MapProfile;

public class FcliProfile : Profile
{
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    public FcliProfile()
    {
        CreateMap<Fcli, EtablissementMkgtDto>()
            .ForMember(dest => dest.code, act => act.MapFrom(src => src.Code))
            .ForMember(dest => dest.nom, act => act.MapFrom(src => src.Nom))
            .ForMember(dest => dest.adr1, act => act.MapFrom(src => src.Adr1))
            .ForMember(dest => dest.adr2, act => act.MapFrom(src => src.Adr2))
            .ForMember(dest => dest.adr3, act => act.MapFrom(src => src.Adr3))
            .ForMember(dest => dest.cp, act => act.MapFrom(src => src.Cp))
            .ForMember(dest => dest.ville, act => act.MapFrom(src => src.Ville))
            .ForMember(dest => dest.pays, act => act.MapFrom(src => src.Pays))
            .ForMember(dest => dest.ape, act => act.MapFrom(src => src.Ape))
            .ForMember(dest => dest.intracom, act => act.MapFrom(src => src.Intrc))
            .ForMember(dest => dest.siret, act => act.MapFrom(src => src.Siret))
            .ForMember(dest => dest.tpSoc, act => act.MapFrom(src => src.Tp_Soc))
            .ForMember(dest => dest.modReg, act => act.MapFrom(src => src.Mode_Reg))
            .ForMember(dest => dest.modRegF, act => act.MapFrom(src => src.Mode_Regf))
            .ForMember(dest => dest.tva, act => act.MapFrom(src => src.Tva))
            .ForMember(dest => dest.tvaF, act => act.MapFrom(src => src.Taux_Tvaf))
            .ForMember(dest => dest.secteur, act => act.MapFrom(src => src.Secteur))
            .ForMember(dest => dest.smTva, act => act.MapFrom(src => src.Sm_Tva))
            .ForMember(dest => dest.codPay, act => act.MapFrom(src => src.Cod_Pay))
            .ForMember(dest => dest.email1, act => act.MapFrom(src => src.Email1))
            .ForMember(dest => dest.email2, act => act.MapFrom(src => src.Email2))
            .ForMember(dest => dest.t2, act => act.MapFrom(src => src.T2))
            .ForMember(dest => dest.t3, act => act.MapFrom(src => src.T3))
            .ForMember(dest => dest.ptb2, act => act.MapFrom(src => src.Ptb2))
            .ForMember(dest => dest.ptb3, act => act.MapFrom(src => src.Ptb3))
            .ForMember(dest => dest.intl2, act => act.MapFrom(src => src.Intl2))
            .ForMember(dest => dest.intl3, act => act.MapFrom(src => src.Intl3))
            .ForMember(dest => dest.encours, act => act.MapFrom(src => src.Encours))
            .ForMember(dest => dest.rib, act => act.MapFrom(src => src.Rib))
            .ForMember(dest => dest.dateCre, act => act.MapFrom(src => src.Dat_Cre))
            .ForMember(dest => dest.dateMdf, act => act.MapFrom(src => src.Dat_Mdf))
            .ForMember(dest => dest.cptFac, act => act.MapFrom(src => src.Cpt_Fac))
            .ForMember(dest => dest.cptAch, act => act.MapFrom(src => src.Cpt_Ach))
            .ForMember( dest => dest.cc, act => act.MapFrom(src => src.Cc));
        
        CreateMap<EtablissementMkgtDto, Fcli>()
            .ForMember(dest => dest.Code, act => act.MapFrom(src => src.code.Length > 13 ? src.code.Substring(0, 13) : src.code))
            .ForMember(dest => dest.Nom, act => act.MapFrom(src => src.nom.Length > 32 ? src.nom.Substring(0, 32) : src.nom))
            .ForMember(dest => dest.Adr1, act => act.MapFrom(src => src.adr1.Length > 32 ? src.adr1.Substring(0, 32) : src.adr1))
            .ForMember(dest => dest.Adr2, act => act.MapFrom(src => src.adr2.Length > 32 ? src.adr2.Substring(0, 32) : src.adr2))
            .ForMember(dest => dest.Adr3, act => act.MapFrom(src => src.adr3.Length > 32 ? src.adr3.Substring(0, 32) : src.adr3))
            .ForMember(dest => dest.Cp, act => act.MapFrom(src => src.cp.Length > 10 ? src.cp.Substring(0, 10) : src.cp))
            .ForMember(dest => dest.Ville, act => act.MapFrom(src => src.ville.Length > 32 ? src.ville.Substring(0, 32) : src.ville))
            .ForMember(dest => dest.Pays, act => act.MapFrom(src => src.pays.Length > 32 ? src.pays.Substring(0, 32) : src.pays))
            .ForMember(dest => dest.Ape, act => act.MapFrom(src => src.ape.Length > 50 ? src.ape.Substring(0, 50) : src.ape))
            .ForMember(dest => dest.Intrc, act => act.MapFrom(src => src.intracom.Length > 14 ? src.intracom.Substring(0, 14) : src.intracom))
            .ForMember(dest => dest.Siret, act => act.MapFrom(src => src.siret.Length > 14 ? src.siret.Substring(0, 14) : src.siret))
            .ForMember(dest => dest.Tp_Soc, act => act.MapFrom(src => src.tpSoc.Length > 13 ? src.tpSoc.Substring(0, 13) : src.tpSoc))
            .ForMember( dest => dest.Mode_Reg, act => act.MapFrom(src => src.modReg.Length > 3 ? src.modReg.Substring(0, 3) : src.modReg))
            .ForMember(dest => dest.Tva, act => act.MapFrom(src => src.tva))
            .ForMember(dest => dest.Secteur, act => act.MapFrom(src => src.secteur.Length> 2 ? src.secteur.Substring(0, 2) : src.secteur))
            .ForMember(dest => dest.Sm_Tva, act => act.MapFrom(src => src.smTva.Length > 10 ? src.smTva.Substring(0, 10) : src.smTva))
            .ForMember(dest => dest.Cod_Pay, act => act.MapFrom(src => src.codPay.Length > 3 ? src.codPay.Substring(0, 3) : src.codPay))
            .ForMember(dest => dest.Email1, act => act.MapFrom(src => src.email1.Length > 70 ? src.email1.Substring(0, 70) : src.email1))
            .ForMember(dest => dest.Email2, act => act.MapFrom(src => src.email2.Length > 70 ? src.email2.Substring(0, 70) : src.email2))
            .ForMember(dest => dest.T2, act => act.MapFrom(src => src.t2.Length > 14 ? src.t2.Replace( "+33 ", "0").Substring(0, 14) : src.t2.Replace( "+33 ", "0")))
            .ForMember(dest => dest.T3, act => act.MapFrom(src =>src.t3.Length > 14 ? src.t3.Replace( "+33 ", "0").Substring(0, 14) : src.t3.Replace( "+33 ", "0")))
            .ForMember(dest => dest.Ptb2, act => act.MapFrom(src => src.ptb2.Length > 14 ? src.ptb2.Replace( "+33 ", "0").Substring(0, 14) : src.ptb2.Replace( "+33 ", "0")))
            .ForMember(dest => dest.Ptb3, act => act.MapFrom(src => src.ptb3.Length > 14 ? src.ptb3.Replace( "+33 ", "0").Substring(0, 14) : src.ptb3.Replace( "+33 ", "0")))
            .ForMember(dest => dest.Intl2, act => act.MapFrom(src => src.intl2.Length > 32 ? src.intl2.Substring(0, 32) : src.intl2))
            .ForMember(dest => dest.Intl3, act => act.MapFrom(src => src.intl3.Length > 32 ? src.intl3.Substring(0, 32) : src.intl3))
            .ForMember(dest => dest.Encours, act => act.MapFrom(src => src.encours))
            .ForMember(dest => dest.Rib, act => act.MapFrom(src => src.rib.Length > 23 ? src.rib.Substring(0, 23) : src.rib))
            .ForMember(dest => dest.Dat_Cre, act => act.MapFrom(src => src.dateCre))
            .ForMember(dest => dest.Dat_Mdf, act => act.MapFrom(src => src.dateMdf))
            .ForMember(dest => dest.Cpt_Fac, act => act.MapFrom(src => src.cptFac.Length > 20 ? src.cptFac.Substring(0, 20) : src.cptFac))
            .ForMember(dest => dest.Cpt_Ach, act => act.MapFrom(src => src.cptAch.Length > 20 ? src.cptAch.Substring(0, 20) : src.cptAch))
            .ForMember(dest => dest.Frn_Cli, act => act.MapFrom(src => src.frnCli))
            .ForMember( dest => dest.Taux_Tvaf, act => act.MapFrom(src => src.tvaF))
            .ForMember( dest => dest.Cc, act => act.MapFrom(src => src.cc))
            .ForMember(dest => dest.Mode_Regf, act => act.MapFrom(src => src.modRegF))
            .ForMember(dest => dest.Fam, act => act.MapFrom(src => src.fam.Length > 2 ? src.fam.Substring(0, 2) : src.fam));
    }
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
}