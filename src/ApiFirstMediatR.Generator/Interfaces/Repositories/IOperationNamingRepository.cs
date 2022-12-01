namespace ApiFirstMediatR.Generator.Interfaces.Repositories;

internal interface IOperationNamingRepository
{
    string? GetControllerNameByPath(string path);
    string? GetOperationNameByPathAndOperationType(string path, OperationType operationType);
    string? GetOperationNameByOperationId(string operationId);
    string? GetControllerNameByOperationId(string operationId);
}