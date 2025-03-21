// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BalanceEuropeRepository.cs
// Created : 2024/02/26 - 14:44
// Updated : 2024/02/26 - 14:44

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub.BalanceInterfaces;
using RecyOs.ORM.Service;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class BalanceEuropeRepository : BalanceRepository<BalanceEurope>, IBalanceEuropeRepository
{
    private readonly IRoleRepository<Role> _roleRepository;
    private readonly IUserRepository<User> _userRepository;
    private readonly ITokenInfoService _tokenInfoService;
    public BalanceEuropeRepository(
        DataContext context,
        ITokenInfoService tokenInfoService,
        IRoleRepository<Role> roleRepository,
        IUserRepository<User> userRepository
        ) : base(context, tokenInfoService)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _tokenInfoService = tokenInfoService;
    }

    public async Task<(IEnumerable<BalanceEurope>, int, decimal)> GetFilteredListWithCount(BalanceEuropeGridFilter filter, bool includeDeleted = false)
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
            .ApplyFilter<BalanceEurope>(filter)
            .Include(b => b.ClientEurope)
            .Include(b => b.Societe)
            .AsQueryable();

        // Apply Role-Based Filtering
        if (isCommercial)
        {
            query = query.Where(b => b.SocieteId == userSocieteId
                                     && b.ClientEurope.Commercial.UserId == userId);
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
