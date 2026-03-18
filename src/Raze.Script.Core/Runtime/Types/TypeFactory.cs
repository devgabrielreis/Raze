namespace Raze.Script.Core.Runtime.Types;

internal static class TypeFactory
{
    private static readonly Dictionary<RuntimeType, RuntimeType> _cache = [];

    internal static RuntimeType GetType(BaseType baseType, bool isNullable, params RuntimeType[] generics)
    {
        var candidate = new RuntimeType(baseType, isNullable, generics);

        lock (_cache)
        {
            if (_cache.TryGetValue(candidate, out var existing))
            {
                return existing;
            }

            _cache.Add(candidate, candidate);
            return candidate;
        }
    }
}
