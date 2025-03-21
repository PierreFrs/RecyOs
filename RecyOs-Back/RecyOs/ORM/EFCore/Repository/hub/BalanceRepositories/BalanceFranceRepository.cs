// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceFranceRepository.cs
// Created : 2024/02/26 - 12:27
// Updated : 2024/02/26 - 12:27

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using Xunit.Sdk;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class BalanceFranceRepository : BalanceRepository<BalanceFrance>, IBalanceFranceRepository
{
    private readonly IRoleRepository<Role> _roleRepository;
    private readonly IUserRepository<User> _userRepository;
    private readonly ITokenInfoService _tokenInfoService;
    public BalanceFranceRepository(
        DataContext context,
        IRoleRepository<Role> roleRepository,
        IUserRepository<User> userRepository,
        ITokenInfoService tokenInfoService) : base(context, tokenInfoService)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<(IEnumerable<BalanceFrance>, int, decimal)> GetFilteredListWithCount(
        BalanceFranceGridFilter filter,
        bool includeDeleted = false)
    {
        var userId = _tokenInfoService.GetCurrentUserId();
        var roles = await _roleRepository.GetListByUserId(userId, new ContextSession());
        var user = await _userRepository.Get(userId, new ContextSession());
        int? userSocieteId = user.SocieteId;

        var isCommercial = roles.Any(r => r.Name == "commercial");
        var isResponsableBu = roles.Any(r => r.Name == "responsable_bu");
        var isCompta = roles.Any(r => r.Name == "compta");

        // Base Query
        var query = _dbSet
            .ApplyFilter<BalanceFrance>(filter)
            .Include(b => b.EtablissementClient)
            .Include(b => b.Societe)
            .AsQueryable();

        // Apply Role-Based Filtering
        if (isCommercial)
        {
            query = query.Where(b => b.SocieteId == userSocieteId
                                     && b.EtablissementClient.ClientGroupe == false
                                     && b.EtablissementClient.Commercial.UserId == userId);
        }
        else if (isResponsableBu)
        {
            query = query.Where(b => b.SocieteId == userSocieteId);
        }
        else if (isCompta)
        {
            query = query.Where(b => b.SocieteId != 1);
        }
        else
        {
            throw new UnauthorizedAccessException("You are not authorized to access this resource");
        }

        // Fetch Count & Sum in a Single Query
        var totalCount = await query.CountAsync();
        var totalMontant = await query.SumAsync(b => b.Montant);

        // Fetch Paginated Results
        var paginatedResult = await query
            .Skip(filter.PageSize * (filter.PageNumber - 1))
            .Take(filter.PageSize)
            .ToArrayAsync();

        return (paginatedResult, totalCount, totalMontant);
    }

}