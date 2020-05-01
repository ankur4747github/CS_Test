using Server.Factory;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Server.StockServices
{
    public class RegisterClients : IRegisterClients
    {
        private Dictionary<string, IBroadcastorCallBack> _clients { get; set; }

        public RegisterClients()
        {
            _clients = new Dictionary<string, IBroadcastorCallBack>();
        }

        public void RegisterClient(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                IBroadcastorCallBack callback =
                    OperationContext.Current.GetCallbackChannel<IBroadcastorCallBack>();

                if (_clients.Keys.Contains(clientId))
                {
                    _clients.Remove(clientId);
                    ObjFactory.Instance.CreateLogger()
                        .Log("UnRegistered Client = " + clientId, GetType().Name, false);
                }
                _clients.Add(clientId, callback);
                ObjFactory.Instance.CreateLogger()
                        .Log("Registered Client = " + clientId, GetType().Name, false);
            }
        }

        public void UnRegisterClient(List<string> inactiveClients)
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

        public IReadOnlyDictionary<string, IBroadcastorCallBack> GetClients()
        {
            return _clients;
        }
    }
}