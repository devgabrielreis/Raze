namespace Raze.Script.Core.Runtime.Values;

public abstract class RuntimeValue : ICloneable
{
    public abstract object Value { get; }

    public abstract string TypeName { get; }

    public abstract override string ToString();

    public abstract object Clone();
}
