using Raze.Script.Core.Symbols;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class FunctionType : RuntimeType
{
    protected override string BaseTypeName => GetTypeName();

    internal RuntimeType ReturnType { get; private set; }

    internal IReadOnlyList<ParameterSymbol> Parameters { get; private set; }

    public FunctionType(bool isNullable, RuntimeType returnType, IReadOnlyList<ParameterSymbol> parameters)
        : base(isNullable)
    {
        ReturnType = returnType;
        Parameters = parameters;
    }

    public override bool Equals(RuntimeType? other)
    {
        if (other is not FunctionType)
        {
            return false;
        }

        if (other.IsNullable != IsNullable)
        {
            return false;
        }

        FunctionType otherAsFunc = (other as FunctionType)!;

        if (!otherAsFunc.ReturnType.Equals(ReturnType))
        {
            return false;
        }

        return otherAsFunc.Parameters.SequenceEqual(Parameters);
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        if (value is FunctionValue functionValue)
        {
            return functionValue.ReturnType.Equals(ReturnType) && functionValue.Parameters.SequenceEqual(Parameters);
        }

        return false;
    }

    private string GetTypeName()
    {
        List<string> generics = Parameters.Select(p => p.Type.TypeName).ToList();
        generics.Add(ReturnType.TypeName);

        return $"function<${string.Join(", ", generics)}>";
    }
}
