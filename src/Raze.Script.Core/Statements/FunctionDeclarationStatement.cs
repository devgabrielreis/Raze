using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Script.Core.Statements;

internal sealed class FunctionDeclarationStatement : Statement
{
    internal string Identifier { get; private set; }

    internal RuntimeType ReturnType { get; private set; }

    internal IReadOnlyList<ParameterSymbol> Parameters { get; private set; }

    internal CodeBlockStatement Body { get; private set; }

    internal FunctionDeclarationStatement(
        string identifier,
        RuntimeType returnType,
        IReadOnlyList<ParameterSymbol> parameters,
        CodeBlockStatement body,
        SourceInfo source
    )
        : base(source, false)
    {
        Identifier = identifier;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitFunctionDeclarationStatement(this, state);
    }
}
