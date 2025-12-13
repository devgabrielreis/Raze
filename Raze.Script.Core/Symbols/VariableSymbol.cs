using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Types;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols;

public class VariableSymbol : Symbol
{
    public RuntimeValue Value => IsInitialized
                                    ? _value!
                                    : throw new UninitializedVariableException(new SourceInfo("RazeInternals"));

    public bool IsConstant { get; private set; }

    public bool IsInitialized => _value != null;

    public RuntimeType Type { get; private set; }

    private RuntimeValue? _value;

    public VariableSymbol(RuntimeValue? value, RuntimeType type, bool isConstant, SourceInfo source)
    {
        if (type is VoidType)
        {
            throw new InvalidSymbolDeclarationException("Variable cannot have void type", source);
        }

        _value = null;
        Type = type;
        IsConstant = false;

        if (value != null)
        {
            SetValue(value, source);
        }

        IsConstant = isConstant;

        if (isConstant && !IsInitialized)
        {
            throw new UninitializedConstantException(source);
        }
    }

    public void SetValue(RuntimeValue newValue, SourceInfo source)
    {
        if (IsConstant)
        {
            throw new ConstantAssignmentException(source);
        }

        if (!Type.Accept(newValue))
        {
            throw new VariableTypeException(newValue.TypeName, Type.TypeName, source);
        }

        _value = newValue.Clone() as RuntimeValue;
    }
}
