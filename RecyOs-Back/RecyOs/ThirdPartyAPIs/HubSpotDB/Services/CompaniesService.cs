// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CompaniesService.cs
// Created : 2024/04/16 - 14:28
// Updated : 2024/04/16 - 14:28

using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Entities;
using RecyOs.HubSpotDB.Interfaces;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace RecyOs.HubSpotDB.Services;

public class CompaniesService<TCompanies> : ICompaniesService where TCompanies : Companies, new()
{
    private readonly ICompaniesRepository<TCompanies> _companiesRepository;
    private readonly IMapper _mapper;
    
    public CompaniesService(
        ICompaniesRepository<TCompanies> 
        companiesRepository, 
        IMapper mapper)
    {
        _companiesRepository = companiesRepository;
        _mapper = mapper;
    }
    
    /// <summary>
    /// Insère un nouvel établissement client dans la base de données.
    /// </summary>
    /// <param name="company"></param>
    /// <returns></returns>
    public async Task<CompaniesDto> CreateCompany(CompaniesDto company)
    {
        if (company == null) return null;
        
        var etablissementClient = _mapper.Map<TCompanies>(company);
        Companies result = await _companiesRepository.CreateCompany(etablissementClient);
        return _mapper.Map<CompaniesDto>(result);
    }
    
    /// <summary>
    /// Met à jour un enregistrement spécifique de la table Companies dans la base de données HubSpot en utilisant les informations fournies dans l'objet Companies.
    /// </summary>
    /// <param name="company">L'objet Companies contenant les données à mettre à jour.</param>
    /// <returns>
    /// Une tâche représentant l'opération asynchrone. Le résultat de la tâche est l'objet Companies mis à jour.
    /// Si une erreur se produit lors de la mise à jour, le résultat peut être null ou une exception peut être levée.
    /// </returns>
    public async Task<CompaniesDto> UpdateCompany(CompaniesDto company)
    {
        if (company == null) return null;
        
        var etablissementClient = _mapper.Map<TCompanies>(company);
        Companies result = await _companiesRepository.UpdateCompany(etablissementClient);
        return _mapper.Map<CompaniesDto>(result);
    }
    
    
}