using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TCPBinding;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var uris = new Uri[1];
            string adr = "net.tcp://localhost:8000/StockService";
            uris[0] = new Uri(adr);
            IStockService stockService = new StockService();
            ServiceHost host = new ServiceHost(stockService);
            var binding = new NetTcpBinding(SecurityMode.None);
            host.AddServiceEndpoint(typeof(IStockService), binding, adr);
            host.Opened += Host_Opened;
            try
            {
                host.Open();
                Console.Read();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Service Opening = " + ex.Message);
            }
        }

        private static void Host_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Service Up and Running");
        }

        
    }
}
