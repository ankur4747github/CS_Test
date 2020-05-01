using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Server.StockServices
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class StockService : IStockService
    {
        private static Dictionary<string, IBroadcastorCallBack> _clients =
                     new Dictionary<string, IBroadcastorCallBack>();

        private static object locker = new object();

        public StockService()
        {
            StartBroadCastingPrice();
        }

        public void RegisterClient(string clientId)
        {
            if (!string.IsNullOrEmpty(clientId))
            {
                try
                {
                    IBroadcastorCallBack callback =
                        OperationContext.Current.GetCallbackChannel<IBroadcastorCallBack>();
                    lock (locker)
                    {
                        if (_clients.Keys.Contains(clientId))
                            _clients.Remove(clientId);
                        _clients.Add(clientId, callback);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async void StartBroadCastingPrice()
        {
            lock (locker)
            {
                var inactiveClients = new List<string>();
                foreach (var client in _clients)
                {
                    try
                    {
                        StockData eventDataType = new StockData();
                        eventDataType.StockPrice = new Random().Next(100, 120);
                        client.Value.BroadcastToClient(eventDataType);
                    }
                    catch (Exception ex)
                    {
                        inactiveClients.Add(client.Key);
                    }
                }

                CleanInactiveClients(inactiveClients);
            }
            await Task.Delay(250);
            StartBroadCastingPrice();
        }

        private void CleanInactiveClients(List<string> inactiveClients)
        {
            if (inactiveClients.Count > 0)
            {
                foreach (var client in inactiveClients)
                {
                    _clients.Remove(client);
                }
            }
        }
    }
}