using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract()]
    public class TradeOrderData
    {
        [DataMember]
        public int BuyUserId { get; set; }

        [DataMember]
        public int SellUserId { get; set; }

        [DataMember]
        public int TradeQuantity { get; set; }

        [DataMember]
        public double TradePrice { get; set; }
    }
}