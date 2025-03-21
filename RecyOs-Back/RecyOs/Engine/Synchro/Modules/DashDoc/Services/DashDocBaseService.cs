using RecyOs.Engine.Services;

namespace RecyOs.Engine.Modules.DashDoc.Services;

public abstract class DashDocBaseService<TDestination> : BaseDataService<TDestination> where TDestination : class
{
    protected DashDocBaseService() : base()
    {
        
    }
}