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

internal readonly record struct UnaryOperationKey(
    string OperandTypeName,
    Type Operator,
    bool IsPostfix
);

internal delegate RuntimeValue UnaryOperation(
    RuntimeValue operand,
    SourceInfo source
);

internal class OperationDispatcher
{
    private readonly Dictionary<BinaryOperationKey, BinaryOperation> _binaryOperations;
    private readonly Dictionary<UnaryOperationKey, UnaryOperation> _unaryOperations;

    public OperationDispatcher()
    {
        _binaryOperations = new Dictionary<BinaryOperationKey, BinaryOperation>();
        _unaryOperations = new Dictionary<UnaryOperationKey, UnaryOperation>();
    }

    public void RegisterFrom<TOperationRegistrar>() where TOperationRegistrar: IOperationRegistrar
    {
        TOperationRegistrar.RegisterBinaryOperations(this);
        TOperationRegistrar.RegisterUnaryOperations(this);
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

    public RuntimeValue ExecuteUnaryOperation(RuntimeValue operand, OperatorToken op, bool isPostfix, SourceInfo source)
    {
        var key = new UnaryOperationKey(operand.TypeName, op.GetType(), isPostfix);

        if (!_unaryOperations.TryGetValue(key, out var operationFunc))
        {
            throw new UnsupportedUnaryOperationException(
                op.Lexeme, operand.TypeName, isPostfix, source
            );
        }

        return operationFunc(operand, source);
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

    public void RegisterUnaryOperation(UnaryOperationKey key, UnaryOperation operation)
    {
        if (!typeof(OperatorToken).IsAssignableFrom(key.Operator))
        {
            throw new ArgumentException($"Operator must be an {nameof(OperatorToken)}");
        }

        if (_unaryOperations.ContainsKey(key))
        {
            throw new InvalidOperationException("A value with this key already exists");
        }

        _unaryOperations.Add(key, operation);
    }
}
