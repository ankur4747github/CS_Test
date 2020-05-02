using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services.Stock
{
    public interface IRegisterClient
    {
        bool Register(string clientId);

        void PlaceOrder(StockService.PlaceOrderData data);
    }
}
