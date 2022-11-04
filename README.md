# ApiFirstMediatR
Generates Controllers, DTOs and MediatR Requests from a given OpenAPI Spec file to support API First development. Business logic implementation is handled by MediatR handlers that implement the generated MediatR Requests.

Currently supports ASP.NET Core 6.0 and OpenAPI Spec version 3 and 2 in both yaml and json formats.

## How to use it
Add the following to your `.csproj`:
```xml
    <ItemGroup>
        <PackageReference Include="ApiFirstMediatR.Generator" Version="1.0.0-alpha-1" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />
    </ItemGroup>

    <ItemGroup>
        <AdditionalFiles Include="api_spec.json" />
    </ItemGroup>
```

## Viewing the generated files
### Visual Studio
In the solution explorer expand Project -> Dependencies -> Analyzers -> ApiFirstMediatR.Generator -> ApiFirstMediatR.Generator.ApiSourceGenerator.

### Rider
In the explorer expand Project -> Dependencies -> .NET 6.0 -> Source Generators -> ApiFirstMediatR.Generator.ApiSourceGenerator

### VSCode
Add the following to your `.csproj`
```xml
    <!-- Begin VSCode Compatibility -->
    <PropertyGroup>
        <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
        <CompilerGeneratedFilesOutputPath>Generated</CompilerGeneratedFilesOutputPath>
    </PropertyGroup>
    <ItemGroup>
        <Compile Remove="$(CompilerGeneratedFilesOutputPath)/**/*.cs" />
    </ItemGroup>

    <Target Name="CleanSourceGeneratedFiles" BeforeTargets="BeforeRebuild" DependsOnTargets="$(BeforeBuildDependsOn)">
        <RemoveDir Directories="$(CompilerGeneratedFilesOutputPath)" />
    </Target>
    <!-- End VSCode Compatibility -->
```

This will force the generators to output the generated files to the `Generated` directory, but notify the compiler to ignore the generated files and continue to use the normal Roslyn Source Generator compile process. Adding the Generated directory to your `.gitignore` is recommended.
