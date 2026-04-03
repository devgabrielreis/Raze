using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Operations;

internal readonly record struct BinaryOperationKey(
    RuntimeType LeftType,
    string Operator,
    RuntimeType RightType
);

internal delegate void BinaryOperation(
    ref readonly RuntimeValue left,
    ref readonly RuntimeValue right,
    out RuntimeValue result,
    ref readonly SourceInfo source
);

internal readonly record struct UnaryOperationKey(
    RuntimeType OperandType,
    string Operator,
    bool IsPostfix
);

internal delegate void UnaryOperation(
    ref readonly RuntimeValue operand,
    out RuntimeValue result,
    ref readonly SourceInfo source
);

internal sealed class OperationDispatcher
{
    private readonly Dictionary<BinaryOperationKey, BinaryOperation> _binaryOperations;
    private readonly Dictionary<UnaryOperationKey, UnaryOperation> _unaryOperations;

    internal OperationDispatcher()
    {
        _binaryOperations = new Dictionary<BinaryOperationKey, BinaryOperation>();
        _unaryOperations = new Dictionary<UnaryOperationKey, UnaryOperation>();
    }

    internal void RegisterFrom<TOperationRegistrar>() where TOperationRegistrar: IOperationRegistrar
    {
        TOperationRegistrar.RegisterBinaryOperations(this);
        TOperationRegistrar.RegisterUnaryOperations(this);
    }

    internal void ExecuteBinaryOperation(
        ref readonly RuntimeValue left,
        string op,
        ref readonly RuntimeValue right,
        out RuntimeValue target,
        ref readonly SourceInfo source
    )
    {
        var key = new BinaryOperationKey(left.Type, op, right.Type);

        if (!_binaryOperations.TryGetValue(key, out var operationFunc))
        {
            ThrowHelper.Throw<UnsupportedBinaryOperationException>(
                $"Cannot perform {left.Type} {op} {right.Type}",
                in source
            );
        }

        operationFunc(in left, in right, out target, in source);
    }

    internal void ExecuteUnaryOperation(
        ref readonly RuntimeValue operand,
        string op,
        out RuntimeValue target,
        bool isPostfix,
        ref readonly SourceInfo source
    )
    {
        var key = new UnaryOperationKey(operand.Type, op, isPostfix);

        if (!_unaryOperations.TryGetValue(key, out var operationFunc))
        {
            var operation = isPostfix ? $"{operand.Type}{op}" : $"{op}{operand.Type}";
            ThrowHelper.Throw<UnsupportedUnaryOperationException>(
                $"Cannot perform {operation}", in source
            );
        }

        operationFunc(in operand, out target, in source);
    }

    internal void RegisterBinaryOperation(BinaryOperationKey key, BinaryOperation operation)
    {
        if (_binaryOperations.ContainsKey(key))
        {
            throw new InvalidOperationException("A value with this key already exists");
        }

        _binaryOperations.Add(key, operation);
    }

    internal void RegisterUnaryOperation(UnaryOperationKey key, UnaryOperation operation)
    {
        if (_unaryOperations.ContainsKey(key))
        {
            throw new InvalidOperationException("A value with this key already exists");
        }

        _unaryOperations.Add(key, operation);
    }
}
