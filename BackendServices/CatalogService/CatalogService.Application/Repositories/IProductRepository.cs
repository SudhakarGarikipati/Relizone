using CatalogService.Domain.Entities;

namespace CatalogService.Application.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();

        Product GetById(int id);

        void Add(Product product);

        void Update(Product product);

        IEnumerable<Product> GetByIds(int [] ids);

        void Delete(int id);

        int SaveChanges();

    }
}
