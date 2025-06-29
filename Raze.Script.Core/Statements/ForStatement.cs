using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class ForStatement : Statement
{
    public IReadOnlyList<Statement> Initialization { get; }

    public Expression? Condition { get; }
    
    public Statement? Update { get; }

    public CodeBlockStatement Body { get; }

    public override bool RequireSemicolon => false;

    public ForStatement(List<Statement> initialization, Expression? condition, Statement? update, CodeBlockStatement body, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
    }
}
