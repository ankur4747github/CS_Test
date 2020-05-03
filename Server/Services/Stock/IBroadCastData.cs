using Server.Model;
using System.Collections.Generic;

namespace Server.StockServices
{
    public interface IBroadCastData
    {
        void BroadCastStockPrice(StockData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients);

        void BroadCastTradeData(TradeOrderData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients);

        void BroadCastMarketOrderBookData(MarketOrderBookData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients);

        void BroadCastMarketOrderBookDataToSingleClient(MarketOrderBookData data,
           IReadOnlyDictionary<int, IBroadcastorCallBack> clients, int clientId);
    }
}