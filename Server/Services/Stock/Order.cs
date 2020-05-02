using Server.Factory;
using Server.Model;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Services.Stock
{
    public class Order : IOrder
    {
        #region Fields

        private volatile bool _alreadyRunning;

        private ConcurrentQueue<PlaceOrderData> _unprocessed { get; set; }
        private Dictionary<double, Queue<PlaceOrderData>> _buyPendinfOrders { get; set; }
        private Dictionary<double, Queue<PlaceOrderData>> _sellPendinfOrders { get; set; }

        #endregion Fields

        #region Constructor

        public Order()
        {
            _unprocessed = new ConcurrentQueue<PlaceOrderData>();
            _buyPendinfOrders = new Dictionary<double, Queue<PlaceOrderData>>();
            _sellPendinfOrders = new Dictionary<double, Queue<PlaceOrderData>>();
        }

        #endregion Constructor

        #region Public Methods

        public void AddOrderIntoQueue(PlaceOrderData data)
        {
            _unprocessed.Enqueue(data);
            ObjFactory.Instance.CreateLogger()
                    .Log(string.Format("Order Enqueue Client Id = {0} Price = {1} Quantity= {2} type ={3}",
                    data.ClientId, data.Price, data.Quantity, data.IsBuy)
                    , this.GetType().Name, false);

            if (_unprocessed.Count > 0 && !_alreadyRunning)
            {
                _alreadyRunning = true;
                Task.Run(() => StartProcess_UnProcessedOrder());
            }
        }

        #endregion Public Methods

        #region Private Method

        private void StartProcess_UnProcessedOrder()
        {
            MatchOrders();
            if (_unprocessed.Count > 0)
            {
                StartProcess_UnProcessedOrder();
            }
            else
            {
                _alreadyRunning = false;
            }
        }

        private void MatchOrders()
        {
        }

        #endregion Private Method
    }
}