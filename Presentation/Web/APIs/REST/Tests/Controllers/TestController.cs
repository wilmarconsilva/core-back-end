using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;
using REST.Shared.Controllers;

namespace REST.Tests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        public TestController(ILogger logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {

        }

        /// <summary>
        /// Exclusive for internal Tests.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> TestAsync(CancellationToken cancellationToken)
        {
            return await this.ProcessRequestAsync(async () =>
            {
                return NoContent();
            }, cancellationToken);
        }
    }
}