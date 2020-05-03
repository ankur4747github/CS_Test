using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract()]
    public class MarketOrderBookData
    {
        [DataMember]
        public List<PlaceOrderData> BuyPendingOrders { get; set; }

        [DataMember]
        public List<PlaceOrderData> SellPendingOrders { get; set; }
    }
}