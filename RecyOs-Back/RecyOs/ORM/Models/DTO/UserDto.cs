using System.Collections.Generic;
using RecyOs.ORM.DTO.hub;
using RecyOs.ORM.Entities;
using RecyOs.ORM.Entities.hub;

namespace RecyOs.ORM.DTO;

public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Status { get; set; }
    #nullable enable
    public CommercialDto? Commercial { get; set; }
    public int SocieteId { get; set; }
    public SocieteDto? SocieteDto { get; set; }
    #nullable disable
    public ICollection<RoleDto> Roles { get; set; }
}