using Server.Factory;
using Server.Model;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server.StockServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class StockService : IStockService
    {
        #region Fields

        private object _locker { get; set; }

        #endregion Fields

        #region Constructor

        public StockService()
        {
            _locker = new object();
            Task.Run(() => StartBroadCastingPrice());
        }

        #endregion Constructor

        #region Public Methods

        public void RegisterClient(int clientId)
        {
            ObjFactory.Instance.CreateLogger()
                    .Log("Start Registered ClientId =" + clientId, this.GetType().Name, false);
            lock (_locker)
            {
                ObjFactory.Instance.CreateRegisterClients().RegisterClient(clientId);
            }
        }

        public void PlaceOrder(PlaceOrderData data)
        {
            ObjFactory.Instance.CreateOrder().AddOrderIntoQueue(data);
        }

        #endregion Public Methods

        #region Private Methods

        private async void StartBroadCastingPrice()
        {
            if (ObjFactory.Instance.CreateRegisterClients().GetClients() != null &&
                ObjFactory.Instance.CreateRegisterClients().GetClients().Count > 0)
            {
                var eventDataType = ObjFactory.Instance.CreateStockData();
                eventDataType.StockPrice = new Random().Next(100, 120);
                ObjFactory.Instance.CreateLogger()
                        .Log("BroadCast Price = " + eventDataType.StockPrice, this.GetType().Name, false);
                lock (_locker)
                {
                    ObjFactory.Instance.CreateBroadCastData()
                        .BroadCastStockPrice(eventDataType,
                        ObjFactory.Instance.CreateRegisterClients().GetClients());
                }
            }
            await Task.Delay(500);
            StartBroadCastingPrice();
        }

        #endregion Private Methods
    }
}