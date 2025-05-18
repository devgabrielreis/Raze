namespace Raze.Script.Core.Statements.Expressions;

public class BinaryExpression : Expression
{
    public Expression Left { get; private set; }
    public string Operator { get; private set; }
    public Expression Right { get; private set; }

    public BinaryExpression(Expression left, string op, Expression right, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}
