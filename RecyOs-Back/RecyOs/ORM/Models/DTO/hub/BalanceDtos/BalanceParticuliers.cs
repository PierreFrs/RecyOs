using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Models.DTO.hub.BalanceDtos;

public class BalanceParticuliersDto : BalanceDto
{
    public ClientParticulierDto ClientParticulier { get; set; }
    public SocieteDto Societe { get; set; }
}
