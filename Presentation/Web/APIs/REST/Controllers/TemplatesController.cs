using Application.Interfaces.Services;
using Domain.Interfaces.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace REST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TemplatesController : Controller
    {
        private readonly ITemplateRenderer _templateRenderer;

        public TemplatesController(ITemplateRenderer templateRenderer, ILogger<TemplatesController> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            _templateRenderer = templateRenderer;
        }

        [HttpGet("{templateName}")]
        public ActionResult<string> GetByName(string templateName)
        {
            try
            {
                string source = _templateRenderer.GetSource(templateName);
                return Content(source, "text/plain", Encoding.UTF8);
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
