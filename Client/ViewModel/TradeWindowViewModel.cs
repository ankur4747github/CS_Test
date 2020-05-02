using Client.Command;
using Client.Constants;
using Client.Factory;
using Client.StockService;
using GalaSoft.MvvmLight.Messaging;
using Server.Constants;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModel
{
    public class TradeWindowViewModel : BaseViewModel
    {
        #region Button Command

        #region StartCommand

        private ICommand _startCommand;

        public ICommand StartCommand
        {
            get
            {
                return _startCommand;
            }
            set
            {
                _startCommand = value;
            }
        }

        #endregion StartCommand

        #region BuyCommand

        private ICommand _buyCommand;

        public ICommand BuyCommand
        {
            get
            {
                return _buyCommand;
            }
            set
            {
                _buyCommand = value;
            }
        }

        #endregion BuyCommand

        #region SellCommand

        private ICommand _sellCommand;

        public ICommand SellCommand
        {
            get
            {
                return _sellCommand;
            }
            set
            {
                _sellCommand = value;
            }
        }

        #endregion SellCommand

        #endregion Button Command

        #region INotifyPropertyChange Member

        #region ClientId

        public string ClientId
        {
            get { return _clientId; }
            set
            {
                if (value == _clientId)
                    return;

                _clientId = value;
                OnPropertyChangedAsync(nameof(ClientId));
            }
        }

        private string _clientId { get; set; }

        #endregion ClientId

        #region BuySellPrice

        public double BuySellPrice
        {
            get { return _buySellPrice; }
            set
            {
                if (value == _buySellPrice)
                    return;

                _buySellPrice = value;
                OnPropertyChangedAsync(nameof(BuySellPrice));
            }
        }

        private double _buySellPrice { get; set; }

        #endregion BuySellPrice

        #region Quantity

        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value == _quantity)
                    return;

                _quantity = value;
                OnPropertyChangedAsync(nameof(Quantity));
            }
        }

        private int _quantity { get; set; }

        #endregion Quantity

        #region IsTradeUIVisible

        public bool IsTradeUIVisible
        {
            get { return _isTradeUIVisible; }
            set
            {
                if (value == _isTradeUIVisible)
                    return;

                _isTradeUIVisible = value;
                OnPropertyChangedAsync(nameof(IsTradeUIVisible));
            }
        }

        private bool _isTradeUIVisible { get; set; }

        #endregion IsTradeUIVisible

        #region IsProgressBarVisible

        public bool IsProgressBarVisible
        {
            get { return _isProgressBarVisible; }
            set
            {
                if (value == _isProgressBarVisible)
                    return;

                _isProgressBarVisible = value;
                OnPropertyChangedAsync(nameof(IsProgressBarVisible));
            }
        }

        private bool _isProgressBarVisible { get; set; }

        #endregion IsProgressBarVisible

        #region Price

        public double Price
        {
            get { return _price; }
            set
            {
                if (value == _price)
                    return;

                _price = value;
                OnPropertyChangedAsync(nameof(Price));
            }
        }

        private double _price;

        #endregion Price

        #endregion INotifyPropertyChange Member

        #region Constructor

        public TradeWindowViewModel()
        {
            InitCommand();
            InitMessenger();
        }

        #endregion Constructor

        #region InitCommand

        private void InitCommand()
        {
            StartCommand = new RelayCommand(new Action<object>(StartCommandClick));
            BuyCommand = new RelayCommand(new Action<object>(BuyCommandClick));
            SellCommand = new RelayCommand(new Action<object>(SellCommandClick));
        }

        #endregion InitCommand

        #region Private Methods

        #region InitMessenger

        private void InitMessenger()
        {
            Messenger.Default.Unregister<StockData>(this,
                  MessengerToken.BROADCASTSTOCKPRICE, UpdatePrice);
            Messenger.Default.Register<StockData>(this,
                MessengerToken.BROADCASTSTOCKPRICE, UpdatePrice);
        }

        #endregion InitMessenger

        #region Button Click

        private void StartCommandClick(object obj)
        {
            if (!string.IsNullOrEmpty(ClientId?.Trim()) &&
                new Regex(Constant.REGEX_NUMBER_ONLY).IsMatch(ClientId.Trim()))
            {
                IsProgressBarVisible = true;
                StartRegisterClient();
            }
            else
            {
                MessageBox.Show(resourceManager.GetString("EmptyID"));
            }
        }

        private void SellCommandClick(object obj)
        {
            if (IsValidPriceAndQuantity())
            {
                ObjFactory.Instance.CreateLogger().Log(string.Format("Start Sell Price {0} Quantity {1}",
                    BuySellPrice, Quantity), GetType().Name, false);
                var data = GetOrderData(false);
                ObjFactory.Instance.CreateRegisterClients().PlaceOrder(data);
            }
        }

        private void BuyCommandClick(object obj)
        {
            if (IsValidPriceAndQuantity())
            {
                ObjFactory.Instance.CreateLogger().Log(string.Format("Start Buy Price {0} Quantity {1}",
                    BuySellPrice, Quantity), GetType().Name, false);
                var data = GetOrderData(true);
                ObjFactory.Instance.CreateRegisterClients().PlaceOrder(data);
            }
        }

        #endregion Button Click

        #region Register Client

        private async void StartRegisterClient()
        {
            await Task.Run(() => ObjFactory.Instance.CreateRegisterClients().Register(ClientId))
                                .ContinueWith(task => CheckIsClientRegitered(task.Result));
        }

        private void CheckIsClientRegitered(bool isRegistered)
        {
            IsProgressBarVisible = false;
            if (isRegistered)
            {
                IsTradeUIVisible = true;
            }
            else
            {
                MessageBox.Show(resourceManager.GetString("UnableRegister"));
            }
        }

        #endregion Register Client

        #region Update Data

        private void UpdatePrice(StockData obj)
        {
            Price = obj.StockPrice;
        }

        #endregion Update Data

        #region Validation

        private bool IsValidPriceAndQuantity()
        {
            if (BuySellPrice > 0 && Quantity > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show(resourceManager.GetString("ValidPriceQuantity"));
            }
            return false;
        }

        #endregion Validation

        private PlaceOrderData GetOrderData(bool isBuy)
        {
            var data = ObjFactory.Instance.CreateStockServicePlaceOrderData();
            data.Price = BuySellPrice;
            data.IsBuy = isBuy;
            data.IsSell = !isBuy;
            data.Quantity = Quantity;
            return data;
        }

        #endregion Private Methods
    }
}