using Application.Interfaces.Services;
using HandlebarsDotNet;
using System.Reflection;

namespace Infrastructure.Templates
{
    public class EmbeddedTemplateRenderer : ITemplateRenderer
    {
        private static readonly Assembly _applicationAssembly = typeof(ITemplateRenderer).Assembly;

        public string Render(string templateName, object model)
        {
            HandlebarsTemplate<object, object> template = Handlebars.Compile(GetSource(templateName));

            return template(model);
        }

        public string GetSource(string templateName)
        {
            string resourceName = $"Application.Templates.Documents.{templateName}.hbs";

            using Stream stream = _applicationAssembly.GetManifestResourceStream(resourceName)
                ?? throw new FileNotFoundException($"Template '{templateName}' não encontrado como embedded resource.");

            using StreamReader reader = new(stream);

            return reader.ReadToEnd();
        }
    }
}
