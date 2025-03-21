// Created by : Pierre FRAISSE
// RecyOs => RecyOs => SocieteDto.cs
// Created : 2024/02/23 - 15:14
// Updated : 2024/02/23 - 15:14

using System.Collections.Generic;

namespace RecyOs.ORM.DTO.hub;

public class SocieteDto : TrackedDto
{
    public string Nom { get; set; }
    public string IdOdoo { get; set; }
    public ICollection<CommercialDto> Commercials { get; set; }
}