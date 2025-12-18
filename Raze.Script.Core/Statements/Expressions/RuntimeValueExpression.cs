using Raze.Script.Core.Engine;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Statements.Expressions;

internal class RuntimeValueExpression : Expression
{
    internal RuntimeValue Value { get; private set; }

    internal RuntimeValueExpression(RuntimeValue value, SourceInfo source)
        : base(source)
    {
        Value = value;
    }

    internal override TResult AcceptVisitor<TState, TResult>(IStatementVisitor<TState, TResult> visitor, TState state)
    {
        return visitor.VisitRuntimeValueExpression(this, state);
    }
}
