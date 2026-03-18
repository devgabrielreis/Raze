using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Symbols;

internal class VariableSymbol : Symbol
{
    internal bool IsConstant { get; }

    internal bool IsInitialized { get; private set; }

    internal RuntimeType Type { get; }

    private RuntimeValue _value;

    internal VariableSymbol(
        RuntimeValue? value,
        RuntimeType type,
        bool isConstant,
        ref readonly SourceInfo source
    )
    {
        if (type == RuntimeType.Void || type == RuntimeType.Null)
        {
            ThrowInvalidSymbolDeclarationException(type, in source);
        }

        Type = type;
        IsConstant = false;
        IsInitialized = false;

        if (value != null)
        {
            var refValue = value.Value;
            SetValue(in refValue, in source);
        }

        IsConstant = isConstant;

        if (isConstant && !IsInitialized)
        {
            ThrowUninitializedConstantException(in source);
        }
    }

    internal ref readonly RuntimeValue GetValue(ref readonly SourceInfo source)
    {
        if (!IsInitialized)
        {
            ThrowUninitializedVariableException(in source);
        }

        return ref _value;
    }

    internal void SetValue(ref readonly RuntimeValue newValue, ref readonly SourceInfo source)
    {
        if (IsConstant)
        {
            ThrowConstantAssignmentException(in source);
        }

        if (!Type.IsCompatibleWith(in newValue))
        {
            ThrowVariableTypeException(in newValue, in source);
        }

        _value = newValue;
        IsInitialized = true;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowInvalidSymbolDeclarationException(RuntimeType type, ref readonly SourceInfo source)
    {
        throw new InvalidSymbolDeclarationException($"Variable cannot have {type} type", source);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowUninitializedConstantException(ref readonly SourceInfo source)
    {
        throw new UninitializedConstantException(source);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowConstantAssignmentException(ref readonly SourceInfo source)
    {
        throw new ConstantAssignmentException(source);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private void ThrowVariableTypeException(ref readonly RuntimeValue value, ref readonly SourceInfo source)
    {
        throw new VariableTypeException(value.Type.ToString(), Type.ToString(), source);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowUninitializedVariableException(ref readonly SourceInfo source)
    {
        throw new UninitializedVariableException(source);
    }
}
