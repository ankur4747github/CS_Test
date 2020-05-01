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
            IReadOnlyDictionary<string, IBroadcastorCallBack> clients);
    }
}
