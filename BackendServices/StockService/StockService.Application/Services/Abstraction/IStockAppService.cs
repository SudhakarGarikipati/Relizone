namespace StockService.Application.Services.Abstraction
{
    public interface IStockAppService
    {
        bool CheckStockAvailibility(int productId, int quantity);

        bool ReserveStock(int productId, int quantity);

        bool UpdateStock(int productId, int quantity);
    }
}
