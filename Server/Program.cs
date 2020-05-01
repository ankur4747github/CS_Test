using Server.StockServices;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(() => HostStockService());
            Console.WriteLine("Press any key twice to stop service");
            Console.ReadLine();
            Console.ReadLine();
        }

        private static void HostStockService()
        {
            using (ServiceHost host = new ServiceHost(typeof(StockService)))
            {
                host.Open();
                Console.WriteLine("Host started @ " + DateTime.Now.ToString());
                Console.ReadLine();
            }
        }
    }
}