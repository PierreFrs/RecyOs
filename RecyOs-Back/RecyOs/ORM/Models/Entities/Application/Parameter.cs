using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace RecyOs.ORM.Entities
{
    public class Parameter : TrackedEntity
    {
        [Required]
        [Column("nom")]
        [StringLength(100)]
        public string Nom { get; set; }

        // Vous pouvez conserver Valeur sous forme de string, 
        // et effectuer les conversions côté application si besoin.
        [Required]
        [Column("valeur", TypeName = "nvarchar(max)")]
        public string Valeur { get; set; }

        [Required]
        [Column("module")]
        public string Module { get; set; }

        [StringLength(50)]
        [Column("type")]
        [DefaultValue("String")]
        public string Type { get; set; } = "String"; // Exemples : String, Integer, Boolean, etc.

#nullable enable
        // Nouveau champ pour le type de contrôle (correspond à controlType côté front)
        [StringLength(50)]
        [Column("control_type")]
        public string? ControlType { get; set; }

        // Stocker les options sous forme de JSON (si nécessaire)
        [Column("options", TypeName = "nvarchar(max)")]
        public string? OptionsJson { get; set; }

        // Propriété non mappée pour faciliter l'accès aux options
        [NotMapped]
        public List<Option>? Options
        {
            get => string.IsNullOrWhiteSpace(OptionsJson)
                        ? null
                        : JsonSerializer.Deserialize<List<Option>>(OptionsJson);
            set => OptionsJson = JsonSerializer.Serialize(value);
        }

        // Nouveau champ pour le placeholder
        [StringLength(200)]
        [Column("placeholder")]
        public string? Placeholder { get; set; }

        // Nouveau champ pour l'icône de préfixe
        [StringLength(50)]
        [Column("prefix_icon")]
        public string? PrefixIcon { get; set; }

        // Nouveau champ pour l'icône de suffixe
        [StringLength(50)]
        [Column("suffix_icon")]
        public string? SuffixIcon { get; set; }

        // Nouveau champ pour le titre
        [StringLength(200)]
        [Column("label")]
        public string? Label { get; set; }
#nullable disable
    }

    // Classe option pour les selects/chips/etc.
    public class Option
    {
        public string Value { get; set; }
        public string Label { get; set; }
    }
}
