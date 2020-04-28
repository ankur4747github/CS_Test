using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TCPBinding;

namespace Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class StockService : IStockService
    {
        public void PostStockDetail(Stock stock)
        {
            Console.ForegroundColor = (ConsoleColor)new Random().Next(10);
            Console.WriteLine("-> Received = " + stock.Price);
        }
    }
}
