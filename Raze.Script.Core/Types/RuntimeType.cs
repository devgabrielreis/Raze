using Raze.Script.Core.Values;

namespace Raze.Script.Core.Types;

public abstract class RuntimeType : IEquatable<RuntimeType>
{
    public bool IsNullable { get; private set; }

    public string TypeName => $"{(IsNullable ? "Nullable" : "")}{BaseTypeName}";

    protected abstract string BaseTypeName { get; }

    public RuntimeType(bool isNullable)
    {
        IsNullable = isNullable;
    }

    public bool Accept(RuntimeValue value)
    {
        if (value is NullValue)
        {
            return IsNullable;
        }

        return AcceptNonNull(value);
    }

    public abstract bool Equals(RuntimeType? other);

    protected abstract bool AcceptNonNull(RuntimeValue value);
}
