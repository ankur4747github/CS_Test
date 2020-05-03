using Server.Factory;
using Server.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Stock
{
    public class Order : IOrder
    {
        #region Fields

        private volatile bool _alreadyRunning;

        private ConcurrentQueue<PlaceOrderData> _unprocessed;
        private Dictionary<double, Queue<PlaceOrderData>> _buyPendinfOrders { get; set; }
        private Dictionary<double, Queue<PlaceOrderData>> _sellPendinfOrders { get; set; }
        private List<TradeOrderData> _tradeOrderData { get; set; }

        #endregion Fields

        #region Constructor

        public Order()
        {
            _unprocessed = new ConcurrentQueue<PlaceOrderData>();
            _buyPendinfOrders = new Dictionary<double, Queue<PlaceOrderData>>();
            _sellPendinfOrders = new Dictionary<double, Queue<PlaceOrderData>>();
            _tradeOrderData = new List<TradeOrderData>();
        }

        #endregion Constructor

        #region Public Methods

        public void AddOrderIntoQueue(PlaceOrderData data)
        {
            if (IsValidOrder(data))
            {
                _unprocessed.Enqueue(data);
                ObjFactory.Instance.CreateLogger()
                        .Log(string.Format("Order Enqueue Client Id = {0} Price = {1} Quantity= {2} type ={3}",
                        data.ClientId, data.Price, data.Quantity, data.IsBuy)
                        , this.GetType().Name, false);

                if (!_unprocessed.IsEmpty && !_alreadyRunning)
                {
                    _alreadyRunning = true;
                    Task.Run(() => StartProcess_UnProcessedOrder());
                }
            }
        }

        #endregion Public Methods

        #region Private Method

        private bool IsValidOrder(PlaceOrderData data)
        {
            if (data != null && data.ClientId > 0 &&
                data.Price > 0 && data.Quantity > 0)
            {
                return true;
            }
            return false;
        }

        private void StartProcess_UnProcessedOrder()
        {
            PlaceOrderData data;
            _unprocessed.TryDequeue(out data);
            if (data != null)
            {
                MatchOrders(data);
            }

            if (!_unprocessed.IsEmpty)
            {
                StartProcess_UnProcessedOrder();
            }
            else
            {
                _alreadyRunning = false;
            }
        }

        #region Match Order

        private void MatchOrders(PlaceOrderData data)
        {
            if (data.IsBuy)
            {
                if (_sellPendinfOrders.Count > 0)
                {
                    MatchBuyOrder(data);
                }
                else
                {
                    AddNewDataInTheBuyQueue(data);
                }
            }
            else
            {
                if (_buyPendinfOrders.Count > 0)
                {
                    MatchSellOrder(data);
                }
                else
                {
                    AddNewDataInTheSellQueue(data);
                }
            }
            UpdateMarketOrderBook();
        }

        private void AddNewDataInTheBuyQueue(PlaceOrderData data)
        {
            if (!_buyPendinfOrders.ContainsKey(data.Price))
            {
                Queue<PlaceOrderData> placeOrderData = new Queue<PlaceOrderData>();
                placeOrderData.Enqueue(data);
                _buyPendinfOrders.Add(data.Price, placeOrderData);
            }
            else
            {
                Queue<PlaceOrderData> placeOrderData = _buyPendinfOrders[data.Price];
                placeOrderData.Enqueue(data);
            }
        }

        private void AddNewDataInTheSellQueue(PlaceOrderData data)
        {
            if (!_sellPendinfOrders.ContainsKey(data.Price))
            {
                Queue<PlaceOrderData> placeOrderData = new Queue<PlaceOrderData>();
                placeOrderData.Enqueue(data);
                _sellPendinfOrders.Add(data.Price, placeOrderData);
            }
            else
            {
                Queue<PlaceOrderData> placeOrderData = _sellPendinfOrders[data.Price];
                placeOrderData.Enqueue(data);
            }
        }

        private void MatchSellOrder(PlaceOrderData data)
        {
            var keyList = _buyPendinfOrders.Where(x => x.Key >= data.Price)
                                        .Select(x => x.Key)
                                        .ToList();
            foreach (var key in keyList)
            {
                var value = _buyPendinfOrders[key];
                ExecuteSellOrder(data, value, key);
                if (data.Quantity == 0)
                {
                    break;
                }
            }
            if (data.Quantity > 0)
            {
                AddNewDataInTheSellQueue(data);
            }
        }

        private void MatchBuyOrder(PlaceOrderData data)
        {
            var keyList = _sellPendinfOrders.Where(x => x.Key >= data.Price)
                                            .Select(x => x.Key)
                                            .ToList();
            foreach (var key in keyList)
            {
                var value = _sellPendinfOrders[key];
                ExecuteBuyOrder(data, value, key);
                if (data.Quantity == 0)
                {
                    break;
                }
            }
            if (data.Quantity > 0)
            {
                AddNewDataInTheBuyQueue(data);
            }
        }

        private void ExecuteSellOrder(PlaceOrderData data, Queue<PlaceOrderData> value, double key)
        {
            if (value.Count > 0)
            {
                var oldOrderData = value.Peek();
                if (data.Quantity > oldOrderData.Quantity)
                {
                    int tradeQuantity = oldOrderData.Quantity;
                    oldOrderData.Quantity = oldOrderData.Quantity - tradeQuantity;
                    data.Quantity = data.Quantity - tradeQuantity;
                    Task.Run(() => UpdateTrade(oldOrderData.ClientId, data.ClientId, tradeQuantity, oldOrderData.Price));
                }
                else
                {
                    int tradeQuantity = data.Quantity;
                    oldOrderData.Quantity = 0;
                    data.Quantity = data.Quantity - tradeQuantity;
                    Task.Run(() => UpdateTrade(oldOrderData.ClientId, data.ClientId, tradeQuantity, oldOrderData.Price));

                    value.Dequeue();
                    if (data.Quantity > 0)
                    {
                        ExecuteSellOrder(data, value, key);
                    }
                }
            }
            else
            {
                _buyPendinfOrders.Remove(key);
            }
        }

        private void ExecuteBuyOrder(PlaceOrderData data, Queue<PlaceOrderData> value, double key)
        {
            if (value.Count > 0)
            {
                var oldOrderData = value.Peek();
                if (data.Quantity < oldOrderData.Quantity)
                {
                    int tradeQuantity = oldOrderData.Quantity;
                    Task.Run(() => UpdateTrade(data.ClientId, oldOrderData.ClientId, tradeQuantity, oldOrderData.Price));
                    oldOrderData.Quantity = oldOrderData.Quantity - tradeQuantity;
                }
                else
                {
                    int tradeQuantity = data.Quantity;
                    Task.Run(() => UpdateTrade(data.ClientId, oldOrderData.ClientId, tradeQuantity, oldOrderData.Price));
                    oldOrderData.Quantity = 0;
                    value.Dequeue();
                    if (data.Quantity > 0)
                    {
                        ExecuteSellOrder(data, value, key);
                    }
                }
            }
            else
            {
                _sellPendinfOrders.Remove(key);
            }
        }

        #endregion Match Order

        #region After Trade

        private void UpdateTrade(int buyUserId, int sellUserId, int tradeQuantity, double tradePrice)
        {
            var tradeData = ObjFactory.Instance.CreateTradeOrderData();
            tradeData.BuyUserId = buyUserId;
            tradeData.SellUserId = sellUserId;
            tradeData.TradeQuantity = tradeQuantity;
            tradeData.TradePrice = tradePrice;
            _tradeOrderData.Add(tradeData);

            ObjFactory.Instance.CreateBroadCastData().BroadCastTradeData(tradeData,
                ObjFactory.Instance.CreateRegisterClients().GetClients());
        }

        #endregion After Trade

        #region MarketOrderBook

        private void UpdateMarketOrderBook()
        {
            var marketOrderBook = ObjFactory.Instance.CreateMarketOrderBookData();
            marketOrderBook.BuyPendingOrders = _buyPendinfOrders;
            marketOrderBook.SellPendingOrders = _sellPendinfOrders;

            ObjFactory.Instance.CreateBroadCastData().BroadCastMarketOrderBookData(marketOrderBook,
                ObjFactory.Instance.CreateRegisterClients().GetClients());
        }

        #endregion MarketOrderBook

        #endregion Private Method
    }
}