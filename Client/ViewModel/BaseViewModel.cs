using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using Client.Factory;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Resources;
using System.Reflection;

namespace Client.ViewModel
{
    public abstract class BaseViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Fields
        public ResourceManager resourceManager { get; set; }
        #endregion

        #region Constructor
        public BaseViewModel()
        {
            resourceManager = new ResourceManager("Client.Properties.Resources", 
                Assembly.GetExecutingAssembly());
        }
        #endregion

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