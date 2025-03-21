// Created by : Pierre FRAISSE
// RecyOs => RecyOs => CommercialProfile.cs
// Created : 2024/03/26 - 14:49
// Updated : 2024/03/26 - 14:49

using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class CommercialProfile : Profile
{
    public CommercialProfile()
    {
        CreateMap<Commercial, CommercialDto>().ReverseMap();
    }
}