/// <summary>
/// Classe permettant de créer un contexte de données
/// </summary>
/// <author>Benjamin ROLLIN</author>
/// <date>2025-01-30</date>
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecyOs.Engine.Interfaces;
using RecyOs.Helpers;

namespace RecyOs.Engine;

public class DataContextEngine : IDataContextEngine
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;
    private DataContext _context;
    
    public DataContextEngine(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
    }
    
    /// <summary>
    /// Crée un contexte de données et le retourne à l'appelant
    /// </summary>
    /// <returns></returns>
    public DataContext GetContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseSqlServer(_configuration.GetConnectionString("WebApiDatabase"))
            .Options;
        _context = new DataContext(options, _configuration, _env);

        return _context;
    }
}