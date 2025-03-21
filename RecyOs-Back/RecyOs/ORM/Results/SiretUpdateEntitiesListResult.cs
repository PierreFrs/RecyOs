// <copyright file="SiretUpdateEntitiesListResult.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using RecyOs.MKGT_DB.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Results;

public class SiretUpdateEntitiesListResult
{
    public EtablissementFiche EtablissementFiche { get; set; }
    public EtablissementClient EtablissementClient { get; set; }
    public Fcli Fcli { get; set; }
}