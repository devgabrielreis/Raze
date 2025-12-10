using Raze.Script.Core.Metadata;
using Raze.Script.Core.Symbols;
using Raze.Script.Core.Types;

namespace Raze.Script.Core.Statements;

internal class FunctionDeclarationStatement : Statement
{
    public string Identifier { get; private set; }

    public RuntimeType ReturnType { get; private set; }

    public IReadOnlyList<ParameterSymbol> Parameters { get; private set; }

    public CodeBlockStatement Body { get; private set; }

    public override bool RequireSemicolon => false;

    public FunctionDeclarationStatement(
        string identifier,
        RuntimeType returnType,
        IReadOnlyList<ParameterSymbol> parameters,
        CodeBlockStatement body,
        SourceInfo source
    )
        : base(source)
    {
        Identifier = identifier;
        ReturnType = returnType;
        Parameters = parameters;
        Body = body;
    }
}
