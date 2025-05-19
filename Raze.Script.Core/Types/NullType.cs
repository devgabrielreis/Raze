using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Types;

public class NullType : RuntimeType
{
    public override object? Value => null;

    public override string TypeName => "NULL";

    internal override RuntimeType ExecuteBinaryOperation(string op, RuntimeType other, BinaryExpression source)
    {
        throw new UnsupportedBinaryOperationException(
            TypeName,
            other.TypeName,
            op,
            source.StartLine,
            source.StartColumn
        );
    }

    public override string ToString()
    {
        return "NULL";
    }
}
