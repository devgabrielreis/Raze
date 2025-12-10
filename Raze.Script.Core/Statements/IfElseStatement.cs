using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class IfElseStatement : Statement
{
    public Expression Condition { get; }
    public CodeBlockStatement Then { get; }
    public Statement? Else { get; }
    public override bool RequireSemicolon => false;

    public IfElseStatement(Expression condition, CodeBlockStatement then, Statement? elseStmt, SourceInfo source)
        : base(source)
    {
        Condition = condition;
        Then = then;
        Else = elseStmt;
    }
}
