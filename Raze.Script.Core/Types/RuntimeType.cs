using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public abstract class RuntimeType : IEquatable<RuntimeType>
{
    public bool IsNullable { get; private set; }

    public string TypeName => $"{BaseTypeName}{(IsNullable ? "?" : "")}";

    protected abstract string BaseTypeName { get; }

    public RuntimeType(bool isNullable)
    {
        IsNullable = isNullable;
    }

    public bool AcceptValue(RuntimeValue value)
    {
        if (value is NullValue)
        {
            return IsNullable;
        }

        return AcceptNonNullValue(value);
    }

    public abstract bool Equals(RuntimeType? other);

    protected abstract bool AcceptNonNullValue(RuntimeValue value);
}
