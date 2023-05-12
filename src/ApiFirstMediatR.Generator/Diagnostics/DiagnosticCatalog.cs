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

    private static readonly DiagnosticDescriptor ApiSpecFeatureNotSupportedDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiSpecFeatureNotSupported,
        title: "API spec feature not supported",
        messageFormat: "API spec feature not supported: {0}",
        category: Category,
        DiagnosticSeverity.Warning,
        isEnabledByDefault: true
    );
    
    private static readonly DiagnosticDescriptor ApiFirstMediatRUnexpectedErrorDescriptor = new
    (
        id: DiagnosticIdentifiers.ApiFirstMediatRUnexpectedError,
        title: "Unexpected error occured while generating API interface",
        messageFormat: "Unexpected error occured while generating API interface: {0}",
        category: Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor InvalidSerializationLibraryDescriptor = new
    (
        id: DiagnosticIdentifiers.InvalidSerializationLibrary,
        title: "Unsupported JSON Serialization Library Selected",
        messageFormat: "Unsupported JSON Serialization Library Selected: {0}",
        category: Category,
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
    
    private static readonly DiagnosticDescriptor InvalidOperationGenerationModeDescriptor = new
    (
        id: DiagnosticIdentifiers.InvalidOperationGenerationMode,
        title: "Unsupported Operation Generation Mode Selected",
        messageFormat: "Unsupported Operation Generation Mode Selected: {0}",
        category: Category,
        DiagnosticSeverity.Error,
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

    public static Diagnostic ApiSpecFeatureNotSupported(Location location, string errorMessage)
        => Diagnostic.Create(ApiSpecFeatureNotSupportedDescriptor, location, errorMessage);

    public static Diagnostic ApiFirstMediatRUnexpectedError(Location location, string errorMessage)
        => Diagnostic.Create(ApiFirstMediatRUnexpectedErrorDescriptor, location, errorMessage);
    
    public static Diagnostic InvalidSerializationLibrary(Location location, string serializationLibrary)
        => Diagnostic.Create(InvalidSerializationLibraryDescriptor, location, serializationLibrary);
    
    public static Diagnostic InvalidOperationGenerationMode(Location location, string operationGenerationMode)
        => Diagnostic.Create(InvalidOperationGenerationModeDescriptor, location, operationGenerationMode);
}