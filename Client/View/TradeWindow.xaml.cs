using Client.ViewModel;
using GalaSoft.MvvmLight.Threading;
using System.Windows;

namespace Client
{
    public partial class TradeWindow : Window
    {
        public TradeWindowViewModel ViewModel { get; set; }

        public TradeWindow()
        {
            InitializeComponent();
            //DispatcherHelper.Initialize();
            ViewModel = new TradeWindowViewModel();
            this.DataContext = ViewModel;
        }
    }
}