using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Builders.Types;

internal abstract class TypeBuilder
{
    internal static TypeBuilder Integer => new PrimitiveType(BaseType.Integer);
    internal static TypeBuilder Decimal => new PrimitiveType(BaseType.Decimal);
    internal static TypeBuilder Boolean => new PrimitiveType(BaseType.Boolean);
    internal static TypeBuilder String => new PrimitiveType(BaseType.String);

    internal TypeBuilder AsNullable()
    {
        return new NullableType(this);
    }

    internal RuntimeType Build()
    {
        var blueprint = new TypeBlueprint();

        UpdateBlueprint(ref blueprint);

        if (blueprint.BaseType == null)
        {
            ThrowHelper.ThrowInvalidOperationException("The primitive type was not set");
        }

        return TypeFactory.GetType(blueprint.BaseType.Value, blueprint.IsNullable);
    }

    internal abstract void UpdateBlueprint(ref TypeBlueprint blueprint);
}
