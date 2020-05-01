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
                }
                _clients.Add(clientId, callback);
            }
        }

        public void UnRegisterClient(List<string> inactiveClients)
        {
            if (inactiveClients.Count > 0)
            {
                foreach (var client in inactiveClients)
                {
                    _clients.Remove(client);
                }
            }
        }

        public IReadOnlyDictionary<string, IBroadcastorCallBack> GetClients()
        {
            return _clients;
        }
    }
}