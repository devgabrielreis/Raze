namespace Raze.Script.Core.Builders.Types;

internal sealed class NullableType : TypeBuilder
{
    internal TypeBuilder InnerType { get; }

    internal NullableType(TypeBuilder innerType)
    {
        InnerType = innerType;
    }

    internal override void UpdateBlueprint(ref TypeBlueprint blueprint)
    {
        blueprint.IsNullable = true;
        InnerType.UpdateBlueprint(ref blueprint);
    }
}
