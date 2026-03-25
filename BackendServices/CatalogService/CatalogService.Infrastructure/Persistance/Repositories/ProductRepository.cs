using CatalogService.Application.Repositories;
using CatalogService.Domain.Entities;

namespace CatalogService.Infrastructure.Persistance.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly CatalogServiceDbContext _context;

        public ProductRepository(CatalogServiceDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Product product)
        {
            // This method should add a new product to the database.
            _context.Products.Add(product);
        }

        public void Delete(int id)
        {
            // This method should delete a product by its ID.
            if (id <= 0)
            {
                throw new ArgumentException("Invalid product ID", nameof(id));
            }
            var p = _context.Products.Find(id);
            if (p != null)
            {
                _context.Products.Remove(p);
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid product ID", nameof(id));
            }
            return _context.Products.Find(id);
        }

        public IEnumerable<Product> GetByIds(int[] ids)
        {
            return _context.Products.Where(p => ids.Contains(p.ProductId)).ToList();
        }

        public int SaveChanges()
        {
           return _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
        }
    }
}
