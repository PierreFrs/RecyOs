using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;

public class ParameterProfile : Profile
{
    public ParameterProfile()
    {
        CreateMap<Parameter, ParameterDto>()
            .ReverseMap();
    }
    
}