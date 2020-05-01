using System.Runtime.Serialization;

namespace Server.Model
{
    [DataContract()]
    public class StockData
    {
        [DataMember]
        public double StockPrice { get; set; }
    }
}