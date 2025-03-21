using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.Entities.hub;

public class ClientGpi: DeletableEntity
{
    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <remarks>
    /// The code property is used to store a unique identifier for an entity In GPI Database.
    /// It is stored in a database column named "code" and has a maximum length of 8 characters.
    /// The code is required and must be provided when creating or updating an entity.
    /// </remarks>
    [Column("code")]
    [MaxLength(8)]
    [Required]
    public string Code { get; set; }
    
    /// <summary>
    /// Gets or sets the value of the "Type" property.
    /// </summary>
    /// <remarks>
    /// This property is mapped to the "type" column in the database.
    /// The maximum length of the value is 3 characters.
    /// The value is required and cannot be null.
    /// </remarks>
    /// <value>UPD for update, CRE for create</value>
    [Column("type")]
    [MaxLength(3)]
    [Required]
    public string Type { get; set; }

    /// <summary>
    /// Gets or sets the compte.
    /// </summary>
    /// <value>
    /// The compte is the id of customer in Odoo.
    /// </value>
    [Column("compte")]
    [Required]
    public long Compte { get; set; }
    
    [Column("siret")]
    [MaxLength(14)]
    [Required]
    public string Siret { get; set; }
    
    [Column("vat_number")]
    [MaxLength(14)]
    [Required]
    public string Vat { get; set; }
    
    [Column("nom")]
    [MaxLength(255)]
    public string Nom { get; set; }
    
    [Column("adresse_1")] 
    [MaxLength(255)] 
    public string Adresse1 { get; set; }
    
    [Column("code_postal")]
    [MaxLength(10)]
    public string CodePostal { get; set; }
    
    [Column("mode_reglement")]
    public int ModeReglement { get; set; }
    
    [Column("delai_reglement")]
    public int DelaiReglement { get; set; }
    
    /// <summary>
    /// Gets or sets the value of the Collectif property.
    /// </summary>
    /// <remarks>
    /// Cette valeur contien le code du compte de vente 
    /// The value is stored in the database column "collectif".
    /// The maximum length allowed for the value is 8 characters.
    /// </remarks>
    [Column("collectif")]
    [MaxLength(8)]
    public string Collectif { get; set; }
    
    [Column("client_bloque")]
    public bool ClientBloque { get; set; }
    
    [Column("pays")]
    [MaxLength(255)]
    public string Pays { get; set; }
    
#nullable enable        
    [Column("ville")]
    [MaxLength(255)]
    public string? Ville { get; set; }
    
    [Column("departement")]
    [MaxLength(255)]
    public string? Departement { get; set; }
    
    [Column("email")]
    [MaxLength(255)]
    public string? Email { get; set; }
    
    [Column("telephone")]
    [MaxLength(30)]
    public string? Telephone { get; set; }
    
    [Column("portable")]
    [MaxLength(30)]
    public string? Portable { get; set; }
    
    [Column("adresse_2")] 
    [MaxLength(255)] 
    public string? Adresse2 { get; set; }
    
    [Column("adresse_3")] 
    [MaxLength(255)] 
    public string? Adresse3 { get; set; }
    
#nullable disable
}