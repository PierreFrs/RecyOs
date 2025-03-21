using AutoMapper;
using RecyOs.ORM.DTO;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.MapProfile;


    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();


        }
    }
