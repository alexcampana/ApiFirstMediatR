// Global using directives

global using System.Collections.Immutable;
global using ApiFirstMediatR.Generator.Constants;
global using ApiFirstMediatR.Generator.Diagnostics;
global using ApiFirstMediatR.Generator.Extensions;
global using ApiFirstMediatR.Generator.Generators;
global using ApiFirstMediatR.Generator.Interfaces;
global using ApiFirstMediatR.Generator.Mappers;
global using ApiFirstMediatR.Generator.Models;
global using ApiFirstMediatR.Generator.Repositories;
global using ApiFirstMediatR.Generator.Templates;
global using ApiFirstMediatR.Generator.Utils;
global using CaseExtensions;
global using IoC;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.Diagnostics;
global using Microsoft.OpenApi.Extensions;
global using Microsoft.OpenApi.Models;
global using Microsoft.OpenApi.Readers;
global using Scriban;
global using Scriban.Runtime;

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ApiFirstMediatR.Generator.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]