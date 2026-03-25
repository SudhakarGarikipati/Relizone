using StockService.Application.Repositories;

namespace StockService.Infrastructure.Persistance.Repositories
{
    public class StockServiceRepository : IStockServiceRepository
    {
        readonly StockServiceDbContext _context;
        public StockServiceRepository(StockServiceDbContext context)
        {
            _context = context;
        }

        public bool CheckStockAvailability(int productId, int quantity)
        {
          return _context.Stocks.Any(s=>s.ProductId == productId && s.Quantity >= quantity);
        }

        public bool ReserveStock(int productId, int quantity)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.ProductId == productId);
            if (stock != null)
            {
                stock.Quantity -= quantity;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateStock(int productId, int quantity)
        {
            var stock = _context.Stocks.FirstOrDefault(s => s.ProductId == productId);
            if (stock != null)
            {
                stock.Quantity += quantity;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
