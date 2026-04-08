using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Types;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Raze.Script.Core.Runtime.Values;

[StructLayout(LayoutKind.Explicit)]
internal readonly struct RuntimeValue
{
    internal static readonly RuntimeValue True  = new(true);
    internal static readonly RuntimeValue False = new(false);
    internal static readonly RuntimeValue Null  = new(RuntimeType.Null);
    internal static readonly RuntimeValue Void  = new(RuntimeType.Void);

    // O _refValue e o Type devem ser alocados separados dos outros valores, já que eles
    // são ponteiros que o GC checa para limpar de tempos em tempos, sobrescreve-los
    // pode fazer com que o GC tente limpar um endereço inválido na memória
    [FieldOffset(0)] internal readonly RuntimeType Type;
    [FieldOffset(8)] private readonly object? _refValue;

    // Todos esses campos ocupam o mesmo espaço na memória para
    // economizar espaço, já que apenas um deles será usado por vez.
    // Sempre verifique o Type antes de acessar o valor
    [FieldOffset(16)] private readonly Int128 _integerValue;
    [FieldOffset(16)] private readonly decimal _decimalValue;
    [FieldOffset(16)] private readonly bool _boolValue;

    internal RuntimeValue(Int128 intValue)
    {
        _refValue = null;
        _decimalValue = default;
        _boolValue = default;

        Type = RuntimeType.Integer;
        _integerValue = intValue;
    }

    internal RuntimeValue(decimal decimalValue)
    {
        _refValue = null;
        _integerValue = default;
        _boolValue = default;

        Type = RuntimeType.Decimal;
        _decimalValue = decimalValue;
    }

    internal RuntimeValue(string stringValue)
    {
        _integerValue = default;
        _decimalValue = default;
        _boolValue = default;

        Type = RuntimeType.String;
        _refValue = stringValue;
    }

    internal RuntimeValue(UserFunctionValue userFunctionValue)
    {
        _integerValue = default;
        _decimalValue = default;
        _boolValue = default;

        var generics = userFunctionValue.Parameters.Select(p => p.Type).ToList();
        generics.Add(userFunctionValue.ReturnType);

        Type = TypeFactory.GetType(BaseType.UserFunction, false, generics.ToArray());
        _refValue = userFunctionValue;
    }

    private RuntimeValue(bool boolValue)
    {
        _refValue = null;
        _integerValue = default;
        _decimalValue = default;

        Type = RuntimeType.Boolean;
        _boolValue = boolValue;
    }

    private RuntimeValue(RuntimeType type)
    {
        _refValue = null;
        _integerValue = default;
        _decimalValue = default;
        _boolValue = default;

        Type = type;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Int128 AsInteger()
    {
        Debug.Assert(Type == RuntimeType.Integer);
        return _integerValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal decimal AsDecimal()
    {
        Debug.Assert(Type == RuntimeType.Decimal);
        return _decimalValue;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool AsBoolean()
    {
        Debug.Assert(Type == RuntimeType.Boolean);
        return _boolValue;
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override string ToString()
    {
        return Type.Base switch
        {
            BaseType.Integer      => _integerValue.ToString(),
            BaseType.Decimal      => _decimalValue.ToString(CultureInfo.InvariantCulture),
            BaseType.Boolean      => _boolValue ? Keywords.TRUE_LITERAL : Keywords.FALSE_LITERAL,
            BaseType.String       => $"\"{(string)_refValue!}\"",
            BaseType.UserFunction => Type.ToString(),
            BaseType.Null         => TypeNames.NULL_TYPE_NAME,
            BaseType.Void         => TypeNames.VOID_TYPE_NAME,
            _                     => $"<{Type}>"
        };
    }
}
