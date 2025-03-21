using System;
using System.Globalization;
using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class EntrepriseCouvertureProfile : Profile
{
    public EntrepriseCouvertureProfile()
    {
        CreateMap<EntrepriseCouverture, EntrepriseCouvertureDto>();
        CreateMap<EntrepriseCouvertureDto, EntrepriseCouverture>();
    }
}