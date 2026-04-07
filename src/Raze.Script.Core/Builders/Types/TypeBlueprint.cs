using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Builders.Types;

internal sealed class TypeBlueprint
{
    internal BaseType? BaseType { get; set; } = null;
    internal bool IsNullable { get; set; } = false;
}
