using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Types;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols;

public class VariableSymbol : Symbol
{
    public RuntimeValue Value => IsInitialized
                                    ? _value!
                                    : throw new UninitializedVariableException(null, null);

    public bool IsConstant { get; private set; }

    public bool IsInitialized => _value != null;

    public RuntimeType Type { get; private set; }

    private RuntimeValue? _value;

    public VariableSymbol(RuntimeValue? value, RuntimeType type, bool isConstant)
        : this(value, type, isConstant, null, null)
    {
    }

    internal VariableSymbol(RuntimeValue? value, RuntimeType type, bool isConstant, int? sourceLine, int? sourceColumn)
    {
        _value = null;
        Type = type;
        IsConstant = false;

        if (value != null)
        {
            SetValue(value, sourceLine, sourceColumn);
        }

        IsConstant = isConstant;

        if (isConstant && !IsInitialized)
        {
            throw new UninitializedConstantException(sourceLine, sourceColumn);
        }
    }

    public void SetValue(RuntimeValue newValue)
    {
        SetValue(newValue, null, null);
    }

    internal void SetValue(RuntimeValue newValue, int? sourceLine, int? sourceColumn)
    {
        if (IsConstant)
        {
            throw new ConstantAssignmentException(sourceLine, sourceColumn);
        }

        if (!Type.Accept(newValue))
        {
            throw new VariableTypeException(newValue.GetType().Name, Type.TypeName, sourceLine, sourceColumn);
        }

        _value = newValue.Clone() as RuntimeValue;
    }
}
