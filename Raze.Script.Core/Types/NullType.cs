using Raze.Script.Core.AST;
using Raze.Script.Core.Exceptions;

namespace Raze.Script.Core.Types;

public class NullType : RuntimeType
{
    public override object? Value => null;

    public override string TypeName => "NULL";

    public override RuntimeType ExecuteBinaryOperation(string op, RuntimeType other, BinaryExpression source)
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
