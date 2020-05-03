using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Factory;
using Server.Model;
using Server.Services.Stock;
using System.Threading;

namespace ServerTest.Services.Stock
{
    [TestClass]
    public class Order_Tests
    {
        private IOrder _order;

        [TestInitialize]
        public void SetUp()
        {
            ObjFactory.Instance.Cleanup();
            _order = ObjFactory.Instance.CreateOrder();
        }

        [TestMethod]
        [DataRow(0, 0, 0, true, false)]
        [DataRow(0, 0, 0, false, false)]
        [DataRow(1, 120, 30, true, true)]
        [DataRow(1, 120, 30, true, true)]
        public void CheckAddOrder_Tests(int clientId, double price, int quantity, bool isBuy, bool result)
        {
            var data = GetOrderData(clientId, price, quantity, isBuy);
            bool notAvailable = false;
            if (isBuy)
            {
                notAvailable = !_order.GetOrderData().BuyPendingOrders.Contains(data);
            }
            else
            {
                notAvailable = !_order.GetOrderData().SellPendingOrders.Contains(data);
            }

            _order.AddOrderIntoQueue(data);
            Thread.Sleep(2000);
            if (isBuy)
            {
                bool isOrderPlaced = _order.GetOrderData().BuyPendingOrders.Contains(data);
                Assert.AreEqual((isOrderPlaced && notAvailable), result);
            }
            else
            {
                bool isOrderPlaced = _order.GetOrderData().SellPendingOrders.Contains(data);
                Assert.AreEqual((isOrderPlaced && notAvailable), result);
            }
            Thread.Sleep(1000);
        }

        [TestMethod]
        [DataRow(1, 2, 120, 30, true, true)]
        [DataRow(1, 2, 320, 300, true, true)]
        public void CheckBuyOrderExecute_Tests(int buyerId, int sellerId, double price, int quantity, bool isBuy, bool result)
        {
            var sellerData = GetOrderData(sellerId, price, quantity, !isBuy);
            _order.AddOrderIntoQueue(sellerData);
            Thread.Sleep(2000);
            bool isOrderPlaced = _order.GetOrderData().SellPendingOrders.Contains(sellerData);
            var buyerData = GetOrderData(buyerId, price, quantity, isBuy);
            _order.AddOrderIntoQueue(buyerData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 1)
            {
                Assert.AreEqual((listTradeOrder[0].BuyUserId == buyerId &&
                                listTradeOrder[0].SellUserId == sellerId), result);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [DataRow(1, 2, 120, 30, false, true)]
        [DataRow(1, 2, 1200, 300, false, true)]
        public void CheckSellOrderExecute_Tests(int buyerId, int sellerId, double price, int quantity, bool isBuy, bool result)
        {
            var buyerData = GetOrderData(buyerId, price, quantity, !isBuy);
            _order.AddOrderIntoQueue(buyerData);
            Thread.Sleep(2000);
            bool isOrderPlaced = _order.GetOrderData().BuyPendingOrders.Contains(buyerData);
            var sellerData = GetOrderData(sellerId, price, quantity, isBuy);
            _order.AddOrderIntoQueue(sellerData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 1)
            {
                Assert.AreEqual((listTradeOrder[0].BuyUserId == buyerId &&
                                listTradeOrder[0].SellUserId == sellerId), result);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [DataRow(1, 2, 120, 100, 30, false, true)]
        [DataRow(1, 2, 1200, 800, 300, false, true)]
        public void CheckCorrectQuantityTrade_Tests(int buyerId, int sellerId, double price, int quantity,
                                                    int tradeQuantity, bool isBuy, bool result)
        {
            var buyerData = GetOrderData(buyerId, price, quantity, !isBuy);
            _order.AddOrderIntoQueue(buyerData);
            Thread.Sleep(2000);
            var sellerData = GetOrderData(sellerId, price, tradeQuantity, isBuy);
            _order.AddOrderIntoQueue(sellerData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 1)
            {
                Assert.AreEqual((listTradeOrder[0].BuyUserId == buyerId &&
                                listTradeOrder[0].SellUserId == sellerId &&
                                listTradeOrder[0].TradeQuantity == tradeQuantity), result);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [DataRow(1, 2, 120, 119, 100, false, true)]
        [DataRow(1, 2, 1200, 1100, 100, false, true)]
        public void CheckSellOrderCorrectPriceTrade_Tests(int buyerId, int sellerId, double price, int tradePrice,
                                                          int quantity, bool isBuy, bool result)
        {
            var buyerData = GetOrderData(buyerId, price, quantity, !isBuy);
            _order.AddOrderIntoQueue(buyerData);
            Thread.Sleep(2000);
            var sellerData = GetOrderData(sellerId, tradePrice, quantity, isBuy);
            _order.AddOrderIntoQueue(sellerData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 1)
            {
                Assert.AreEqual((listTradeOrder[0].BuyUserId == buyerId &&
                                listTradeOrder[0].SellUserId == sellerId &&
                                listTradeOrder[0].TradePrice == price), result);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [DataRow(1, 2, 120, 119, 100, true, true)]
        [DataRow(1, 2, 1200, 1100, 100, true, true)]
        public void CheckBuyOrderCorrectPriceTrade_Tests(int buyerId, int sellerId, double price, int tradePrice,
                                                          int quantity, bool isBuy, bool result)
        {
            var sellerData = GetOrderData(sellerId, tradePrice, quantity, !isBuy);
            _order.AddOrderIntoQueue(sellerData);
            Thread.Sleep(2000);
            var buyerData = GetOrderData(buyerId, price, quantity, isBuy);
            _order.AddOrderIntoQueue(buyerData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 1)
            {
                Assert.AreEqual((listTradeOrder[0].BuyUserId == buyerId &&
                                listTradeOrder[0].SellUserId == sellerId &&
                                listTradeOrder[0].TradePrice == tradePrice), result);
            }
            else
            {
                Assert.Fail();
            }
        }

        private static PlaceOrderData GetOrderData(int clientId, double price, int quantity, bool isBuy)
        {
            var data = ObjFactory.Instance.CreatePlaceOrderData();
            data.ClientId = clientId;
            data.Price = price;
            data.Quantity = quantity;
            data.IsBuy = isBuy;
            return data;
        }
    }
}