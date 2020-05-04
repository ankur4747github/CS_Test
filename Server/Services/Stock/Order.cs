using Server.Factory;
using Server.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services.Stock
{
    public class Order : IOrder
    {
        #region Fields

        private volatile bool _alreadyRunning = false;

        private ConcurrentQueue<PlaceOrderData> _unprocessed { get; set; }
        private List<PlaceOrderData> _buyPendingOrders { get; set; }
        private List<PlaceOrderData> _sellPendingOrders { get; set; }
        private List<TradeOrderData> _tradeOrderData { get; set; }

        #endregion Fields

        #region Constructor

        public Order()
        {
            _unprocessed = new ConcurrentQueue<PlaceOrderData>();
            _buyPendingOrders = new List<PlaceOrderData>();
            _sellPendingOrders = new List<PlaceOrderData>();
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

        #region MarketOrderBook

        public void UpdateMarketOrderBook(int clientID)
        {
            var marketOrderBook = GetOrderData();

            ObjFactory.Instance.CreateBroadCastData().BroadCastMarketOrderBookData(marketOrderBook,
                ObjFactory.Instance.CreateRegisterClients().GetClients());
        }

        public MarketOrderBookData GetOrderData()
        {
            var marketOrderBook = ObjFactory.Instance.CreateMarketOrderBookData();
            marketOrderBook.BuyPendingOrders = _buyPendingOrders;
            marketOrderBook.SellPendingOrders = _sellPendingOrders;
            return marketOrderBook;
        }

        #endregion MarketOrderBook

        public List<TradeOrderData> GetTradeListOrderData()
        {
            return _tradeOrderData;
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
                if (_sellPendingOrders.Count > 0)
                {
                    MatchBuyOrder(data);
                }
                else
                {
                    AddNewDataInTheBuyList(data);
                }
            }
            else
            {
                if (_buyPendingOrders.Count > 0)
                {
                    MatchSellOrder(data);
                }
                else
                {
                    AddNewDataInTheSellList(data);
                }
            }
            UpdateMarketOrderBook();
        }

        private void AddNewDataInTheBuyList(PlaceOrderData data)
        {
            _buyPendingOrders.Add(data);
        }

        private void AddNewDataInTheSellList(PlaceOrderData data)
        {
            _sellPendingOrders.Add(data);
        }

        private void MatchSellOrder(PlaceOrderData data)
        {
            var list = _buyPendingOrders.Where(x => x.Price >= data.Price).ToList();
            foreach (var oldData in list)
            {
                ExecuteSellOrder(data, oldData);
                if (data.Quantity == 0)
                {
                    break;
                }
            }

            _buyPendingOrders.RemoveAll(x => x.Quantity == 0);
            if (data.Quantity > 0)
            {
                AddNewDataInTheSellList(data);
            }
        }

        private void MatchBuyOrder(PlaceOrderData data)
        {
            var list = _sellPendingOrders.Where(x => x.Price <= data.Price).ToList();
            foreach (var oldData in list)
            {
                ExecuteBuyOrder(data, oldData);
                if (data.Quantity == 0)
                {
                    break;
                }
            }

            _sellPendingOrders.RemoveAll(x => x.Quantity == 0);
            if (data.Quantity > 0)
            {
                AddNewDataInTheBuyList(data);
            }
        }

        private void ExecuteSellOrder(PlaceOrderData data, PlaceOrderData oldOrderData)
        {
            if (data.Quantity < oldOrderData.Quantity)
            {
                int tradeQuantity = data.Quantity;
                oldOrderData.Quantity = oldOrderData.Quantity - tradeQuantity;
                data.Quantity = 0;
                UpdateTrade(oldOrderData.ClientId, data.ClientId, tradeQuantity, oldOrderData.Price);
            }
            else
            {
                int tradeQuantity = oldOrderData.Quantity;
                oldOrderData.Quantity = 0;
                data.Quantity = data.Quantity - tradeQuantity;
                UpdateTrade(oldOrderData.ClientId, data.ClientId, tradeQuantity, oldOrderData.Price);
            }
        }

        private void ExecuteBuyOrder(PlaceOrderData data, PlaceOrderData oldOrderData)
        {
            if (data.Quantity < oldOrderData.Quantity)
            {
                int tradeQuantity = data.Quantity;
                oldOrderData.Quantity = oldOrderData.Quantity - tradeQuantity;
                data.Quantity = 0;
                UpdateTrade(data.ClientId, oldOrderData.ClientId, tradeQuantity, oldOrderData.Price);
            }
            else
            {
                int tradeQuantity = oldOrderData.Quantity;
                oldOrderData.Quantity = oldOrderData.Quantity - tradeQuantity;
                data.Quantity = data.Quantity - tradeQuantity;
                UpdateTrade(data.ClientId, oldOrderData.ClientId, tradeQuantity, oldOrderData.Price);
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

            Task.Run(() => ObjFactory.Instance.CreateBroadCastData().BroadCastTradeData(tradeData,
                           ObjFactory.Instance.CreateRegisterClients().GetClients()));

            ObjFactory.Instance.CreateLogger().Log(string.Format(
                "BuyUserId {0} sellUserId {1} tradeQuantity {2} tradePrice {3}",
                buyUserId, sellUserId, tradeQuantity, tradePrice), GetType().Name, false);
        }

        #endregion After Trade

        #region MarketOrderBook

        private void UpdateMarketOrderBook()
        {
            var marketOrderBook = GetOrderData();
            ObjFactory.Instance.CreateBroadCastData().BroadCastMarketOrderBookData(marketOrderBook,
                ObjFactory.Instance.CreateRegisterClients().GetClients());
        }

        #endregion MarketOrderBook

        #endregion Private Method
    }
}