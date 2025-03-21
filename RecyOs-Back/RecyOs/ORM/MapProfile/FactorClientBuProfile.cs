// <copyright file="FactorClientBuProfile.cs" company="p.fraisse@recygroup.local Pierre FRAISSE">
// Copyright (c) p.fraisse@recygroup.local Pierre FRAISSE. All rights reserved.
// </copyright>

using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class FactorClientBuProfile : Profile
{
    public FactorClientBuProfile()
    {
        CreateMap<FactorClientEuropeBu, FactorClientEuropeBuDto>().ReverseMap();
        CreateMap<FactorClientFranceBu, FactorClientFranceBuDto>().ReverseMap();
    }
}