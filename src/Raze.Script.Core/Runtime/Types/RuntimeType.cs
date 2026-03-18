using Raze.Script.Core.Constants;
using Raze.Script.Core.Runtime.Values;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Types;

internal sealed class RuntimeType
{
    public static readonly RuntimeType Integer = TypeFactory.GetType(BaseType.Integer, false);
    public static readonly RuntimeType Decimal = TypeFactory.GetType(BaseType.Decimal, false);
    public static readonly RuntimeType Boolean = TypeFactory.GetType(BaseType.Boolean, false);
    public static readonly RuntimeType String = TypeFactory.GetType(BaseType.String, false);

    public static readonly RuntimeType NullableInteger = TypeFactory.GetType(BaseType.Integer, true);
    public static readonly RuntimeType NullableDecimal = TypeFactory.GetType(BaseType.Decimal, true);
    public static readonly RuntimeType NullableBoolean = TypeFactory.GetType(BaseType.Boolean, true);
    public static readonly RuntimeType NullableString = TypeFactory.GetType(BaseType.String, true);

    public static readonly RuntimeType Void = TypeFactory.GetType(BaseType.Void, false);
    public static readonly RuntimeType Null = TypeFactory.GetType(BaseType.Null, true);

    internal BaseType Base { get; }
    internal bool IsNullable { get; }
    internal RuntimeType[] Generics { get; }

    private readonly int _hashCode;

    internal RuntimeType(BaseType baseType, bool isNullable, RuntimeType[] generics)
    {
        Base = baseType;
        Generics = generics;
        IsNullable = isNullable;

        _hashCode = CalculateHash();
    }

    internal bool IsCompatibleWith(ref readonly RuntimeValue value)
    {
        if (IsNullable && value.Type == RuntimeType.Null)
        {
            return true;
        }

        return (Base == value.Type.Base &&
                Generics.SequenceEqual(value.Type.Generics));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override string ToString()
    {
        var typeName = Base switch
        {
            BaseType.Integer => TypeNames.INTEGER_TYPE_NAME,
            BaseType.Decimal => TypeNames.DECIMAL_TYPE_NAME,
            BaseType.Boolean => TypeNames.BOOLEAN_TYPE_NAME,
            BaseType.String => TypeNames.STRING_TYPE_NAME,
            BaseType.UserFunction => $"{TypeNames.FUNCTION_TYPE_NAME}<{GetGenericsString()}>",
            BaseType.Null => TypeNames.NULL_TYPE_NAME,
            BaseType.Void => TypeNames.VOID_TYPE_NAME,
            _ => $"<{Base}>"
        };

        if (IsNullable)
        {
            typeName += Operators.QUESTION_MARK;
        }

        return typeName;
    }

    public override int GetHashCode()
    {
        return _hashCode;
    }

    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is RuntimeType other &&
                                           _hashCode == other._hashCode &&
                                           Base == other.Base &&
                                           IsNullable == other.IsNullable &&
                                           Generics.SequenceEqual(other.Generics));
    }

    private string GetGenericsString()
    {
        var genericsStringList = Generics.Select(g => g.ToString()).ToList();

        return string.Join(", ", genericsStringList);
    }

    private int CalculateHash()
    {
        var hash = new HashCode();

        hash.Add(Base);
        hash.Add(IsNullable);
        foreach (var gen in Generics)
        {
            hash.Add(gen);
        }

        return hash.ToHashCode();
    }
}
