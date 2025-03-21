using Microsoft.Extensions.Configuration;
using RecyOs.ThirdPartyAPIs.DashdocDB.Interface;

namespace RecyOs.ThirdPartyAPIs.DashdocDB.Repository;

public class TransportDashdocSettings : ITransportDashdocSettings
{
    private readonly string _uri;
    private readonly string _token;
    
    public TransportDashdocSettings(IConfiguration configuration)
    {
        _token = configuration.GetSection("Dashdoc:Token").Value;
        _uri = configuration.GetSection("Dashdoc:EndPoint").Value;
    }


    public string GetUri()
    {
        return _uri;
    }

    public string GetToken()
    {
        return _token;
    }
}