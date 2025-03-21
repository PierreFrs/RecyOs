using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Repository;

public class ShipperDashdocRepository : DashdocRepository, IShipperDashdocRepository
{
    public ShipperDashdocRepository(IConfiguration configuration,
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        IShipperDashdocSettings dashdocSettings) : base(configuration, httpClient, httpContextAccessor,
        dashdocSettings)
    {
        
    }
}