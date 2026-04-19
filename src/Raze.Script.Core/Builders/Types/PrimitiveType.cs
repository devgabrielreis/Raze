using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Builders.Types;

internal sealed class PrimitiveType : TypeBuilder
{
    internal BaseType BaseType { get; }

    internal PrimitiveType(BaseType baseType)
    {
        BaseType = baseType;
    }

    internal override void UpdateBlueprint(ref TypeBlueprint blueprint)
    {
        blueprint.BaseType = BaseType;
    }
}
