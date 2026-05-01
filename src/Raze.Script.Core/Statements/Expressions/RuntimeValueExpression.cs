using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Statements.Expressions;

internal sealed class RuntimeValueExpression : Expression
{
    internal readonly RuntimeValue Value;

    internal RuntimeValueExpression(RuntimeValue value, ref readonly SourceInfo source)
        : base(in source, true)
    {
        Value = value;
    }

    internal override void AcceptVisitor<TState, TResult>(
        IStatementVisitor<TState, TResult> visitor,
        TState state,
        out TResult result
    )
    {
        visitor.VisitRuntimeValueExpression(this, state, out result);
    }
}
