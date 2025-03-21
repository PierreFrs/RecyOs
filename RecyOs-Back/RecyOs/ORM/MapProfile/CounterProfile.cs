using AutoMapper;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.MapProfile;

public class CounterProfile : Profile
{
    public CounterProfile()
    {
        CreateMap<Counter, CounterDto>();
        CreateMap<CounterDto, Counter>();
    }
}