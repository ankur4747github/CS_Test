using Client.Model;
using Client.Services.Stock;
using Client.ViewModel;
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

        public Services.Stock.IStockService CreateStockServiceClients()
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
                _objContainer.RegisterInstance(typeof(ServerStockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (ServerStockService.PlaceOrderData)_objContainer.Resolve(typeof(ServerStockService.PlaceOrderData), MethodBase.GetCurrentMethod().Name);
        }

        public MarketOrderData CreateMarketOrderData()
        {
            if (!_objContainer.IsRegistered(typeof(MarketOrderData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(MarketOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (MarketOrderData)_objContainer.Resolve(typeof(MarketOrderData), MethodBase.GetCurrentMethod().Name);
        }

        public ServerStockService.MarketOrderBookData CreateMarketOrderBookData()
        {
            if (!_objContainer.IsRegistered(typeof(ServerStockService.MarketOrderBookData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(ServerStockService.MarketOrderBookData), MethodBase.GetCurrentMethod().Name);
            }
            return (ServerStockService.MarketOrderBookData)_objContainer.Resolve(typeof(ServerStockService.MarketOrderBookData), MethodBase.GetCurrentMethod().Name);
        }

        public ServerStockService.StockData CreateStockData()
        {
            if (!_objContainer.IsRegistered(typeof(ServerStockService.StockData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(ServerStockService.StockData), MethodBase.GetCurrentMethod().Name);
            }
            return (ServerStockService.StockData)_objContainer.Resolve(typeof(ServerStockService.StockData), MethodBase.GetCurrentMethod().Name);
        }

        public ServerStockService.TradeOrderData CreateTradeOrderData()
        {
            if (!_objContainer.IsRegistered(typeof(ServerStockService.TradeOrderData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(ServerStockService.TradeOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (ServerStockService.TradeOrderData)_objContainer.Resolve(typeof(ServerStockService.TradeOrderData), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Data

        #region ViewModel

        public TradeWindowViewModel CreateTradeWindowViewModel()
        {
            if (!_objContainer.IsRegistered(typeof(TradeWindowViewModel), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(TradeWindowViewModel), MethodBase.GetCurrentMethod().Name);
            }
            return (TradeWindowViewModel)_objContainer.Resolve(typeof(TradeWindowViewModel), MethodBase.GetCurrentMethod().Name);
        }

        #endregion ViewModel

        #region CleanUp

        public void Cleanup()
        {
            _objContainer.Dispose();
            _objContainer = new UnityContainer();
        }

        #endregion CleanUp
    }
}