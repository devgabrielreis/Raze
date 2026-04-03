using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Runtime.Symbols;

internal sealed class VariableSymbol
{
    internal readonly bool IsConstant;

    internal bool IsInitialized { get; private set; }

    internal readonly RuntimeType Type;

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
            ThrowHelper.Throw<InvalidSymbolDeclarationException>(
                $"Variable cannot have {type} type",
                in source
            );
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
            ThrowHelper.Throw<UninitializedConstantException>(
                "Cannot declare a constant without an initializer",
                in source
            );
        }
    }

    internal ref readonly RuntimeValue GetValue(ref readonly SourceInfo source)
    {
        if (!IsInitialized)
        {
            ThrowHelper.Throw<UninitializedVariableException>(
                "Trying to access variable before its initialization",
                in source
            );
        }

        return ref _value;
    }

    internal void SetValue(ref readonly RuntimeValue newValue, ref readonly SourceInfo source)
    {
        if (IsConstant)
        {
            ThrowHelper.Throw<ConstantAssignmentException>(
                "Cannot modify a constant value",
                in source
            );
        }

        if (!Type.IsCompatibleWith(in newValue))
        {
            ThrowHelper.Throw<VariableTypeException>(
                $"Trying to assign type {newValue.Type} to variable of type {Type}",
                in source
            );
        }

        _value = newValue;
        IsInitialized = true;
    }
}
