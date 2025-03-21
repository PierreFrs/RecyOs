using RecyOs.OdooDB.Entities;

namespace RecyOs.OdooDB.Repository;

public interface IResCompanyService
{
    public ResCompanyOdooModel GetResCompany(long id);
    public ResCompanyOdooModel[] GetAllResCompanies();
}