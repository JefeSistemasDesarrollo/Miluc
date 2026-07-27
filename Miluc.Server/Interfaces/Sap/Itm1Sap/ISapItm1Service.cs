using Miluc.Server.Models.Sap;

namespace Miluc.Server.Interfaces.Sap.Itm1Sap
{
    public interface ISapItm1Service
    {
        Task<List<ITM1>> GetListItm1Async();

    }
}
