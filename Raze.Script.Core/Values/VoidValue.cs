using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Values;

public class VoidValue : RuntimeValue
{
    public override object Value => this;

    public override string TypeName => "Undefined";

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op.Lexeme,
            source.StartLine,
            source.StartColumn
        );
    }

    public override string ToString()
    {
        return "undefined";
    }

    public override object Clone()
    {
        return new VoidValue();
    }
}
