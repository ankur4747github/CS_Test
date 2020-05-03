namespace Client.Services.Stock
{
    public interface IStockService
    {
        bool Register(int clientId);

        bool PlaceOrder(ServerStockService.PlaceOrderData data);
    }
}