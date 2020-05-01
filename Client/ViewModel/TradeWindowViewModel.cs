using Client.Command;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
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

        #endregion INotifyPropertyChange Member

        #region Constructor

        public TradeWindowViewModel()
        {
            InitCommand();
        }

        #endregion Constructor

        #region Init

        private void InitCommand()
        {
            StartCommand = new RelayCommand(new Action<object>(StartCommandClick));
        }

        #endregion Init

        #region Private Methods

        #region Button Click

        private void StartCommandClick(object obj)
        {
            if (!string.IsNullOrEmpty(ClientId?.Trim()) &&
                new Regex("^[1-9]+$").IsMatch(ClientId.Trim()))
            {
                IsTradeUIVisible = true;
            }
            else
            {
                MessageBox.Show(resourceManager.GetString("EmptyID"));
            }
        }

        #endregion Button Click

        #endregion Private Methods
    }
}