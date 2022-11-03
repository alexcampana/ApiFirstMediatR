namespace ApiFirstMediatR.Generator.Diagnostics;

internal static class DiagnosticCatalog
{
    private const string Category = "APIFirstMediatR";

    private static readonly DiagnosticDescriptor ApiSpecFileNotFoundDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFileNotFound,
        title: "Couldn't find an API spec file",
        messageFormat: "Couldn't find an API spec file, did you add the API spec file as an AdditionalFile in your .csproj?", // TODO: Point to documentation
        category: Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    private static readonly DiagnosticDescriptor ApiSpecFileEmptyDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFileEmpty,
        title: "API spec file is empty",
        messageFormat: "API spec file is empty",
        category: Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor ApiSpecFileParsingErrorDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFileParsingError,
        title: "Parsing error with API spec file",
        messageFormat: "Parsing error with API spec file: {0}",
        category: Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    internal static readonly DiagnosticDescriptor ApiMissingImplementationDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiMissingImplementation,
        title: "Missing MediatR Handler Implementation",
        messageFormat: "Missing MediatR Handler Implementation: {0}",
        category: Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );

    public static Diagnostic ApiSpecFileNotFound()
        => Diagnostic.Create(ApiSpecFileNotFoundDescriptor, Location.None);
    
    public static Diagnostic ApiSpecFileEmpty(Location location)
        => Diagnostic.Create(ApiSpecFileEmptyDescriptor, location);

    public static Diagnostic ApiSpecFileParsingError(Location location, string errorMessage)
        => Diagnostic.Create(ApiSpecFileParsingErrorDescriptor, location, errorMessage);

    public static Diagnostic ApiMissingImplementation(Location location, string implementation)
        => Diagnostic.Create(ApiMissingImplementationDescriptor, location, implementation);
}