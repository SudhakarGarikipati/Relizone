namespace StockService.Application.Repositories
{
    public interface IStockServiceRepository
    {
        bool CheckStockAvailability(int productId, int quantity);

        bool ReserveStock(int productId, int quantity);

        bool UpdateStock(int productId, int quantity);
    }
}
