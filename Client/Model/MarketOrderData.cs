namespace Client.Model
{
    public class MarketOrderData
    {
        public int MyBidQuantity { get; set; }
        public int MrktBidQuantity { get; set; }
        public int MyAskQuantity { get; set; }
        public int MrktAskQuantity { get; set; }
        public double Price { get; set; }
    }
}