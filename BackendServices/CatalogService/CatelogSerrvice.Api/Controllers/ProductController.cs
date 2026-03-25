using CatalogService.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController: ControllerBase
    {
        readonly IProductAppService _productAppService;
        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService ?? throw new ArgumentNullException(nameof(productAppService));
        }

        [HttpGet]
        //public ActionResult<IEnumerable<ProductDto>> GetAll()
        public IActionResult GetAll()
        {
            var products = _productAppService.GetAll();
            if (products == null || !products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

    }
}
