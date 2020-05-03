using Server.Model;
using System.Collections.Generic;

namespace Server.Services.Stock
{
    public interface IOrder
    {
        void AddOrderIntoQueue(PlaceOrderData data);

        void UpdateMarketOrderBook(int clientId);
        List<TradeOrderData> GetTradeListOrderData();

        MarketOrderBookData GetOrderData();
    }
}