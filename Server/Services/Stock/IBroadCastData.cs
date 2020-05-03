using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
