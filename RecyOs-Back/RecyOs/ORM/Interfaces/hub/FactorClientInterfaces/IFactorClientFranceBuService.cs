// <copyright file="IFactorClientFranceBuService.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.Interfaces;

public interface IFactorClientFranceBuService : IBaseFactorClientBuService<EtablissementClient, FactorClientFranceBuDto>
{
}