// <copyright file="IfactorClientFranceBuRepository.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.Interfaces;

public interface IFactorClientFranceBuRepository : IBaseFactorClientBuRepository<FactorClientFranceBu>
{
    Task<ServiceResult> UpdateClientIdInFactorClientFranceBuAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session);
}