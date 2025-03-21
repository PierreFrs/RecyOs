using Microsoft.AspNetCore.Http;
using RecyOs.Helpers;
using RecyOs.ORM.Interfaces.hub;
using RecyOs.ORM.Entities.hub;
namespace RecyOs.ORM.EFCore.Repository.hub;

public class FournisseurEuropeRepository : EuropeRepository,
    IFournisseurEuropeRepository<ClientEurope>
{
    public FournisseurEuropeRepository(DataContext context, IHttpContextAccessor httpContextAccessor) 
        : base(context, httpContextAccessor, "Fournisseur")
    {
    }
}