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
            StockData eventData = ObjFactory.Instance.CreateStockData();
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
            TradeOrderData eventData = ObjFactory.Instance.CreateTradeOrderData();
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
            TradeOrderData eventData = ObjFactory.Instance.CreateTradeOrderData();
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
    }
}