using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Runtime.Operations;

internal readonly record struct BinaryOperationKey(
    string LeftTypeName,
    Type Operator,
    string RightTypeName
);

internal delegate RuntimeValue BinaryOperation(
    RuntimeValue left,
    RuntimeValue right,
    SourceInfo source
);

internal class OperationDispatcher
{
    private readonly Dictionary<BinaryOperationKey, BinaryOperation> _binaryOperations;

    public OperationDispatcher()
    {
        _binaryOperations = new Dictionary<BinaryOperationKey, BinaryOperation>();
    }

    public void RegisterFrom<TOperationRegistrar>() where TOperationRegistrar: IOperationRegistrar
    {
        TOperationRegistrar.RegisterBinaryOperations(this);
    }

    public RuntimeValue ExecuteBinaryOperation(RuntimeValue left, OperatorToken op, RuntimeValue right, SourceInfo source)
    {
        var key = new BinaryOperationKey(left.TypeName, op.GetType(), right.TypeName);

        if (!_binaryOperations.TryGetValue(key, out var operationFunc))
        {
            throw new UnsupportedBinaryOperationException(
                left.TypeName, right.TypeName, op.Lexeme, source
            );
        }

        return operationFunc(left, right, source);
    }

    public void RegisterBinaryOperation(BinaryOperationKey key, BinaryOperation operation)
    {
        if (!typeof(OperatorToken).IsAssignableFrom(key.Operator))
        {
            throw new ArgumentException($"Operator must be an {nameof(OperatorToken)}");
        }

        if (_binaryOperations.ContainsKey(key))
        {
            throw new InvalidOperationException("A value with this key already exists");
        }

        _binaryOperations.Add(key, operation);
    }
}
