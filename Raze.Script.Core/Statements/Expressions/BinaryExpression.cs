﻿using Raze.Script.Core.Tokens.Operators;

namespace Raze.Script.Core.Statements.Expressions;

internal class BinaryExpression : Expression
{
    public Expression Left { get; private set; }
    public OperatorToken Operator { get; private set; }
    public Expression Right { get; private set; }

    public BinaryExpression(Expression left, OperatorToken op, Expression right, int startLine, int startColumn)
        : base(startLine, startColumn)
    {
        Left = left;
        Operator = op;
        Right = right;
    }
}
