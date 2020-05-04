using Client.Constants;
using Client.Factory;
using Client.ServerStockService;
using Client.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace ClientTest.ViewModel
{
    [TestClass]
    public class TradeWindowViewModel_Tests
    {
        #region Fields

        private TradeWindowViewModel ViewModel;

        #endregion Fields

        #region Setup

        [TestInitialize]
        public void Setup()
        {
            ObjFactory.Instance.Cleanup();
            ViewModel = ObjFactory.Instance.CreateTradeWindowViewModel();
        }

        #endregion Setup

        #region Check Valid Client ID

        [TestMethod]
        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(11111, true)]
        [DataRow(111111, true)]
        public void CheckValidClientId_Test(int clientID, bool result)
        {
            ViewModel.ClientId = clientID;
            bool isValid = ViewModel.IsValidClientID();
            Assert.AreEqual(isValid, result);
        }

        #endregion Check Valid Client ID

        #region Check Valid Price Quantity

        [TestMethod]
        [DataRow(0, 0, false)]
        [DataRow(1, 0, false)]
        [DataRow(0, 1, false)]
        [DataRow(1, 1, true)]
        [DataRow(1111, 1, true)]
        [DataRow(1111, 1323, true)]
        [DataRow(65656, 747747, true)]
        public void CheckValidPriceQuantity_Test(double price, int quatity, bool result)
        {
            ViewModel.BuySellPrice = price;
            ViewModel.Quantity = quatity;
            bool isValid = ViewModel.IsValidPriceAndQuantity();
            Assert.AreEqual(isValid, result);
        }

        #endregion Check Valid Price Quantity

        #region Check Price Update

        [TestMethod]
        [DataRow(1)]
        [DataRow(100)]
        [DataRow(10000)]
        public void CheckStockPriceUpdate_Test(double stockPrice)
        {
            var eventData = ObjFactory.Instance.CreateStockData();
            eventData.StockPrice = stockPrice;
            Messenger.Default.Send(eventData, MessengerToken.BROADCASTSTOCKPRICE);
            Thread.Sleep(2000);
            Assert.AreEqual(stockPrice, ViewModel.Price);
        }

        #endregion Check Price Update

        #region Check Trade Update

        [TestMethod]
        [DataRow(1, 2, 100, 120)]
        [DataRow(2, 1, 20, 12)]
        public void CheckTradeUpdate_Test
            (int buyUserId, int sellUserID, int tradeQuantity, double tradePrice)
        {
            DispatcherHelper.Initialize();
            var eventData = ObjFactory.Instance.CreateTradeOrderData();
            eventData.BuyUserId = buyUserId;
            eventData.SellUserId = sellUserID;
            eventData.TradeQuantity = tradeQuantity;
            eventData.TradePrice = tradePrice;
            Messenger.Default.Send(eventData, MessengerToken.BROADCASTTRADEDATA);
            Thread.Sleep(2000);

            bool isValidOrderCount = ViewModel.TradeOrderDataList.Count == 1;
            bool isValidDataAdded = (ViewModel.TradeOrderDataList[0].BuyUserId == buyUserId &&
                ViewModel.TradeOrderDataList[0].SellUserId == sellUserID &&
                ViewModel.TradeOrderDataList[0].TradeQuantity == tradeQuantity &&
                ViewModel.TradeOrderDataList[0].TradePrice == tradePrice);

            Assert.IsTrue(isValidOrderCount && isValidDataAdded);
        }

        [TestMethod]
        [DataRow(1, 2, 100, 120, 3)]
        [DataRow(2, 1, 20, 12, 4)]
        public void CheckMultiTradeUpdate_Test
            (int buyUserId, int sellUserID, int tradeQuantity, double tradePrice, int count)
        {
            DispatcherHelper.Initialize();
            var eventData = ObjFactory.Instance.CreateTradeOrderData();
            eventData.BuyUserId = buyUserId;
            eventData.SellUserId = sellUserID;
            eventData.TradeQuantity = tradeQuantity;
            eventData.TradePrice = tradePrice;
            for (int i = 0; i < count; i++)
            {
                Messenger.Default.Send(eventData, MessengerToken.BROADCASTTRADEDATA);
            }

            Thread.Sleep(2000);

            bool isValidOrderCount = ViewModel.TradeOrderDataList.Count == count;
            for (int i = 0; i < count; i++)
            {
                bool isValidDataAdded = (ViewModel.TradeOrderDataList[i].BuyUserId == buyUserId &&
                ViewModel.TradeOrderDataList[i].SellUserId == sellUserID &&
                ViewModel.TradeOrderDataList[i].TradeQuantity == tradeQuantity &&
                ViewModel.TradeOrderDataList[i].TradePrice == tradePrice);

                Assert.IsTrue(isValidOrderCount && isValidDataAdded);
            }
        }

        #endregion Check Trade Update

        #region Check OrderBook Update

        [TestMethod]
        [DataRow(1, true, 100, 120)]
        [DataRow(1, false, 100, 120)]
        public void CheckBidOrderBookUpdate_Test
          (int clientId, bool isBuy, int quantity, double price)
        {
            DispatcherHelper.Initialize();
            var eventData = ObjFactory.Instance.CreateMarketOrderBookData();
            eventData.BuyPendingOrders = new Client.ServerStockService.PlaceOrderData[1];
            var mrktData = GetMarketData(clientId, isBuy, quantity, price);
            eventData.BuyPendingOrders[0] = mrktData;
            ViewModel.ClientId = clientId;
            Messenger.Default.Send(eventData, MessengerToken.BROADCASTMARKETORDERBOOK);
            Thread.Sleep(2000);
            bool isValidOrderCount = ViewModel.MarketOrderDataList.Count == 1;

            bool isValidDataAdded = (ViewModel.MarketOrderDataList[0].MrktBidQuantity == quantity &&
               ViewModel.MarketOrderDataList[0].MyBidQuantity == quantity &&
               ViewModel.MarketOrderDataList[0].Price == price);

            Assert.IsTrue(isValidOrderCount && isValidDataAdded);
        }

        [TestMethod]
        [DataRow(1, 2, 100, 120, 121, true)]
        [DataRow(1, 2, 100, 120, 121, false)]
        public void CheckAskBidOrderBookUpdate_Test
          (int buyerId, int sellerId, int quantity, double buyerPrice,
            double sellerPrice, bool buyerClientSame)
        {
            DispatcherHelper.Initialize();
            var eventData = ObjFactory.Instance.CreateMarketOrderBookData();
            eventData.BuyPendingOrders = new Client.ServerStockService.PlaceOrderData[1];
            eventData.SellPendingOrders = new Client.ServerStockService.PlaceOrderData[1];

            var bmrktData = ObjFactory.Instance.CreateStockServicePlaceOrderData();
            bmrktData.ClientId = buyerId;
            bmrktData.Quantity = quantity;
            bmrktData.Price = buyerPrice;
            bmrktData.IsBuy = true;
            eventData.BuyPendingOrders[0] = bmrktData;

            var smrktData = ObjFactory.Instance.CreateStockServicePlaceOrderData();
            smrktData.ClientId = sellerId;
            smrktData.Quantity = quantity;
            smrktData.Price = sellerPrice;
            smrktData.IsBuy = false;
            eventData.SellPendingOrders[0] = smrktData;

            int myBidQuantity = 0;
            int myAskQuantity = 0;
            if (buyerClientSame)
            {
                ViewModel.ClientId = buyerId;
                myBidQuantity = quantity;
            }
            else
            {
                ViewModel.ClientId = sellerId;
                myAskQuantity = quantity;
            }

            Messenger.Default.Send(eventData, MessengerToken.BROADCASTMARKETORDERBOOK);
            Thread.Sleep(2000);
            bool isValidOrderCount = ViewModel.MarketOrderDataList.Count == 2;

            bool isBidValidDataAdded = (ViewModel.MarketOrderDataList[0].MrktBidQuantity == quantity &&
               ViewModel.MarketOrderDataList[0].MyBidQuantity == myBidQuantity &&
               ViewModel.MarketOrderDataList[0].Price == buyerPrice);

            bool isAskValidDataAdded = (ViewModel.MarketOrderDataList[1].MrktAskQuantity == quantity &&
               ViewModel.MarketOrderDataList[1].MyAskQuantity == myAskQuantity &&
               ViewModel.MarketOrderDataList[1].Price == sellerPrice);

            Assert.IsTrue(isValidOrderCount && isBidValidDataAdded && isAskValidDataAdded);
        }

        #endregion Check OrderBook Update

        #region Private Methods

        private PlaceOrderData GetMarketData(int clientId, bool isBuy, int quantity, double price)
        {
            var mrktData = ObjFactory.Instance.CreateStockServicePlaceOrderData();
            mrktData.ClientId = clientId;
            mrktData.Quantity = quantity;
            mrktData.Price = price;
            mrktData.IsBuy = isBuy;
            return mrktData;
        }

        #endregion Private Methods
    }
}