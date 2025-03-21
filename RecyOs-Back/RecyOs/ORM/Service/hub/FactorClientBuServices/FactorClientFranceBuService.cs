// <copyright file="FactorClientFranceBuService.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Threading.Tasks;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;

public class FactorClientFranceBuService : BaseFactorClientBuService<FactorClientFranceBu, FactorClientFranceBuDto>, IFactorClientFranceBuService
{
    public FactorClientFranceBuService(IFactorClientFranceBuRepository repository, IMapper mapper, ITokenInfoService tokenInfoService) : base(repository, mapper, tokenInfoService)
    {
    }
}