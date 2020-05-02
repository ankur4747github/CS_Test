using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model
{
    [DataContract()]
    public class PlaceOrderData
    {
        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public bool IsBuy { get; set; }

        [DataMember]
        public bool IsSell { get; set; }
    }
}
