using Client.Constants;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;

namespace Client.Services.Stock
{
    public class BroadcastorCallback : StockService.IStockServiceCallback
    {
        #region Public Methods

        public void BroadcastPriceToClient(StockService.StockData eventData)
        {
            Task.Run(() => BroadcastPriceToUI(eventData));
        }

        #endregion Public Methods

        #region Private Methods

        private void BroadcastPriceToUI(StockService.StockData eventData)
        {
            Messenger.Default.Send(eventData, MessengerToken.BROADCASTSTOCKPRICE);
        }

        #endregion Private Methods
    }
}