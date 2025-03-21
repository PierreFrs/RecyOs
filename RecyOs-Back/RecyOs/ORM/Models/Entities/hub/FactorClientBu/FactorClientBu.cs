// <copyright file="FactorClientBu.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Entities;

public class FactorClientBu : DeletableEntityWithoutId
{
    [Column("id_client")]
    [Required]
    public int IdClient { get; set; }
    
    [Column("id_bu")]
    [Required]
    public int IdBu { get; set; }
    
    [Column("date_export")]
    public DateTime? ExportDate { get; set; }
}