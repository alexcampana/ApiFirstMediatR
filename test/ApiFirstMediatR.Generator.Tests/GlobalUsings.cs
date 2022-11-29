global using System.Collections.Immutable;
global using System.Reflection;
global using ApiFirstMediatR.Generator.Constants;
global using ApiFirstMediatR.Generator.Diagnostics;
global using ApiFirstMediatR.Generator.Interfaces;
global using ApiFirstMediatR.Generator.Mappers;
global using ApiFirstMediatR.Generator.Models;
global using ApiFirstMediatR.Generator.Repositories;
global using ApiFirstMediatR.Generator.Tests.Assertions;
global using ApiFirstMediatR.Generator.Tests.Utils;
global using FluentAssertions;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp;
global using Microsoft.CodeAnalysis.Text;
global using Microsoft.OpenApi;
global using Microsoft.OpenApi.Expressions;
global using Microsoft.OpenApi.Models;
global using Moq;
global using Xunit;