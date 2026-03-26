using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;
using Raze.Script.Core.Statements.Expressions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Symbols;

internal sealed class ParameterSymbol
{
    internal bool IsConstant { get; }

    internal RuntimeType Type { get; }

    internal string Identifier { get; }

    internal Expression? DefaultValue { get; }

    internal SourceInfo SourceInfo { get; }

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
            (defaultValue is null ? null : new RuntimeValueExpression(defaultValue.Value, source)),
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
            ThrowConstantAssignmentException(type, in source);
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    [DoesNotReturn]
    [StackTraceHidden]
    private static void ThrowConstantAssignmentException(RuntimeType type, ref readonly SourceInfo source)
    {
        throw new InvalidSymbolDeclarationException($"Parameter cannot have {type} type", source);
    }
}
