using Server.Factory;
using Server.Model;
using System.Collections.Generic;

namespace Server.StockServices
{
    public class BroadCastData : IBroadCastData
    {
        #region Public Methods

        public void BroadCastStockPrice(StockData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients)
        {
            var inactiveClients = new List<int>();
            foreach (var client in clients)
            {
                try
                {
                    client.Value.BroadcastPriceToClient(data);
                    ObjFactory.Instance.CreateLogger()
                        .Log("After BroadCast to clientId = " + client, this.GetType().Name, false);
                }
                catch
                {
                    inactiveClients.Add(client.Key);
                    ObjFactory.Instance.CreateLogger()
                        .Log("Error in BroadCast Added in inactiveList ClientId =" + client, this.GetType().Name);
                }
            }
            RemoveInactiveClients(inactiveClients);
        }

        public void BroadCastTradeData(TradeOrderData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients)
        {
            foreach (var client in clients)
            {
                //BroadCast Trade Update to all Client
                //if(data.BuyUserId == client.Key || data.SellUserId == client.Key)
                {
                    try
                    {
                        client.Value.BroadcastTradeDataToClient(data);
                    }
                    catch
                    {
                        ObjFactory.Instance.CreateLogger()
                            .Log("BroadCastTradeData =" + client, this.GetType().Name);
                    }
                }
            }
        }

        public void BroadCastMarketOrderBookData(MarketOrderBookData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients)
        {
            foreach (var client in clients)
            {
                try
                {
                    client.Value.BroadCastMarketOrderBookData(data);
                }
                catch
                {
                    ObjFactory.Instance.CreateLogger()
                        .Log("BroadCastMarketOrderBookData =" + client, this.GetType().Name);
                }
            }
        }

        public void BroadCastMarketOrderBookDataToSingleClient(MarketOrderBookData data,
            IReadOnlyDictionary<int, IBroadcastorCallBack> clients, int clientId)
        {
            try
            {
                var value = clients[clientId];
                if (value != null)
                {
                    value.BroadCastMarketOrderBookData(data);
                }
            }
            catch
            {
                ObjFactory.Instance.CreateLogger()
                    .Log("BroadCastMarketOrderBookDataToSingleClient =" + clientId, this.GetType().Name);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void RemoveInactiveClients(List<int> inactiveClients)
        {
            if (inactiveClients.Count > 0)
            {
                ObjFactory.Instance.CreateRegisterClients().UnRegisterClient(inactiveClients);
            }
        }

        #endregion Private Methods
    }
}