// <copyright file="FactorClientFranceBu.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Entities;

public class FactorClientFranceBu : FactorClientBu
{
    [ForeignKey(nameof(IdClient))]
    [InverseProperty(nameof(Entities.hub.EtablissementClient.FactorClientFranceBus))]
    public virtual EtablissementClient EtablissementClient { get; set; }
    
    [ForeignKey(nameof(IdBu))]
    [InverseProperty(nameof(Entities.hub.BusinessUnit.FactorClientFranceBus))]
    public virtual BusinessUnit BusinessUnit { get; set; }
}