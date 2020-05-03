using Client.Factory;
using GalaSoft.MvvmLight;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Client.ViewModel
{
    public abstract class BaseViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields

        public ResourceManager resourceManager { get; set; }

        #endregion Fields

        #region Constructor

        public BaseViewModel()
        {
            resourceManager = new ResourceManager("Client.Properties.Resources",
                Assembly.GetExecutingAssembly());
        }

        #endregion Constructor

        #region PropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChangedAsync([CallerMemberName]string propertyName = null)
        {
            try
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (Exception ex)
            {
                ObjFactory.Instance.CreateLogger().Log("OnPropertyChangedAsync = " + ex.Message, GetType().Name);
            }
        }

        #endregion PropertyChanged
    }
}