using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Scopes;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Symbols;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Values;

public class FunctionValue : RuntimeValue
{
    public override object Value => this;

    public override string TypeName => GetTypeName();

    internal RuntimeType ReturnType { get; private set; }

    internal IReadOnlyList<ParameterSymbol> Parameters { get; private set; }

    internal CodeBlockStatement Body { get; private set; }

    internal Scope Scope { get; private set; } 

    internal FunctionValue(RuntimeType returnType, IReadOnlyList<ParameterSymbol> parameters, CodeBlockStatement body, Scope scope)
    {
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
        Scope = scope;
    }

    internal override RuntimeValue ExecuteBinaryOperation(OperatorToken op, RuntimeValue other, BinaryExpression source)
    {
        throw new UnsupportedBinaryOperationException(
            TypeName, other.TypeName, op.Lexeme, source.SourceInfo
        );
    }

    public override string ToString()
    {
        return GetTypeName();
    }

    public override object Clone()
    {
        return new FunctionValue(ReturnType, Parameters, Body, Scope);
    }

    private string GetTypeName()
    {
        List<string> generics = Parameters.Select(p => p.Type.TypeName).ToList();
        generics.Add(ReturnType.TypeName);

        return $"{TypeNames.FUNCTION_TYPE_NAME}<{string.Join(", ", generics)}>";
    }
}
