using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Values;

public abstract class RuntimeValue : ICloneable
{
    public abstract object Value { get; }

    public abstract string TypeName { get; }

    internal abstract RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source);

    public abstract override string ToString();

    public abstract object Clone();
}
