using Server.Model;
using System.ServiceModel;

namespace Server.StockServices
{
    [ServiceContract(CallbackContract = typeof(IBroadcastorCallBack))]
    public interface IStockService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterClient(int clientId);

        [OperationContract(IsOneWay = true)]
        void PlaceOrder(PlaceOrderData data);
    }

    public interface IBroadcastorCallBack
    {
        [OperationContract(IsOneWay = true)]
        void BroadcastPriceToClient(StockData eventData);

        [OperationContract(IsOneWay = true)]
        void BroadcastTradeDataToClient(TradeOrderData eventData);

        [OperationContract(IsOneWay = true)]
        void BroadCastMarketOrderBookData(MarketOrderBookData eventData);
    }
}