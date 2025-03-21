using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Repository;

public class TransportDashdocRepository : DashdocRepository, ITransportDashdocRepository
{
    public TransportDashdocRepository(IConfiguration configuration,
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        ITransportDashdocSettings dashdocSettings) : base(configuration, httpClient, httpContextAccessor,
        dashdocSettings)
    {
        
    }
}