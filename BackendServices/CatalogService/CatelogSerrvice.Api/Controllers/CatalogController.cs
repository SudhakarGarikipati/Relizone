using CatalogService.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CatalogController : ControllerBase
    {
        readonly IProductAppService _productAppService;

        public CatalogController(IProductAppService productAppService)
        {
            _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productAppService.GetAll();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productAppService.GetById(id);
            if (product == null)
            {
                return NotFound($"Product with ID {id} not found.");
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult GetByIds(int[] ids)
        {
            var products = _productAppService.GetByIds(ids);
            if (products != null && products.Any())
            {
                return Ok(products);
            }
            return NotFound("No products found for the provided IDs.");
        }
    }
}
