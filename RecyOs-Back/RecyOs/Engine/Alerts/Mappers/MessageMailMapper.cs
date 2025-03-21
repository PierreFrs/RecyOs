/// <summary>
/// Mapper pour les messages de mail.
/// </summary>
/// <author>
/// <name>Benjamin ROLLIN</name>
/// <email>benjamin.rollin@gmail.com</email>
/// <date>2025-01-30</date>
/// </author>
using AutoMapper;
using RecyOs.Engine.Alerts.DTO;
using RecyOs.Engine.Alerts.Entities;
namespace RecyOs.Engine.Alerts.Mappers;

public class MessageMailMapper : Profile
{
    public MessageMailMapper()
    {
        CreateMap<MessageMail, MessageMailDto>().ReverseMap();
    }
} 