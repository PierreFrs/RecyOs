// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceDto.cs
// Created : 2024/02/26 - 14:24
// Updated : 2024/02/26 - 14:24

using System;

namespace RecyOs.ORM.DTO.hub;

public class BalanceDto : TrackedDto
{
    public int ClientId { get; set; }
    public int SocieteId { get; set; }
    public DateTime DateRecuperationBalance { get; set; }
    public Decimal Montant { get; set; }
}