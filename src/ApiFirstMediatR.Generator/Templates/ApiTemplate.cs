namespace ApiFirstMediatR.Generator.Templates;

internal static class ApiTemplate
{
    public static readonly ApiTemplate<Controller> Controller = new ("Templates/Controller.cs.liquid");
    public static readonly ApiTemplate<DataTransferObject> DataTransferObject = new ("Templates/DataTransferObject.cs.liquid");
}

internal class ApiTemplate<T>
{
    private readonly Template _template;
    
    public ApiTemplate(string value)
    {
        var indexTemplateFile = EmbeddedResource.GetContent(value);
        _template = Template.ParseLiquid(indexTemplateFile);
    }
    
    public string Generate(T contextObject)
    {
        return _template.Render(contextObject);
    }
}