using Application.DataTransferObjects.Products;
using Application.Interfaces.Services;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Persons
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : CrudController<ProductSaveDto>
    {
        public ProductsController(IProductService service, ILogger<ProductsController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork) { }
    }
}