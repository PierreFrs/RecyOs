// Created by : Pierre FRAISSE
// RecyOs => RecyOs => ClientCompositeDto.cs
// Created : 2024/03/27 - 10:24
// Updated : 2024/03/27 - 10:24

using System.Collections.Generic;
using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.DTO;

public class ClientCompositeDto
{
    public List<EtablissementClientDto> EtablissementClients { get; set; } = new List<EtablissementClientDto>();
    public List<ClientEuropeDto> ClientEuropes { get; set; } = new List<ClientEuropeDto>();
}