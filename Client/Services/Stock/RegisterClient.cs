using Client.Factory;
using System;

namespace Client.Services.Stock
{
    public class RegisterClient : IRegisterClient
    {
        #region Fields

        private StockService.StockServiceClient _client { get; set; }

       

        #endregion Fields

        #region Public Methods

        public bool Register(string clientId)
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
                    _client = new StockService.StockServiceClient(context);
                    _client?.RegisterClient(clientId);
                    return true;
                }
            }
            catch(Exception ex)
            {
                ObjFactory.Instance.CreateLogger()
                    .Log("Register = " + ex.Message, GetType().Name);
            }
            return false;
        }

        public void PlaceOrder(StockService.PlaceOrderData data)
        {
            if (_client != null)
            {
                _client.PlaceOrder(data);
            }
        }

        #endregion Public Methods
    }
}