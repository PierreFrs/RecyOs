// <copyright file="FactorClientBuDto.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;

namespace RecyOs.ORM.DTO.hub;

public class FactorClientBuDto : DeletableDtoWithoutId
{
    public int IdClient { get; set; }
    
    public int IdBu { get; set; }
    
    
    public DateTime? ExportDate { get; set; }
}