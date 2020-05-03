using Client.Model;
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

        public IStockService CreateRegisterClients()
        {
            if (!_objContainer.IsRegistered(typeof(Services.Stock.StockService), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(Services.Stock.StockService), MethodBase.GetCurrentMethod().Name);
            }
            return (Services.Stock.StockService)_objContainer.Resolve(typeof(Services.Stock.StockService), MethodBase.GetCurrentMethod().Name);
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

        public ServerStockService.PlaceOrderData CreateStockServicePlaceOrderData()
        {
            if (!_objContainer.IsRegistered(typeof(ServerStockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(ServerStockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (ServerStockService.PlaceOrderData)_objContainer.Resolve(typeof(ServerStockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name);
        }


        public MarketOrderData CreateMarketOrderBookData()
        {
            if (!_objContainer.IsRegistered(typeof(MarketOrderData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(MarketOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (MarketOrderData)_objContainer.Resolve(typeof(MarketOrderData), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Data
    }
}