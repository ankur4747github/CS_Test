using Server.Model;
using Server.Services.Stock;
using Server.StockServices;
using Services.Logging;
using System.Reflection;
using Unity;

namespace Server.Factory
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

        internal ILog CreateLogger()
        {
            if (!_objContainer.IsRegistered(typeof(Logger), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(Logger), MethodBase.GetCurrentMethod().Name);
            }
            return (Logger)_objContainer.Resolve(typeof(Logger), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Logs

        #region Data

        public StockData CreateStockData()
        {
            if (!_objContainer.IsRegistered(typeof(StockData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(StockData), MethodBase.GetCurrentMethod().Name);
            }
            return (StockData)_objContainer.Resolve(typeof(StockData), MethodBase.GetCurrentMethod().Name);
        }

        public TradeOrderData CreateTradeOrderData()
        {
            if (!_objContainer.IsRegistered(typeof(TradeOrderData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(TradeOrderData), MethodBase.GetCurrentMethod().Name);
            }
            return (TradeOrderData)_objContainer.Resolve(typeof(TradeOrderData), MethodBase.GetCurrentMethod().Name);
        }

        public MarketOrderBookData CreateMarketOrderBookData()
        {
            if (!_objContainer.IsRegistered(typeof(MarketOrderBookData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterInstance(typeof(MarketOrderBookData), MethodBase.GetCurrentMethod().Name);
            }
            return (MarketOrderBookData)_objContainer.Resolve(typeof(MarketOrderBookData), MethodBase.GetCurrentMethod().Name);
        }

        #endregion Data

        #region Services

        public IRegisterClients CreateRegisterClients()
        {
            if (!_objContainer.IsRegistered(typeof(RegisterClients), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(RegisterClients), MethodBase.GetCurrentMethod().Name);
            }
            return (RegisterClients)_objContainer.Resolve(typeof(RegisterClients), MethodBase.GetCurrentMethod().Name);
        }

        public IBroadCastData CreateBroadCastData()
        {
            if (!_objContainer.IsRegistered(typeof(BroadCastData), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(BroadCastData), MethodBase.GetCurrentMethod().Name);
            }
            return (BroadCastData)_objContainer.Resolve(typeof(BroadCastData), MethodBase.GetCurrentMethod().Name);
        }

        public IOrder CreateOrder()
        {
            if (!_objContainer.IsRegistered(typeof(Order), MethodBase.GetCurrentMethod().Name))
            {
                _objContainer.RegisterSingleton(typeof(Order), MethodBase.GetCurrentMethod().Name);
            }
            return (Order)_objContainer.Resolve(typeof(Order), MethodBase.GetCurrentMethod().Name);
        }
        #endregion Services
    }
}