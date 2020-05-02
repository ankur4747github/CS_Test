using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract()]
    public class PlaceOrderData
    {
        [DataMember]
        public int ClientId { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public bool IsBuy { get; set; }
    }
}