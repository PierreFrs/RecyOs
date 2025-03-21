using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;
using RecyOs.ORM.Filters.hub;
using RecyOs.ORM.Interfaces;
using RecyOs.ORM.Interfaces.hub;

namespace RecyOs.ORM.Service.hub;


public class ClientEuropeService<TClientEurope> : EuropeService<TClientEurope, IClientEuropeRepository<TClientEurope>>,
    IClientEuropeService where TClientEurope : ClientEurope, new()
{
    public ClientEuropeService(
        ICurrentContextProvider contextProvider, 
        IMapper mapper, 
        IClientEuropeRepository<TClientEurope> clientEuropeRepository,
        IGroupRepository groupRepository) : base(contextProvider, clientEuropeRepository, groupRepository, mapper)
    {
    }
}