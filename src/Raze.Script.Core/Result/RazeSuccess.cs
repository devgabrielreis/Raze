using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Result;

public sealed class RazeSuccess : RazeResult
{
    public BaseType ValueType { get; }

    public string ValueTypeString { get; }

    public string ValueString { get; }

    private readonly object? _value;

    internal RazeSuccess(RuntimeValue value)
        : base(true)
    {
        ValueType = value.Type.Base;
        ValueTypeString = value.Type.ToString();
        ValueString = value.ToString();

        _value = value.Type.Base switch
        {
            BaseType.Integer => value.AsInteger(),
            BaseType.Decimal => value.AsDecimal(),
            BaseType.Boolean => value.AsBoolean(),
            BaseType.String  => value.AsString(),
            _ => null
        };
    }

    public Int128 AsInteger()
    {
        if (ValueType != BaseType.Integer)
        {
            throw new InvalidOperationException("This value is not an integer");
        }
        return (Int128)_value!;
    }

    public decimal AsDecimal()
    {
        if (ValueType != BaseType.Decimal)
        {
            throw new InvalidOperationException("This value is not a decimal");
        }
        return (decimal)_value!;
    }

    public bool AsBoolean()
    {
        if (ValueType != BaseType.Boolean)
        {
            throw new InvalidOperationException("This value is not a boolean");
        }
        return (bool)_value!;
    }

    public string AsString()
    {
        if (ValueType != BaseType.String)
        {
            throw new InvalidOperationException("This value is not a string");
        }
        return (string)_value!;
    }
}
