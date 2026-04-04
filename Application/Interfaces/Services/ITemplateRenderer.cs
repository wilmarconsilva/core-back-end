namespace Application.Interfaces.Services
{
    public interface ITemplateRenderer
    {
        string Render(string templateName, object model);

        string GetSource(string templateName);
    }
}
