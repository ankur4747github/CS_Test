using Server.Factory;
using Server.StockServices;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Task.Run(() => HostStockService());
            Console.WriteLine("Press any key twice to stop service");
            Console.ReadLine();
            Console.ReadLine();
        }

        public static void HostStockService()
        {
            try
            {
                using (ServiceHost host = new ServiceHost(typeof(StockService)))
                {
                    host.Open();
                    Console.WriteLine("Host started @ " + DateTime.Now.ToString());
                    ObjFactory.Instance.CreateLogger()
                        .Log("Host started", "Program", false);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ObjFactory.Instance.CreateLogger()
                    .Log("HostStockService EX = " + ex.Message, "Program");
            }
        }
    }
}