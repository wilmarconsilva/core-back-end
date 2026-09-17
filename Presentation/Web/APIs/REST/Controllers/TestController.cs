using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;

namespace REST.Controllers
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