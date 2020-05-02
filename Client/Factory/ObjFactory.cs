using Client.Services.Stock;
using Services.Logging;
using System.Reflection;
using Unity;

namespace Client.Factory
{
    public sealed class ObjFactory
    {
        #region Fields

        private static readonly object _datalock = new object();
        private static ObjFactory _instance { get; set; }
        private IUnityContainer _objContainer { get; set; }

        #endregion Fields

        #region Instance

        public static ObjFactory Instance
        {
            get
            {
                lock (_datalock)
                {
                    if (_instance == null)
                    {
                        _instance = new ObjFactory();
                    }
                    return _instance;
                }
            }
        }

        #endregion Instance

        #region Constructor

        private ObjFactory()
        {
            _objContainer = new UnityContainer();
        }

        #endregion Constructor

        #region Logs

        public ILog CreateLogger()
        {
            if (!_objContainer.IsRegistered(typeof(Logger), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(Logger), MethodBase.GetCurrentMethod().Name);
            }
            return (Logger)_objContainer.Resolve(typeof(Logger), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Logs

        #region Services

        public IRegisterClient CreateRegisterClients()
        {
            if (!_objContainer.IsRegistered(typeof(RegisterClient), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(RegisterClient), MethodBase.GetCurrentMethod().Name);
            }
            return (RegisterClient)_objContainer.Resolve(typeof(RegisterClient), MethodBase.GetCurrentMethod().Name);
        }

        public BroadcastorCallback CreateBroadcastorCallback()
        {
            if (!_objContainer.IsRegistered(typeof(BroadcastorCallback), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(BroadcastorCallback), MethodBase.GetCurrentMethod().Name);
            }
            return (BroadcastorCallback)_objContainer.Resolve(typeof(BroadcastorCallback), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Services

        #region Data

        public StockService.PlaceOrderData CreateStockServicePlaceOrderData()
        {
            if (!_objContainer.IsRegistered(typeof(StockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(StockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (StockService.PlaceOrderData)_objContainer.Resolve(typeof(StockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Data
    }
}