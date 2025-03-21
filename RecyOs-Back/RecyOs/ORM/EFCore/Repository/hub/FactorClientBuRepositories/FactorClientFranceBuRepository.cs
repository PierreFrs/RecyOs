// <copyright file="FactorClientFranceBuRepository.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.EntityFrameworkCore;
using RecyOs.Exceptions;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Results;

namespace RecyOs.ORM.EFCore.Repository.hub.FactorClientBuRepositories;

public class FactorClientFranceBuRepository : BaseFactorClientBuRepository<FactorClientFranceBu>,IFactorClientFranceBuRepository
{
    private readonly ITokenInfoService _tokenInfoService;
    public FactorClientFranceBuRepository(
        DataContext context,
        ITokenInfoService tokenInfoService
        ) 
        : base(context, tokenInfoService)
    {
        _tokenInfoService = tokenInfoService;
    }

    public async Task<ServiceResult> UpdateClientIdInFactorClientFranceBuAsync(int oldEtablissementClientId, int newEtablissementId, ContextSession session)
    {
        try
        {
            // Retrieve the list of FactorClientFranceBu entities associated with the old client ID
            var factorClientFranceBuList = await GetByClientIdAsync(session, oldEtablissementClientId);

            // Mark the old entities as deleted
            foreach (var factorClientFranceBu in factorClientFranceBuList)
            {
                await DeleteAsync(factorClientFranceBu.IdClient, factorClientFranceBu.IdBu, session);
            }

            // Create and add new entities
            foreach (var factorClientFranceBu in factorClientFranceBuList)
            {
                var newEntity = new FactorClientFranceBu
                {
                    IdClient = newEtablissementId,
                    IdBu = factorClientFranceBu.IdBu,
                    ExportDate = factorClientFranceBu.ExportDate,
                    CreatedBy = factorClientFranceBu.CreatedBy,
                    CreateDate = factorClientFranceBu.CreateDate,
                    UpdatedBy = _tokenInfoService.GetCurrentUserName(),
                    UpdatedAt = DateTime.Now
                };
                await CreateAsync(newEntity, session);
            }

            // Verify the changes
            var verificationList = await GetByClientIdAsync(session, newEtablissementId);

            if (verificationList.Count != factorClientFranceBuList.Count)
            {
                return new ServiceResult
                {
                    Success = false,
                    StatusCode = 400,
                    Message = "Une erreur est survenue lors du transfert des données d'affacturage"
                };
            }
            else
            {
                return new ServiceResult
                {
                    Success = true,
                    StatusCode = 200,
                    Message = "Les ids ont été mis à jour"
                };
            }
        }
        catch (RepositoryException ex)
        {
            // Handle repository-specific exceptions
            throw new ServiceException("An error occurred while updating the batch in the repository layer.", ex);
        }
        catch (Exception ex)
        {
            // Handle any other unexpected exceptions
            throw new ServiceException("An unexpected error occurred while updating the batch.", ex);
        }
    }
}