using Server.Factory;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Server.StockServices
{
    public class RegisterClients : IRegisterClients
    {
        #region Fields

        private Dictionary<int, IBroadcastorCallBack> _clients { get; set; }

        #endregion Fields

        #region Constructor

        public RegisterClients()
        {
            _clients = new Dictionary<int, IBroadcastorCallBack>();
        }

        #endregion Constructor

        #region Public Methods

        public void RegisterClient(int clientId)
        {
            if (clientId > 0)
            {
                IBroadcastorCallBack callback =
                    OperationContext.Current?.GetCallbackChannel<IBroadcastorCallBack>();

                if (_clients.Keys.Contains(clientId))
                {
                    _clients.Remove(clientId);
                    ObjFactory.Instance.CreateLogger()
                        .Log("UnRegistered Client = " + clientId, GetType().Name, false);
                }
                _clients.Add(clientId, callback);
                ObjFactory.Instance.CreateLogger()
                        .Log("Registered Client = " + clientId, GetType().Name, false);
                ObjFactory.Instance.CreateOrder().UpdateMarketOrderBook(clientId);
            }
        }

        public void UnRegisterClient(List<int> inactiveClients)
        {
            if (inactiveClients.Count > 0)
            {
                foreach (var client in inactiveClients)
                {
                    _clients.Remove(client);
                    ObjFactory.Instance.CreateLogger()
                        .Log("UnRegistered Client = " + client, GetType().Name, false);
                }
            }
        }

        public IReadOnlyDictionary<int, IBroadcastorCallBack> GetClients()
        {
            return _clients;
        }

        #endregion Public Methods
    }
}