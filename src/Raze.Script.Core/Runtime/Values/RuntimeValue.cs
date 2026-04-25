using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Types;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Values;

internal readonly struct RuntimeValue
{
    internal static readonly RuntimeValue True  = new(true);
    internal static readonly RuntimeValue False = new(false);
    internal static readonly RuntimeValue Null  = new(RuntimeType.Null);
    internal static readonly RuntimeValue Void  = new(RuntimeType.Void);

    internal readonly RuntimeType Type;

    private readonly ValueUnion _value;
    private readonly object? _refValue;

    internal RuntimeValue(Int128 intValue)
    {
        _refValue = null;

        Type = RuntimeType.Integer;
        _value = new ValueUnion(intValue);
    }

    internal RuntimeValue(decimal decimalValue)
    {
        _refValue = null;

        Type = RuntimeType.Decimal;
        _value = new ValueUnion(decimalValue);
    }

    internal RuntimeValue(string stringValue)
    {
        _value = default;

        Type = RuntimeType.String;
        _refValue = stringValue;
    }

    internal RuntimeValue(UserFunctionValue userFunctionValue)
    {
        _value = default;

        var generics = userFunctionValue.Parameters.Select(p => p.Type).ToList();
        generics.Add(userFunctionValue.ReturnType);

        Type = TypeFactory.GetType(BaseType.UserFunction, false, generics.ToArray());
        _refValue = userFunctionValue;
    }

    internal RuntimeValue(SystemFunctionValue systemFunctionValue)
    {
        _value = default;

        var generics = systemFunctionValue.Parameters.Select(p => p.Type).ToList();
        generics.Add(systemFunctionValue.ReturnType);

        Type = TypeFactory.GetType(BaseType.SystemFunction, false, generics.ToArray());
        _refValue = systemFunctionValue;
    }

    private RuntimeValue(bool boolValue)
    {
        _refValue = null;

        Type = RuntimeType.Boolean;
        _value = new ValueUnion(boolValue);
    }

    private RuntimeValue(RuntimeType type)
    {
        _refValue = null;
        _value = default;

        Type = type;
    }

    internal object? AsObject()
    {
        return Type.Base switch
        {
            BaseType.Integer => AsInteger(),
            BaseType.Decimal => AsDecimal(),
            BaseType.Boolean => AsBoolean(),
            BaseType.String  => AsString(),
            _                => _refValue
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Int128 AsInteger()
    {
        Debug.Assert(Type == RuntimeType.Integer);
        return _value.IntegerValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal decimal AsDecimal()
    {
        Debug.Assert(Type == RuntimeType.Decimal);
        return _value.DecimalValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool AsBoolean()
    {
        Debug.Assert(Type == RuntimeType.Boolean);
        return _value.BoolValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal string AsString()
    {
        Debug.Assert(Type == RuntimeType.String);
        return (string)_refValue!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal UserFunctionValue AsUserFunction()
    {
        Debug.Assert(Type.Base == BaseType.UserFunction);
        return (UserFunctionValue)_refValue!;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal SystemFunctionValue AsSystemFunction()
    {
        Debug.Assert(Type.Base == BaseType.SystemFunction);
        return (SystemFunctionValue)_refValue!;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override string ToString()
    {
        return Type.Base switch
        {
            BaseType.Integer        => _value.IntegerValue.ToString(),
            BaseType.Decimal        => GetDecimalString(),
            BaseType.Boolean        => _value.BoolValue ? Keywords.TRUE_LITERAL : Keywords.FALSE_LITERAL,
            BaseType.String         => $"\"{(string)_refValue!}\"",
            BaseType.UserFunction   => Type.ToString(),
            BaseType.SystemFunction => Type.ToString(),
            BaseType.Null           => TypeNames.NULL_TYPE_NAME,
            BaseType.Void           => TypeNames.VOID_TYPE_NAME,
            _                       => $"<{Type}>"
        };
    }

    private string GetDecimalString()
    {
        Debug.Assert(Type == RuntimeType.Decimal);

        var decimalStr = _value.DecimalValue.ToString(CultureInfo.InvariantCulture);

        if (!decimalStr.Contains('.'))
        {
            decimalStr += ".0";
        }

        return decimalStr;
    }
}
