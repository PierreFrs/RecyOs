using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecyOs.Helpers;
using RecyOs.OdooImporter.Interfaces;

namespace RecyOs.OdooImporter.ORM.DbContext
{
    public class OdooImporterDbContext : IOdooImporterDbContext
    {
        private readonly IConfiguration _configuration;

        public OdooImporterDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
          
        }

        public DataContext GetContext()
        {
            var options = new DbContextOptionsBuilder<RecyOs.Helpers.DataContext>()
            .UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"))
            .Options;
            return new DataContext(options, _configuration, null);
        }
    }
} 