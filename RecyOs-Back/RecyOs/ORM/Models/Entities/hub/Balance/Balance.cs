// Created by : Pierre FRAISSE
// RecyOs => RecyOs => Balance.cs
// Created : 2024/02/26 - 14:19
// Updated : 2024/02/26 - 14:19

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecyOs.ORM.Entities.hub;

public class Balance : TrackedEntity
{
    [Column("date_recuperation_balance")]
    [Required]
    public DateTime DateRecuperationBalance { get; set; }
    
    [Column("montant")]
    [Required]
    public Decimal Montant { get; set; }
}