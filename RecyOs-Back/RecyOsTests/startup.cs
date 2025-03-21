using Microsoft.Extensions.DependencyInjection;
using RecyOsTests.Interfaces;
using RecyOsTests.Services;
using RecyOsTests.TestFixtures;

namespace RecyOsTests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDataContextTests, DataContextTests>();
            services.AddSingleton<DocumentPdfServiceFixture>();
            services.AddSingleton<IEngineDataContextTests, EngineDataContextTests>();
        }
    }
}