using System.Collections.Generic;
using RecyOs.ORM.Entities;

namespace RecyOs.ORM.DTO;

public class NavigationDto
{
    public IEnumerable<FuseNavigationItemDto> Compact { get; set; }
    public IEnumerable<FuseNavigationItemDto> Default { get; set; }
    public IEnumerable<FuseNavigationItemDto> Futuristic { get; set; }
    public IEnumerable<FuseNavigationItemDto> Horizontal { get; set; }
}