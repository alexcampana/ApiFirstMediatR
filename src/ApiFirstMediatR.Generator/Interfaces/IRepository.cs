namespace ApiFirstMediatR.Generator.Interfaces;

internal interface IRepository<out T>
{
    IEnumerable<T> Get();
}