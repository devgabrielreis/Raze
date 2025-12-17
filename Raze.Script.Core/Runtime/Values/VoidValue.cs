using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Runtime.Values;

public class VoidValue : RuntimeValue
{
    public override object Value => this;

    public override string TypeName => TypeNames.VOID_TYPE_NAME;

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        throw new UnsupportedBinaryOperationException(
            TypeName, other.TypeName, op.Lexeme, source.SourceInfo
        );
    }

    public override string ToString()
    {
        return TypeName;
    }

    public override object Clone()
    {
        return new VoidValue();
    }
}
