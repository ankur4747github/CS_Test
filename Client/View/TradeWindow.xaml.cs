using Client.Factory;
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
            DispatcherHelper.Initialize();
            ViewModel = ObjFactory.Instance.CreateTradeWindowViewModel();
            this.DataContext = ViewModel;
        }
    }
}