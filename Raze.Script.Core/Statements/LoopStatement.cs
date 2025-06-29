using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Statements;

internal class LoopStatement : Statement
{
    public IReadOnlyList<Statement> Initialization { get; }

    public Expression? Condition { get; }
    
    public Statement? Update { get; }

    public CodeBlockStatement Body { get; }

    public override bool RequireSemicolon => false;

    public LoopStatement(List<Statement> initialization, Expression? condition, Statement? update, CodeBlockStatement body, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Initialization = initialization;
        Condition = condition;
        Update = update;
        Body = body;
    }
}
