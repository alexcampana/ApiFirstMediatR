namespace ApiFirstMediatR.Generator.Templates;

internal static class ApiTemplate
{
    public static readonly ApiTemplate<Controller> Controller = new ("Templates/Controller.cs.liquid");
    public static readonly ApiTemplate<DataTransferObject> DataTransferObject = new ("Templates/DataTransferObject.cs.liquid");
    public static readonly ApiTemplate<DataTransferObjectEnum> DataTransferObjectEnum = new ("Templates/DataTransferObjectEnum.cs.liquid");
    public static readonly ApiTemplate<Endpoint> MediatorRequest = new ("Templates/MediatorRequest.cs.liquid");
}

internal sealed class ApiTemplate<T>
{
    private readonly Template _template;
    
    public ApiTemplate(string value)
    {
        var indexTemplateFile = EmbeddedResource.GetContent(value);
        _template = Template.ParseLiquid(indexTemplateFile);
    }
    
    public string Generate(T contextObject, ApiConfig apiConfig)
    {
        var scriptObject = new ScriptObject();
        scriptObject.Import(apiConfig);
        scriptObject.Import(contextObject);

        var context = new LiquidTemplateContext();
        context.PushGlobal(scriptObject);
        
        return _template.Render(context);
    }
}