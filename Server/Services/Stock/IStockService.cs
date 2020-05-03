using Server.Model;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Server.StockServices
{
    [ServiceContract(CallbackContract = typeof(IBroadcastorCallBack))]
    public interface IStockService
    {
        [OperationContract(IsOneWay = true)]
        void RegisterClient(int clientId);

        [OperationContract]
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped,
            UriTemplate = "json/{id}")]
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