using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Services.Stock
{
    public interface IOrder
    {
        void AddOrderIntoQueue(PlaceOrderData data);
        void UpdateMarketOrderBook(int clientId);
    }
}
