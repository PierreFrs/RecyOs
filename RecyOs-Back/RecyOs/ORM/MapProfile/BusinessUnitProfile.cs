// Created by : Pierre FRAISSE
// RecyOs => RecyOs => BusinessUnitProfile.cs
// Created : 2024/01/19 - 10:55
// Updated : 2024/01/19 - 10:55

using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class BusinessUnitProfile : Profile
{
    public BusinessUnitProfile()
    {
        CreateMap<BusinessUnit, BusinessUnitDto>();
        CreateMap<BusinessUnitDto, BusinessUnit>();
    }
}