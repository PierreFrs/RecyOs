// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CompaniesProfile.cs
// Created : 2024/04/16 - 14:16
// Updated : 2024/04/16 - 14:16

using AutoMapper;
using RecyOs.HubSpotDB.DTO;
using RecyOs.HubSpotDB.Entities;

namespace RecyOs.HubSpotDB.MapProfile;

public class CompaniesProfile : Profile
{
    public CompaniesProfile()
    {
        CreateMap<CompaniesDto, Companies>();
        CreateMap<Companies, CompaniesDto>();
    }
}