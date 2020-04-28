using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TCPBinding;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayStockDetails("NiftyBank");
        }

        private static void DisplayStockDetails(string stockName)
        {
            Console.WriteLine("Press any key for start");
            Console.ReadLine();
            var uri = "net.tcp://localhost:8000/StockService";
            var binding = new NetTcpBinding(SecurityMode.None);
            var channel = new ChannelFactory<IStockService>(binding);
            var endPoint = new EndpointAddress(uri);
            var proxy = channel.CreateChannel(endPoint);
            if (proxy != null)
            {
                SendData(proxy, stockName);
            }
            Console.ReadLine();
        }

        private static void SendData(IStockService proxy, string stockName)
        {
            var stock = GetStock(stockName);
            proxy.PostStockDetail(stock);
            SendData(proxy, stockName);
        }

        private static Stock GetStock(string stockName)
        {
            var rnd = new Random();
            var stock = new Stock
            {
                TimeSent = DateTime.UtcNow,
                Name = stockName,
                Price = rnd.Next(13, 120) + rnd.NextDouble(),
                City = "Pune"
            };
            return stock;
        }
    }
}
