using Raze.Script.Core.Symbols;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public class FunctionType : RuntimeType
{
    protected override string BaseTypeName => GetTypeName();

    internal RuntimeType ReturnType { get; private set; }

    internal IReadOnlyList<RuntimeType> ParameterTypes { get; private set; }

    public FunctionType(bool isNullable, RuntimeType returnType, IReadOnlyList<RuntimeType> parameterTypes)
        : base(isNullable)
    {
        ReturnType = returnType;
        ParameterTypes = parameterTypes;
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

        return otherAsFunc.ParameterTypes.SequenceEqual(ParameterTypes);
    }

    protected override bool AcceptNonNull(RuntimeValue value)
    {
        if (value is not FunctionValue)
        {
            return false;
        }

        var functionValue = (FunctionValue)value;

        if (!functionValue.ReturnType.Equals(ReturnType))
        {
            return false;
        }

        if (functionValue.Parameters.Count != ParameterTypes.Count)
        {
            return false;
        }

        return functionValue.Parameters
            .Zip(ParameterTypes)
            .All(pair => pair.First.Type.Equals(pair.Second));
    }

    private string GetTypeName()
    {
        List<string> generics = ParameterTypes.Select(p => p.TypeName).ToList();
        generics.Add(ReturnType.TypeName);

        return $"function<{string.Join(", ", generics)}>";
    }
}
