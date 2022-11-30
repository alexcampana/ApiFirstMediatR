namespace ApiFirstMediatR.Generator.Interfaces.Repositories;

internal interface IRepository<out T>
{
    IEnumerable<T> Get();
}