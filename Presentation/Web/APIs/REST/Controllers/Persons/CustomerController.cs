using Application.DataTransferObjects.Persons.Customers;
using Application.Interfaces.Services.Persons;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers.Persons
{
    [ApiController]
    [Route("[controller]")]
    public class CustomersController : CrudController<CustomerSaveDto>
    {
        public CustomersController(ICustomerService service, ILogger<CustomersController> logger, IUnitOfWork unitOfWork) : base(service, logger, unitOfWork) { }
    }
}