using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.DTO;

namespace RecyOs.ORM.DTO.hub;

public class GroupDto : TrackedDto
{
    [Column("name")]
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    public virtual ICollection<ClientEuropeDto> ClientEuropes { get; set; } = new List<ClientEuropeDto>();
    public virtual ICollection<EtablissementClientDto> EtablissementClients { get; set; } = new List<EtablissementClientDto>();
    public int? FicheCount { get; set; }

} 