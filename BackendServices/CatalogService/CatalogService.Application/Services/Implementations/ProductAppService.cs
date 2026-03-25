using AutoMapper;
using CatalogService.Application.DTOs;
using CatalogService.Application.Repositories;
using CatalogService.Application.Services.Abstractions;
using CatalogService.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace CatalogService.Application.Services.Implementations
{
    public class ProductAppService : IProductAppService
    {
        // This class is a placeholder for the actual implementation of the productDto application service.

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly string _imageServer;
        private readonly IConfiguration _configuraton;

        public ProductAppService(IProductRepository productRepository, IMapper mapper, IConfiguration configuraton)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _configuraton = configuraton;
            _imageServer = _configuraton["ImageServer"];
        }

        public void Add(ProductDto productDto)
        {
            //var entity = new Product
            //{
            //    Name = productDto.Name,
            //    Description = productDto.Description,
            //    UnitPrice = productDto.UnitPrice,
            //    ImageUrl = productDto.ImageUrl,
            //    CategoryId = productDto.CategoryId
            //};
            var entity = _mapper.Map<Product>(productDto);
            _productRepository.Add(entity);
            _productRepository.SaveChanges();
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

        public IEnumerable<ProductDto> GetAll()
        {
            var products = _productRepository.GetAll();
            if(products == null || !products.Any())
            {
                return [];
            }
            // Map the products to ProductDto and set the ImageUrl
            foreach (var product in products)
            {
                product.ImageUrl = $"{_imageServer}{product.ImageUrl}";
            }
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public ProductDto GetById(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return null; // or throw an exception, depending on your error handling strategy
            }
            product.ImageUrl = $"{_imageServer}{product.ImageUrl}";
            return _mapper.Map<ProductDto>(product);
        }

        public IEnumerable<ProductDto> GetByIds(int[] ids)
        {
            var products = _productRepository.GetByIds(ids);
            if (products == null || !products.Any())
            {
                return [];
            }
            // Map the products to ProductDto and set the ImageUrl
            foreach (var product in products)
            {
                product.ImageUrl = $"{_imageServer}{product.ImageUrl}";
            }
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }

        public void Update(ProductDto productDto)
        {
            var entity = _productRepository.GetById(productDto.ProductId);
            if (entity == null)
            {
                throw new ArgumentException("Product not found", nameof(productDto.ProductId));
            }
            _productRepository.Update(_mapper.Map<Product>(productDto));
        }
    }
}
