using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Statements.Expressions;

internal class ProgramExpression : Expression
{
    public IList<Statement> Body { get; set; }
    public override bool RequireSemicolon => false;

    public ProgramExpression(SourceInfo source)
        : base(source)
    {
        Body = [];
    }
}
