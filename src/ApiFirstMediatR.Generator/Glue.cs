namespace ApiFirstMediatR.Generator;

public class Glue : IConfiguration
{
    public IEnumerable<IToken> Apply(IMutableContainer container)
    {
        // Generators
        yield return container
            .Bind<IApiGenerator>().Tag(1).To<DataTransferObjectGenerator>()
            .Bind<IApiGenerator>().Tag(2).To<ControllerGenerator>()
            .Bind<IApiGenerator>().Tag(3).To<MediatorRequestGenerator>();
            
        // Repositories
        yield return container
            .Bind<IApiSpecRepository>().As(Lifetime.ContainerSingleton).To<ApiSpecRepository>()
            .Bind<IRepository<DataTransferObject>>().As(Lifetime.ContainerSingleton).To<ModelRepository<DataTransferObject>>()
            .Bind<IRepository<Controller>>().As(Lifetime.ContainerSingleton).To<ModelRepository<Controller>>();
            
        // Mappers
        yield return container
            .Bind<DataTransferObjectMapper, IDataTransferObjectMapper, IOpenApiDocumentMapper<DataTransferObject>>().To()
            .Bind<ControllerMapper, IControllerMapper, IOpenApiDocumentMapper<Controller>>().To()
            .Bind<IEndpointMapper>().To<EndpointMapper>()
            .Bind<IParameterMapper>().To<ParameterMapper>()
            .Bind<IPropertyMapper>().To<PropertyMapper>()
            .Bind<IResponseMapper>().To<ResponseMapper>()
            .Bind<ITypeMapper>().To<TypeMapper>();
    }
}