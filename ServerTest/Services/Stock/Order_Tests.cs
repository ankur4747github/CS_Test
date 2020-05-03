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

        [TestMethod]
        [DataRow(1, 2, 3, 10, 10, 17, 10, 7)]
        //Example:- 
        // A placed Buy order Quantity = 10
        // B placed Buy order Quantity = 10
        // C placed Sell order Quantity = 17
        // TradeOrders
        // 1) A will get 10 Quantity
        // 2) B will get 7 Quantity
        public void CheckTimePriorityMatching_Tests(int buyerClientA, int buyerClientB, int sellerClientC,
                                                    int buyerClientA_AskQuantity, int buyerClientB_AskQuantity,
                                                    int sellerClientC_BidQuantity, int tradewithA_Quantity, int tradewithB_Quantity)
        {
            var buyerClientAData = GetOrderData(buyerClientA, 120, buyerClientA_AskQuantity, true);
            _order.AddOrderIntoQueue(buyerClientAData);
            var buyerClientBData = GetOrderData(buyerClientB, 120, buyerClientB_AskQuantity, true);
            _order.AddOrderIntoQueue(buyerClientBData);
            var sellerClientCData = GetOrderData(sellerClientC, 120, sellerClientC_BidQuantity, false);
            _order.AddOrderIntoQueue(sellerClientCData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 2)
            {
                bool trade1Result = (listTradeOrder[0].BuyUserId == buyerClientA &&
                                     listTradeOrder[0].SellUserId == sellerClientC &&
                                     listTradeOrder[0].TradeQuantity == tradewithA_Quantity);

                bool trade2Result = (listTradeOrder[1].BuyUserId == buyerClientB &&
                                     listTradeOrder[1].SellUserId == sellerClientC &&
                                     listTradeOrder[1].TradeQuantity == tradewithB_Quantity);

                Assert.IsTrue(trade1Result && trade2Result);
            }
            else
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        [DataRow(1, 2, 3, 10, 10, 25, 10, 10, 5)]
        [DataRow(1, 2, 3, 40, 40, 95, 40, 40, 15)]
        //Example:- 
        // A placed Buy order Quantity = 10
        // B placed Buy order Quantity = 10
        // A placed Buy order Quantity = 10
        // B placed Buy order Quantity = 10
        // C placed Sell order Quantity = 25
        // TradeOrders
        // 1) A will get 10 Quantity
        // 2) B will get 10 Quantity
        // 3) A will get 5 Quantity
        public void CheckTimePriorityMatching_MultipleOrders_MultipleUsers_Tests
            (int buyerClientA, int buyerClientB, int sellerClientC,
            int buyerClientA_AskQuantity, int buyerClientB_AskQuantity,
            int sellerClientC_BidQuantity, int firstOrderQuantity, int secondOrderQuantity,
            int thirdOrderQuantity)
        {
            var order1Data = GetOrderData(buyerClientA, 120, buyerClientA_AskQuantity, true);
            _order.AddOrderIntoQueue(order1Data);
            var order2Data = GetOrderData(buyerClientB, 120, buyerClientB_AskQuantity, true);
            _order.AddOrderIntoQueue(order2Data);
            var order3Data = GetOrderData(buyerClientA, 120, buyerClientA_AskQuantity, true);
            _order.AddOrderIntoQueue(order3Data);
            var order4Data = GetOrderData(buyerClientB, 120, buyerClientB_AskQuantity, true);
            _order.AddOrderIntoQueue(order4Data);

            var sellerData = GetOrderData(sellerClientC, 120, sellerClientC_BidQuantity, false);
            _order.AddOrderIntoQueue(sellerData);
            Thread.Sleep(2000);
            var listTradeOrder = _order.GetTradeListOrderData();
            if (listTradeOrder.Count == 3)
            {
                bool trade1Result = (listTradeOrder[0].BuyUserId == buyerClientA &&
                                     listTradeOrder[0].SellUserId == sellerClientC &&
                                     listTradeOrder[0].TradeQuantity == firstOrderQuantity);

                bool trade2Result = (listTradeOrder[1].BuyUserId == buyerClientB &&
                                     listTradeOrder[1].SellUserId == sellerClientC &&
                                     listTradeOrder[1].TradeQuantity == secondOrderQuantity);

                bool trade3Result = (listTradeOrder[2].BuyUserId == buyerClientA &&
                                     listTradeOrder[2].SellUserId == sellerClientC &&
                                     listTradeOrder[2].TradeQuantity == thirdOrderQuantity);

                Assert.IsTrue(trade1Result && trade2Result && trade3Result);
            }
            else
            {
                Assert.Fail();
            }
        }

        private PlaceOrderData GetOrderData(int clientId, double price, int quantity, bool isBuy)
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