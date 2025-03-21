using RecyOs.ORM.DTO.hub;

namespace RecyOs.ORM.Models.DTO.hub.BalanceDtos;

public class BalanceFranceDto : BalanceDto
{
    public EtablissementClientDto EtablissementClient { get; set; }
    public SocieteDto Societe { get; set; }
}
