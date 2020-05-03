using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    [DataContract()]
    public class MarketOrderBookData
    {
        [DataMember]
        public Dictionary<double, Queue<PlaceOrderData>> BuyPendingOrders { get; set; }

        [DataMember]
        public Dictionary<double, Queue<PlaceOrderData>> SellPendingOrders { get; set; }
    }
}
