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

        #region SelectProfile Buttom Command

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

        #endregion SelectProfile Buttom Command

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
                IsTradeUIVisible = false;
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

        #endregion Private Methods
    }
}