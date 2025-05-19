using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Types;

public abstract class RuntimeType
{
    public abstract object? Value { get; }

    public abstract string TypeName { get; }

    internal abstract RuntimeType ExecuteBinaryOperation(string op, RuntimeType other, BinaryExpression source);

    public abstract override string ToString();
}
