using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Types;

public class NullType : RuntimeType
{
    public override object? Value => null;

    public override string TypeName => "NULL";

    internal override RuntimeType ExecuteBinaryOperation(OperatorToken op, RuntimeType other, BinaryExpression source)
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
        return "NULL";
    }
}
