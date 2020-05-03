using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server.Factory;
using Server.Services.Stock;
using Server.StockServices;
using System.Threading;
using System.Threading.Tasks;

namespace ServerTest.Services.Stock
{
    [TestClass]
    public class RegisterClients_Tests
    {
        private IRegisterClients _registerClients;
        private IOrder _order;

        [TestInitialize]
        public void SetUp()
        {
            _registerClients = ObjFactory.Instance.CreateRegisterClients();
            _order = ObjFactory.Instance.CreateOrder();
        }

        [TestMethod]
        [DataRow(0, false)]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(210, true)]
        [DataRow(1002, true)]
        public void CheckRegisterClient_Test(int clientId, bool result)
        {
            _registerClients.RegisterClient(clientId);
            bool isRegistered = _registerClients.GetClients().ContainsKey(clientId);
            Assert.AreEqual(isRegistered, result);
        }

        [TestMethod]
        [DataRow(2, true)]
        [DataRow(2, true)]
        [DataRow(210, true)]
        [DataRow(1002, true)]
        [DataRow(0, false)]
        public void CheckUnRegisterClient_Test(int clientId, bool result)
        {
            _registerClients.RegisterClient(clientId);
            bool isRegistered = _registerClients.GetClients().ContainsKey(clientId);
            _registerClients.UnRegisterClient(new System.Collections.Generic.List<int> { clientId });
            bool isUnRegistered = !_registerClients.GetClients().ContainsKey(clientId);
            Assert.AreEqual(result, (isRegistered && isUnRegistered));

        }
    }
}
