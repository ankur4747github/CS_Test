using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.StockServices
{
    public interface IRegisterClients
    {
        void RegisterClient(string clientId);
        void UnRegisterClient(List<string> inactiveClients);
        IReadOnlyDictionary<string, IBroadcastorCallBack> GetClients();

    }
}
