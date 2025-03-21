// <copyright file="FactorClientEuropeBuRepository.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.EFCore.Repository.hub.FactorClientBuRepositories;

public class FactorClientEuropeBuRepository : BaseFactorClientBuRepository<FactorClientEuropeBu>,IFactorClientEuropeBuRepository
{
    public FactorClientEuropeBuRepository(DataContext context, ITokenInfoService tokenInfoService) : base(context, tokenInfoService)
    {
    }
}