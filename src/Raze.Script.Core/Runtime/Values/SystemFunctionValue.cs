using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Runtime.Values;

internal sealed class SystemFunctionValue
{
    internal readonly RuntimeType ReturnType;

    internal readonly IReadOnlyList<ParameterSymbol> Parameters;

    internal readonly Func<RazeFunctionParameters, RazeFunctionReturnValue> Body;

    internal SystemFunctionValue(
        RuntimeType returnType,
        IReadOnlyList<ParameterSymbol> parameters,
        Func<RazeFunctionParameters, RazeFunctionReturnValue> body
    )
    {
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }
}
