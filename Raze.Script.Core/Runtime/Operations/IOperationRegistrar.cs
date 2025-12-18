namespace Raze.Script.Core.Runtime.Operations;

internal interface IOperationRegistrar
{
    static abstract void RegisterBinaryOperations(OperationDispatcher dispatcher);
}
