using Server.Factory;
using Server.Model;
using System.Collections.Generic;

namespace Server.StockServices
{
    public class BroadCastData : IBroadCastData
    {
        #region Public Methods

        public void BroadCastStockPrice(StockData data,
            IReadOnlyDictionary<string, IBroadcastorCallBack> clients)
        {
            var inactiveClients = new List<string>();
            foreach (var client in clients)
            {
                try
                {
                    client.Value.BroadcastToClient(data);
                }
                catch
                {
                    inactiveClients.Add(client.Key);
                }
            }
            RemoveInactiveClients(inactiveClients);
        }

        #endregion Public Methods

        #region Private Methods

        private static void RemoveInactiveClients(List<string> inactiveClients)
        {
            if (inactiveClients.Count > 0)
            {
                ObjFactory.Instance.CreateRegisterClients().UnRegisterClient(inactiveClients);
            }
        }

        #endregion Private Methods
    }
}