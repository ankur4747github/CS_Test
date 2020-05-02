using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.StockServices
{
    public interface IRegisterClients
    {
        void RegisterClient(int clientId);
        void UnRegisterClient(List<int> inactiveClients);
        IReadOnlyDictionary<int, IBroadcastorCallBack> GetClients();



    }
}
