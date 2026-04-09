using Raze.Script.Core.Exceptions;

namespace Raze.Script.Core.Runtime.Values;

internal sealed class RazeFunctionParameters
{
    private readonly Dictionary<string, object?> _parameters;

    internal RazeFunctionParameters()
    {
        _parameters = [];
    }

    internal void Add(string name, object value)
    {
        if (!_parameters.TryAdd(name, value))
        {
            ThrowHelper.ThrowInvalidOperationException($"Parameter \"{name}\" was already added");
        }
    }

    internal object? Get(string name)
    {
        if (!_parameters.TryGetValue(name, out var value))
        {
            ThrowHelper.ThrowInvalidOperationException($"Parameter \"{name}\" was not found");
        }

        return value;
    }
}
