using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Tokens;

internal static class TokenTypeFacts
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrimitiveTypeKeyword(this TokenType type)
    {
        return type is TokenType.IntegerTypeName
                    or TokenType.DecimalTypeName
                    or TokenType.BooleanTypeName
                    or TokenType.StringTypeName
                    or TokenType.FunctionTypeName
                    or TokenType.VoidTypeName;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAssignmentOrCompoundAssignmentOperator(this TokenType type)
    {
        return type == TokenType.Assignment || type.IsCompoundAssignmentOperator();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsCompoundAssignmentOperator(this TokenType type)
    {
        return type is TokenType.AdditionAssignment
                    or TokenType.SubtractionAssignment
                    or TokenType.MultiplicationAssignment
                    or TokenType.DivisionAssignment
                    or TokenType.ModuloAssignment;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEqualityOperator(this TokenType type)
    {
        return type is TokenType.Equal
                    or TokenType.NotEqual;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsRelationalOperator(this TokenType type)
    {
        return type is TokenType.GreaterThan
                    or TokenType.LessThan
                    or TokenType.GreaterEqual
                    or TokenType.LessEqual;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsAdditiveOperator(this TokenType type)
    {
        return type is TokenType.Plus
                    or TokenType.Minus;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsMultiplicativeOperator(this TokenType type)
    {
        return type is TokenType.Multiplication
                    or TokenType.Division
                    or TokenType.Modulo;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUnaryOperator(this TokenType type)
    {
        return type is TokenType.Not
                    or TokenType.Plus
                    or TokenType.Minus;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPrefixOperator(this TokenType type)
    {
        return type is TokenType.Increment
                    or TokenType.Decrement;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsPostfixOperator(this TokenType type)
    {
        return type is TokenType.Increment
                    or TokenType.Decrement
                    or TokenType.NullChecker;
    }
}
