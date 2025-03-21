using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Models.DTO.hub.BalanceDtos;

public class BalanceEuropeDto : BalanceDto
{
    public ClientEuropeDto ClientEurope { get; set; }
    public SocieteDto Societe { get; set; }
}