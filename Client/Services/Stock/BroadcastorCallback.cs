using Client.Constants;
using Client.ServerStockService;
using GalaSoft.MvvmLight.Messaging;
using System.Threading.Tasks;

namespace Client.Services.Stock
{
    public class BroadcastorCallback : IStockServiceCallback
    {
        #region Public Methods

        public void BroadcastPriceToClient(StockData eventData)
        {
            if (eventData != null)
            {
                Task.Run(() => Messenger.Default.Send(eventData, MessengerToken.BROADCASTSTOCKPRICE));
            }
        }

        public void BroadcastTradeDataToClient(TradeOrderData eventData)
        {
            if (eventData != null)
            {
                Task.Run(() => Messenger.Default.Send(eventData, MessengerToken.BROADCASTTRADEDATA));
            }
        }

        public void BroadCastMarketOrderBookData(MarketOrderBookData eventData)
        {
            if (eventData != null)
            {
                Task.Run(() => Messenger.Default.Send(eventData, MessengerToken.BROADCASTMARKETORDERBOOK));
            }
        }

        #endregion Public Methods
    }
}