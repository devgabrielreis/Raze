using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Statements;

namespace Raze.Script.Core.Runtime.Values;

internal sealed class UserFunctionValue
{
    internal RuntimeType ReturnType { get; }

    internal IReadOnlyList<ParameterSymbol> Parameters { get; }

    internal CodeBlockStatement Body { get; }

    internal Scope Scope { get; }

    internal UserFunctionValue(
        RuntimeType returnType,
        IReadOnlyList<ParameterSymbol> parameters,
        CodeBlockStatement body,
        Scope scope
    )
    {
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
        Scope = scope;
    }
}
