using System.Collections.Generic;

namespace Server.StockServices
{
    public interface IRegisterClients
    {
        void RegisterClient(int clientId);

        void UnRegisterClient(List<int> inactiveClients);

        IReadOnlyDictionary<int, IBroadcastorCallBack> GetClients();
    }
}