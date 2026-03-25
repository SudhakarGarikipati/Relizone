using StockService.Application.Repositories;
using StockService.Application.Services.Abstraction;

namespace StockService.Application.Services.Implementation
{
    public class StockAppService : IStockAppService
    {
        readonly IStockServiceRepository _stockServiceRepository;
        public StockAppService(IStockServiceRepository stockServiceRepository) {
            _stockServiceRepository = stockServiceRepository;
        }

        public bool CheckStockAvailibility(int productId, int quantity)
        {
           return _stockServiceRepository.CheckStockAvailability(productId, quantity);
        }

        public bool ReserveStock(int productId, int quantity)
        {
            return _stockServiceRepository.ReserveStock(productId, quantity);
        }

        public bool UpdateStock(int productId, int quantity)
        {
            return _stockServiceRepository.UpdateStock(productId, quantity);
        }
    }
}
