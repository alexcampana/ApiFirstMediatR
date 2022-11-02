namespace ApiFirstMediatR.Generator.Diagnostics;

internal static class DiagnosticCatalog
{
    private static readonly DiagnosticDescriptor ApiSpecFileNotFoundDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFileNotFound,
        title: "Couldn't find an API spec file",
        messageFormat: "Couldn't find an API spec file, did you add the API spec file as an AdditionalFile in your .csproj?", // TODO: Point to documentation
        category: "APIFirstMediatR",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    private static readonly DiagnosticDescriptor ApiSpecFileEmptyDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFileEmpty,
        title: "API spec file is empty",
        messageFormat: "API spec file is empty",
        category: "APIFirstMediatR",
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor ApiSpecFileParsingErrorDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFileParsingError,
        title: "Parsing error with API spec file",
        messageFormat: "Parsing error with API spec file: {errorMessage}",
        category: "APIFirstMediatR",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public static Diagnostic ApiSpecFileNotFound()
        => Diagnostic.Create(ApiSpecFileNotFoundDescriptor, Location.None);
    
    public static Diagnostic ApiSpecFileEmpty(Location location)
        => Diagnostic.Create(ApiSpecFileEmptyDescriptor, location);

    public static Diagnostic ApiSpecFileParsingError(Location location, string errorMessage)
        => Diagnostic.Create(ApiSpecFileParsingErrorDescriptor, location, errorMessage);
}