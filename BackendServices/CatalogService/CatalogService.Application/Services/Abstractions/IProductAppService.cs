using CatalogService.Application.DTOs;

namespace CatalogService.Application.Services.Abstractions
{
    public interface IProductAppService
    {
        IEnumerable<ProductDto> GetAll();

        ProductDto GetById(int id);

        void Add(ProductDto product);

        void Update(ProductDto product);

        IEnumerable<ProductDto> GetByIds(int[] ids);

        void Delete(int id);
    }
}
