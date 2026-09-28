using Application.Interfaces.Services;
using Application.Products.DataTransferObjects;
using Application.Products.DataTransferObjects.Listings;
using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using REST.Shared.Controllers;

namespace REST.Products.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : CrudController<ProductSaveDto, ProductDetailDto, ProductListing, ProductListingFilter>
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service, ILogger<ProductsController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork)
        {
            _service = service;
        }

        [HttpGet]
        public override async Task<ActionResult<List<ProductListing>>> GetAll([FromQuery] ProductListingFilter filter, CancellationToken cancellationToken)
        {
            return await ProcessRequestAsync<List<ProductListing>>(async () =>
            {
                List<ProductListing> listings = await _service.GetAllAsync(filter, cancellationToken);
                return Ok(listings);
            }, cancellationToken);
        }
    }
}