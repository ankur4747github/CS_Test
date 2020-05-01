using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client.ViewModel
{
    public abstract class BaseViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region PropertyChanged

        public new event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChangedAsync([CallerMemberName]string propertyName = null)
        {
            try
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    try
                    {
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                    }
                    catch (Exception ex)
                    {
                        //ObjFactory.Instance.CreateLogger().Log("OnPropertyChangedAsync 1= " + ex.Message, GetType().Name);
                    }
                });
            }
            catch (Exception ex)
            {
                //ObjFactory.Instance.CreateLogger().Log("OnPropertyChangedAsync 2= " + ex.Message, GetType().Name);
            }
        }

        #endregion PropertyChanged
    }
}
