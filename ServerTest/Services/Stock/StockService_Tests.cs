﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Factory;
using Server.Services.Stock;
using Server.StockServices;
using System.Threading;

namespace ServerTest.Services.Stock
{
    [TestClass]
    public class StockService_Tests
    {
        #region Fields

        private IStockService _stockService;
        private IRegisterClients _registerClients;
        private IOrder _order;

        #endregion Fields

        #region Setup

        [TestInitialize]
        public void SetUp()
        {
            ObjFactory.Instance.Cleanup();
            _stockService = ObjFactory.Instance.CreateStockService();
            _registerClients = ObjFactory.Instance.CreateRegisterClients();
            _order = ObjFactory.Instance.CreateOrder();
        }

        #endregion Setup

        #region Test Methods

        [TestMethod]
        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(210, true)]
        [DataRow(1002, true)]
        public void CheckRegisterClient_Test(int clientId, bool result)
        {
            _stockService.RegisterClient(clientId);
            bool isRegistered = _registerClients.GetClients().ContainsKey(clientId);
            Assert.AreEqual(isRegistered, result);
        }

        [TestMethod]
        [DataRow(0, 0, 0, true, false)]
        [DataRow(0, 0, 0, false, false)]
        [DataRow(1, 120, 30, true, true)]
        [DataRow(1, 120, 30, true, true)]
        public void CheckPlaceOrder_Test(int clientId, double price, int quantity, bool isBuy, bool result)
        {
            _stockService.RegisterClient(clientId);
            var data = ObjFactory.Instance.CreatePlaceOrderData();
            data.ClientId = clientId;
            data.Price = price;
            data.Quantity = quantity;
            data.IsBuy = isBuy;
            _stockService.PlaceOrder(data);
            Thread.Sleep(2000);
            if (isBuy)
            {
                bool isOrderPlaced = _order.GetOrderData().BuyPendingOrders.Contains(data);
                Assert.AreEqual(isOrderPlaced, result);
            }
            else
            {
                bool isOrderPlaced = _order.GetOrderData().SellPendingOrders.Contains(data);
                Assert.AreEqual(isOrderPlaced, result);
            }
            Thread.Sleep(1000);
        }

        #endregion Test Methods
    }
}