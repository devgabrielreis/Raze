using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class ProgramExpression : Expression
{
    public IReadOnlyList<Statement> Body { get; private set; }

    public override bool RequireSemicolon => false;

    public ProgramExpression(IReadOnlyList<Statement> body, SourceInfo source)
        : base(source)
    {
        Body = body;
    }
}
