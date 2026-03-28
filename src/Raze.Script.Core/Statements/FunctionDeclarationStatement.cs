using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Statements;

internal sealed class FunctionDeclarationStatement : Statement
{
    internal readonly string Identifier;

    internal readonly RuntimeType ReturnType;

    internal readonly IReadOnlyList<ParameterSymbol> Parameters;

    internal readonly CodeBlockStatement Body;

    internal FunctionDeclarationStatement(
        string identifier,
        RuntimeType returnType,
        IReadOnlyList<ParameterSymbol> parameters,
        CodeBlockStatement body,
        ref readonly SourceInfo source
    )
        : base(in source, false)
    {
        Identifier = identifier;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitFunctionDeclarationStatement(this, state, out result);
    }
}
