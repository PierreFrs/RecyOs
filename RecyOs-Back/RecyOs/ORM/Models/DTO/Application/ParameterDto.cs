using System;
using System.Collections.Generic;

namespace RecyOs.ORM.DTO
{
    public class ParameterDto
    {
        public int Id { get; set; }
        public string Module { get; set; } 
        public string Nom { get; set; }
        public string Valeur { get; set; }
        public string Type { get; set; }

#nullable enable        
        // Nouveau champ pour préciser le type de contrôle (ex : text, number, etc.)
        public string? ControlType { get; set; }
        
        // Nouvelle propriété pour les options, à utiliser notamment pour les selects ou les chips.
        public List<OptionDto>? Options { get; set; }
        
        // Nouveau champ pour le placeholder
        public string? Placeholder { get; set; }
        
        // Nouveau champ pour l'icône préfixe
        public string? PrefixIcon { get; set; }
        
        // Nouveau champ pour l'icône suffixe
        public string? SuffixIcon { get; set; }

        // Nouveau champ pour le titre
        public string? Label { get; set; }
#nullable disable
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OptionDto
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
