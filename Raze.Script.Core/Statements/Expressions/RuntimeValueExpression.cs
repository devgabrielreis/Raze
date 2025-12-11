using Raze.Script.Core.Metadata;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Statements.Expressions;

internal class RuntimeValueExpression : Expression
{
    internal RuntimeValue Value { get; private set; }

    internal RuntimeValueExpression(RuntimeValue value, SourceInfo source)
        : base(source)
    {
        Value = value;
    }
}
