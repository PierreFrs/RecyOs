using System.Threading.Tasks;
using RecyOs.ORM.DTO.dashboard;

namespace RecyOs.ORM.Interfaces.dashboard;

public interface IDashboardCustomerService
{
    Task<DashboardCustomerDto> GetDashboard();

}