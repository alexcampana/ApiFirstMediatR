namespace ApiFirstMediatR.Generator.Mappers;

internal static class ResponseMapper
{
    public static Response Map(OpenApiResponses responses)
    {
        if (responses.TryGetValue("200", out var successResponse))
        {
            if (successResponse.Content.TryGetValue("application/json", out var responseBody))
            {
                return new Response
                {
                    BodyType = TypeMapper.Map(responseBody.Schema),
                    Description = successResponse.Description.SplitOnNewLine(),
                    HttpStatusCode = HttpStatusCodes.Status200
                };
            }
            else
            {
                throw new NotImplementedException($"Only application/json response body supported.");
            }
        }
        else if (responses.TryGetValue("204", out successResponse))
        {
            return new Response
            {
                BodyType = null,
                Description = successResponse.Description.SplitOnNewLine(),
                HttpStatusCode = HttpStatusCodes.Status204
            };
        }
        else
        {
            throw new NotImplementedException($"Response Status not supported");
        }
    }
}