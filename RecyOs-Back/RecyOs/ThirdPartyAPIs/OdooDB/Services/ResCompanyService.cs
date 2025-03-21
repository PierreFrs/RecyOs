using System.Collections.Generic;
using System.Linq;
using NLog;
using RecyOs.OdooDB.Entities;
using RecyOs.OdooDB.Interfaces;
using RecyOs.OdooDB.Repository;

namespace RecyOs.OdooDB.Services;

public class ResCompanyService<TResCompany> :IResCompanyService where TResCompany : ResCompanyOdooModel, new()
{
   private readonly IList<ResCompanyOdooModel> _resCompanies;
   private readonly IResCompanyRepository<TResCompany> _resCompanyRepository;
   private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
   
   public ResCompanyService(IResCompanyRepository<TResCompany> resCompanyRepository)
   {
       _logger.Trace("Mise en cache des sociétés");
       _resCompanyRepository = resCompanyRepository;
       _resCompanies = _resCompanyRepository.GetAllCompanies().Result;
       _logger.Trace("Fin de mise en cache des sociétés");
   }
   
   public ResCompanyOdooModel GetResCompany(long id)
   {
       return _resCompanies.FirstOrDefault(x => x.Id == id);
   }
   
   public ResCompanyOdooModel[] GetAllResCompanies()
   {
       return _resCompanies.ToArray();
   }
   
}