namespace ApiFirstMediatR.Generator.Models;

internal sealed class ApiConfig
{
    public string? Namespace { get; set; }
    public string DtoNamespace => $"{Namespace}.Dtos";
    public string RequestNamespace => $"{Namespace}.Requests";
    public string ControllerNamespace => $"{Namespace}.Controllers";
}