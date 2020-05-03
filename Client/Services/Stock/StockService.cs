using Client.Factory;
using System;

namespace Client.Services.Stock
{
    public class StockService : IStockService
    {
        #region Fields

        private ServerStockService.StockServiceClient _client { get; set; }

        #endregion Fields

        #region Public Methods

        public bool Register(int clientId)
        {
            try
            {
                if (_client != null)
                {
                    _client.Abort();
                    _client = null;
                }

                var cb = ObjFactory.Instance.CreateBroadcastorCallback();
                var context = new System.ServiceModel.InstanceContext(cb);
                if (context != null)
                {
                    _client = new ServerStockService.StockServiceClient(context);
                    _client?.RegisterClient(clientId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ObjFactory.Instance.CreateLogger()
                    .Log("Register = " + ex.Message, GetType().Name);
            }
            return false;
        }

        public bool PlaceOrder(ServerStockService.PlaceOrderData data)
        {
            try
            {
                if (_client != null)
                {
                    _client.PlaceOrder(data);
                    return true;
                }
            }
            catch (Exception ex)
            {
                ObjFactory.Instance.CreateLogger()
                    .Log("Register = " + ex.Message, GetType().Name);
            }
            return false;
        }

        #endregion Public Methods
    }
}