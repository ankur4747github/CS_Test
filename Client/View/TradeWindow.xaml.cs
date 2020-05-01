using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows;

namespace Client
{
    public partial class TradeWindow : Window
    {
        StockService.StockServiceClient _client;
        private delegate void HandleBroadcastCallback(object sender, EventArgs e);

        public void HandleBroadcast(object sender, EventArgs e)
        {
            try
            {
                var eventData = (StockService.StockData)sender;
                this.Dispatcher.BeginInvoke(new Action(() => {
                    txtStockPrice.Text = eventData.StockPrice.ToString();
                }));
            }
            catch (Exception ex)
            {
            }
        }

        private void RegisterClient()
        {
            if ((this._client != null))
            {
                this._client.Abort();
                this._client = null;
            }

            BroadcastorCallback cb = new BroadcastorCallback();
            cb.SetHandler(this.HandleBroadcast);

            System.ServiceModel.InstanceContext context =
                new System.ServiceModel.InstanceContext(cb);
            this._client =
                new StockService.StockServiceClient(context);

            this._client.RegisterClient("Ankur");
        }

        public TradeWindow()
        {
            InitializeComponent();
            RegisterClient();
        }
    }

    public class BroadcastorCallback : StockService.IStockServiceCallback
    {
        private System.Threading.SynchronizationContext syncContext =
            AsyncOperationManager.SynchronizationContext;

        private EventHandler _broadcastorCallBackHandler;
        public void SetHandler(EventHandler handler)
        {
            this._broadcastorCallBackHandler = handler;
        }

        public void BroadcastToClient(StockService.StockData eventData)
        {
            syncContext.Post(new System.Threading.SendOrPostCallback(OnBroadcast),
                   eventData);
        }

        private void OnBroadcast(object eventData)
        {
            this._broadcastorCallBackHandler.Invoke(eventData, null);
        }
    }
}