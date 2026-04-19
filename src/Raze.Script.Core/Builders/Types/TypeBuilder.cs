using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Builders.Types;

public abstract class TypeBuilder
{
    public static TypeBuilder Integer => new PrimitiveType(BaseType.Integer);
    public static TypeBuilder Decimal => new PrimitiveType(BaseType.Decimal);
    public static TypeBuilder Boolean => new PrimitiveType(BaseType.Boolean);
    public static TypeBuilder String  => new PrimitiveType(BaseType.String);
    public static TypeBuilder Void    => new PrimitiveType(BaseType.Void);

    public TypeBuilder AsNullable()
    {
        return new NullableType(this);
    }

    internal abstract void UpdateBlueprint(ref TypeBlueprint blueprint);

    internal RuntimeType Build()
    {
        var blueprint = new TypeBlueprint();

        UpdateBlueprint(ref blueprint);
        ValidateBlueprint(blueprint);
        
        return TypeFactory.GetType(blueprint.BaseType!.Value, blueprint.IsNullable);
    }

    private void ValidateBlueprint(TypeBlueprint blueprint)
    {
        if (blueprint.BaseType == null)
        {
            ThrowHelper.ThrowInvalidOperationException("The primitive type was not set");
        }

        if (blueprint.BaseType == BaseType.Void && blueprint.IsNullable)
        {
            ThrowHelper.ThrowInvalidOperationException("Void type cannot be nullable");
        }
    }
}
