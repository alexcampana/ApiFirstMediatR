namespace ApiFirstMediatR.Generator.Models;

internal sealed class Endpoint
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? OperationName { get; set; }
    public string? MediatorRequestName { get; set; }
    public IEnumerable<Parameter>? QueryParameters { get; set; }
    public string? RequestBodyType { get; set; }
    public IEnumerable<Parameter>? PathParameters { get; set; }
    public IEnumerable<Parameter> AllRequestParameters
    {
        get
        {
            var props = new List<Parameter>();
            props.AddRange(QueryParameters ?? Enumerable.Empty<Parameter>());
            props.AddRange(PathParameters ?? Enumerable.Empty<Parameter>());

            if (RequestBodyType is not null)
            {
                props.Add(new Parameter
                {
                    ParameterName = "body", // TODO: Make this configurable by end user
                    Name = "Body",
                    DataType = $"{RequestBodyType}",
                    Attribute = "[FromBody]"
                });
            }

            return props;
        }
    }
}