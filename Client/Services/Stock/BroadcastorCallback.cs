using Client.Constants;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;

namespace Client.Services.Stock
{
    public class BroadcastorCallback : ServerStockService.IStockServiceCallback
    {
        #region Public Methods

        public void BroadcastPriceToClient(ServerStockService.StockData eventData)
        {
            Task.Run(() => BroadcastPriceToUI(eventData));
        }

        #endregion Public Methods

        #region Private Methods

        private void BroadcastPriceToUI(ServerStockService.StockData eventData)
        {
            Messenger.Default.Send(eventData, MessengerToken.BROADCASTSTOCKPRICE);
        }

        #endregion Private Methods
    }
}