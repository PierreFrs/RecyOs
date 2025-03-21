using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecyOs.Helpers;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.Extensions.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.EFCore.Repository.hub;

public class ClientEuropeRepository : EuropeRepository,
    IClientEuropeRepository<ClientEurope>
{
    public ClientEuropeRepository(DataContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor, "Client")
    {
    }
}