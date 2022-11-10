namespace ApiFirstMediatR.Generator;

public class Glue : IConfiguration
{
    public IEnumerable<IToken> Apply(IMutableContainer container)
    {
        yield return container
            .Bind<IApiGenerator>().To<ApiGenerator>()
            .Bind<IApiSpecRepository>().As(Lifetime.ContainerSingleton).To<ApiSpecRepository>()
            .Bind<IRepository<DataTransferObject>>().As(Lifetime.ContainerSingleton).To<ModelRepository<DataTransferObject>>()
            .Bind<IRepository<Controller>>().As(Lifetime.ContainerSingleton).To<ModelRepository<Controller>>()
            .Bind<IOpenApiDocumentMapper<DataTransferObject>>().To<DataTransferObjectMapper>()
            .Bind<IOpenApiDocumentMapper<Controller>>().To<ControllerMapper>()
            .Bind<IControllerMapper>().To<ControllerMapper>()
            .Bind<IDataTransferObjectMapper>().To<DataTransferObjectMapper>()
            .Bind<IEndpointMapper>().To<EndpointMapper>()
            .Bind<IParameterMapper>().To<ParameterMapper>()
            .Bind<IPropertyMapper>().To<PropertyMapper>()
            .Bind<IResponseMapper>().To<ResponseMapper>()
            .Bind<ITypeMapper>().To<TypeMapper>();
    }
}