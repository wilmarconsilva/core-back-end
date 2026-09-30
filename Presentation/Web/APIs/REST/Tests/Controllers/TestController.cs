using Domain.Shared.Persistance.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace REST.Tests.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Shared.Controllers.Controller
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