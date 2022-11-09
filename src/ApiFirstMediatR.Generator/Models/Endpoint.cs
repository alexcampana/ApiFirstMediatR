namespace ApiFirstMediatR.Generator.Models;

internal sealed class Endpoint
{
    public string? Name { get; set; }
    public string? Path { get; set; }
    public string? OperationName { get; set; }
    public string? MediatorRequestName { get; set; }
    public IEnumerable<string>? Description { get; set; }
    public IEnumerable<string>? ResponseDescription { get; set; }
    public string? ResponseBodyType { get; set; }
    public HttpStatusCode? MainHttpResponseType { get; set; }
    public Parameter? RequestBody { get; set; }
    public IEnumerable<Parameter>? QueryParameters { get; set; }
    public IEnumerable<Parameter>? PathParameters { get; set; }
    public IEnumerable<Parameter> AllRequestParameters
    {
        get
        {
            var props = new List<Parameter>();
            props.AddRange(QueryParameters ?? Enumerable.Empty<Parameter>());
            props.AddRange(PathParameters ?? Enumerable.Empty<Parameter>());

            if (RequestBody is not null)
                props.Add(RequestBody);

            return props;
        }
    }
}