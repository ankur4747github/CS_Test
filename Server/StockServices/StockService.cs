using Server.Factory;
using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server.StockServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class StockService : IStockService
    {
        #region Private Fields

        private object _locker { get; set; }

        #endregion Private Fields

        #region Constructor

        public StockService()
        {
            StartBroadCastingPrice();
        }

        #endregion Constructor

        #region Public Methods

        public void RegisterClient(string clientId)
        {
            lock (_locker)
            {
                ObjFactory.Instance.CreateRegisterClients().RegisterClient(clientId);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private async void StartBroadCastingPrice()
        {
            var eventDataType = ObjFactory.Instance.CreateStockData();
            eventDataType.StockPrice = new Random().Next(100, 120);
            lock (_locker)
            {
                ObjFactory.Instance.CreateBroadCastData()
                    .BroadCastStockPrice(eventDataType,
                    ObjFactory.Instance.CreateRegisterClients().GetClients());
            }

            await Task.Delay(500);
            StartBroadCastingPrice();
        }

        #endregion Private Methods
    }
}