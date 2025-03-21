// CategorieClientService.cs -
// ======================================================================0
// Crée par : Pierre FRAISSE
// Fichier Crée le : 08/11/2023
// Fichier Modifié le : 08/11/2023
// Code développé pour le projet : RecyOs

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.SqlClient.DataClassification;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Interfaces;

namespace RecyOs.ORM.Service;

public class CategorieClientService<TCategorieClient, TCategorieClientDto> : BaseService, ICategorieClientService<TCategorieClientDto>
    where TCategorieClient : CategorieClient, new()
    where TCategorieClientDto : CategorieClientDto, new()
{
    private readonly ICategorieClientRepository<TCategorieClient> _categorieClientRepository;
    private readonly IMapper _mapper;
    private readonly ITokenInfoService _tokenInfoService;

    public CategorieClientService(ICurrentContextProvider contextProvider,
        ICategorieClientRepository<TCategorieClient> categorieClientRepository,
        IMapper mapper,
        ITokenInfoService tokenInfoService) : base(contextProvider)
    {
        _categorieClientRepository = categorieClientRepository;
        _mapper = mapper;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<TCategorieClientDto> CreateCategoryAsync(string label)
    {
        var currentUser = _tokenInfoService.GetCurrentUserName();
        
        var categorieClient = new TCategorieClient()
        {
            CategorieLabel = label,
            CreateDate = DateTime.Now,
            CreatedBy = currentUser,
        };
        var createdCategory  = await _categorieClientRepository.CreateCategoryAsync(categorieClient);
        var categorieClientDto = _mapper.Map<TCategorieClientDto>(createdCategory);
        return categorieClientDto;
    }

    public async Task<List<TCategorieClientDto>> GetListAsync()
    {
        var categories = await _categorieClientRepository.GetListAsync(Session);
        return _mapper.Map<List<TCategorieClientDto>>(categories);
    }

    public async Task<TCategorieClientDto> GetByIdAsync(int id)
    {
        var category = await _categorieClientRepository.GetByIdAsync(id, Session);
        return _mapper.Map<TCategorieClientDto>(category);
    }

    public async Task<TCategorieClientDto> UpdateCategorieClientAsync(int id, string label)
    {
        var currentUser = _tokenInfoService.GetCurrentUserName();

        var existingCategorieClient = await _categorieClientRepository.GetByIdAsync(id, Session);
        if (existingCategorieClient == null)
        {
            return null;
        }
        
        existingCategorieClient.CategorieLabel = label;
        existingCategorieClient.UpdatedAt = DateTime.Now;
        existingCategorieClient.UpdatedBy = currentUser;

        var updatedCategoryEntity = await _categorieClientRepository.UpdateAsync(existingCategorieClient, Session);
        var updatedCategoryDto = _mapper.Map<TCategorieClientDto>(updatedCategoryEntity);

        return updatedCategoryDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _categorieClientRepository.GetByIdAsync(id, Session);
        if (category == null)
        {
            return false;
        }

        await _categorieClientRepository.DeleteAsync(id, Session);
        return true;
    }
}