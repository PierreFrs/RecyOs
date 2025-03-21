// Copyright : Pierre FRAISSE
// RecyOs>RecyOs>FactorClientDto.cs
// Created : 2024/05/2121 - 11:05

using System;

namespace RecyOs.ORM.DTO.hub
{
    public class FactorClientDto
    {
        public int ClientId { get; set; }
        public Guid Refer { get; set; }
        public string Nom { get; set; }
        public string Idcpt { get; set; }
        public string Cpt { get; set; }
        public string Paysres { get; set; }
#nullable enable
        public string? Cptbic { get; set; }
        public string? Cpayscpt { get; set; }
        public string? Siret { get; set; }
        public string? Langue { get; set; }
        public string? Adlib1 { get; set; }
        public string? Adcp { get; set; }
        public string? Adloc { get; set; }
        public string? Adpays { get; set; }
        public string? Cciv { get; set; }
        public string? Cnom { get; set; }
        public string? Cprenom { get; set; }
        public string? Cemail { get; set; }
        public string? Ctel { get; set; }
        public string? Cfax { get; set; }
        public string? Clangue { get; set; }
        public string? Cinfo { get; set; }
        public string? Cptlib { get; set; }
        public string? Flg { get; set; }
        public string? Cl1 { get; set; }
        public string? Cl2 { get; set; }
        public string? Cl3 { get; set; }
        public string? Nombqe { get; set; }
        public string? Adlibbqe { get; set; }
        public string? Adcpbqe { get; set; }
        public string? Advibqe { get; set; }
        public string? Comptemigre { get; set; }
        public string? Forcebic { get; set; }
        public string? Sepamailref1 { get; set; }
        public string? Sepamailref2 { get; set; }
        public string? Typedeb { get; set; }
        public string? Typesfichiers { get; set; }
#nullable disable
    }
}