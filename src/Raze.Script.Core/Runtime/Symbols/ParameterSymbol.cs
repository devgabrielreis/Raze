using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Statements.Expressions;

namespace Raze.Script.Core.Runtime.Symbols;

internal sealed class ParameterSymbol
{
    internal readonly bool IsConstant;

    internal readonly RuntimeType Type;

    internal readonly string Identifier;

    internal readonly Expression? DefaultValue;

    internal readonly SourceInfo SourceInfo;

    internal ParameterSymbol(
        bool isConstant,
        RuntimeType type,
        string identifier,
        ref readonly SourceInfo source,
        RuntimeValue? defaultValue
    )
        : this(
            isConstant,
            type,
            identifier,
            (defaultValue is null ? null : new RuntimeValueExpression(defaultValue.Value, in source)),
            in source
        )
    {
    }

    internal ParameterSymbol(
        bool isConstant,
        RuntimeType type,
        string identifier,
        Expression? defaultValue,
        ref readonly SourceInfo source
    )
    {
        if (type == RuntimeType.Void || type == RuntimeType.Null)
        {
            ThrowHelper.Throw<InvalidSymbolDeclarationException>(
                $"Parameter cannot have {type} type",
                in source
            );
        }

        IsConstant = isConstant;
        Type = type;
        Identifier = identifier;
        DefaultValue = defaultValue;
        SourceInfo = source;
    }

    internal bool HasDefaultValue()
    {
        return DefaultValue is not null;
    }
}
