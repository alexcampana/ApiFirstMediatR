namespace ApiFirstMediatR.Generator.Constants;

internal static class HttpStatusCodes
{
    public static readonly HttpStatusCode Status200 = new HttpStatusCode(200, "Status200OK", "Ok");
    public static readonly HttpStatusCode Status201 = new HttpStatusCode(201, "Status201Created", "CreatedAtAction");
    public static readonly HttpStatusCode Status204 = new HttpStatusCode(204, "Status204NoContent", "NoContent");
}

internal class HttpStatusCode
{
    public HttpStatusCode(int code, string statusCodeReference, string responseMethod)
    {
        Code = code;
        StatusCodeReference = statusCodeReference;
        ResponseMethod = responseMethod;
    }

    public int Code { get; }
    public string StatusCodeReference { get; }
    public string ResponseMethod { get; }
}