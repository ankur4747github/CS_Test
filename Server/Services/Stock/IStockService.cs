using Server.Model;
using System.ServiceModel;

namespace Server.StockServices
{
    [ServiceContract(CallbackContract = typeof(IBroadcastorCallBack))]
    public interface IStockService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterClient(string clientId);
    }

    public interface IBroadcastorCallBack
    {
        [OperationContract(IsOneWay = true)]
        void BroadcastPriceToClient(StockData eventData);
    }
}