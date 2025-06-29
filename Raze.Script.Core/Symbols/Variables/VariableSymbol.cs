using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Symbols.Variables;

public abstract class VariableSymbol : Symbol
{
    public abstract RuntimeValue? Value { get; }

    public bool IsConstant { get; private set; }

    public bool IsNullable { get; private set; }

    public bool IsInitialized { get; private set; }

    private int? _declarationLine;
    private int? _declarationColumn;

    public abstract string VariableTypeName { get; }

    public VariableSymbol(RuntimeValue? value, bool isConstant, bool isNullable)
    {
        Initialize(value, isConstant, isNullable, null, null);
    }

    internal VariableSymbol(RuntimeValue? value, bool isConstant, bool isNullable, int? sourceLine, int? sourceColumn)
    {
        Initialize(value, isConstant, isNullable, sourceLine, sourceColumn);
    }

    private void Initialize(RuntimeValue? value, bool isConstant, bool isNullable, int? sourceLine, int? sourceColumn)
    {
        _declarationLine = sourceLine;
        _declarationColumn = sourceColumn;
        IsNullable = isNullable;
        IsInitialized = false;
        IsConstant = false;

        if (value != null)
        {
            SetNewValue(value, sourceLine, sourceColumn);
        }

        IsConstant = isConstant;
    }

    public virtual void SetNewValue(RuntimeValue newValue)
    {
        SetNewValue(newValue, null, null);
    }

    internal virtual void SetNewValue(RuntimeValue newValue, int? sourceLine, int? sourceColumn)
    {
        if (IsConstant)
        {
            throw new ConstantAssignmentException(sourceLine, sourceColumn);
        }

        // dentro de loops, como é feito o hoisting da declaracao de variaveis, é possivel
        // que a variavel seja acessada antes da linha onde ela é inicializada
        if (!IsInitialized && _declarationLine.HasValue && _declarationColumn.HasValue)
        {
            bool isBeforeDeclaration =
                (sourceLine < _declarationLine) ||
                (sourceLine == _declarationLine && sourceColumn < _declarationColumn);

            if (isBeforeDeclaration)
            {
                throw new UninitializedVariableException(sourceLine, sourceColumn);
            }
        }

        if (!IsNullable && (newValue is NullValue || newValue.Value is null))
        {
            throw new VariableTypeException(nameof(NullValue), VariableTypeName, sourceLine, sourceColumn);
        }

        IsInitialized = true;

        SetValue(newValue, sourceLine, sourceColumn);
    }

    protected abstract void SetValue(RuntimeValue value, int? sourceLine, int? sourceColumn);
}
