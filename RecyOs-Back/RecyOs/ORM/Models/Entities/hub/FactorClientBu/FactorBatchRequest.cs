// <copyright file="FactorBatchRequest.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System.Collections.Generic;

namespace RecyOs.ORM.Entities;

public class FactorBatchRequest
{
    public int ClientId { get; set; }
    public List<int> BuIds { get; set; }
}