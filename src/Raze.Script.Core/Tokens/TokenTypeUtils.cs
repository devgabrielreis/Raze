using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Tokens;

internal static class TokenTypeUtils
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static string GetCompoundAssignmentTokenOperator(this TokenType type)
    {
        return type switch
        {
            TokenType.AdditionAssignment       => Operators.PLUS,
            TokenType.SubtractionAssignment    => Operators.MINUS,
            TokenType.MultiplicationAssignment => Operators.MULTIPLICATION,
            TokenType.DivisionAssignment       => Operators.DIVISION,
            TokenType.ModuloAssignment         => Operators.MODULO,
            _ => ThrowHelper.ThrowArgumentOutOfRangeException<string>(
                $"The method \"{nameof(GetCompoundAssignmentTokenOperator)}\" does not support this token: {type}"
            )
        };
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static string GetFriendlyName(this TokenType type)
    {
        return type switch
        {
            TokenType.Identifier => "identifier",

            TokenType.NamespaceDeclaration => "namespace declaration keyword",
            TokenType.Import               => "import keyword",
            TokenType.FunctionDeclaration  => "function declaration keyword",
            TokenType.Return               => "return keyword",
            TokenType.Const                => "constant declaration keyword",
            TokenType.Var                  => "variable declaration keyword",

            TokenType.Plus           => "addition operator",
            TokenType.Minus          => "subtraction operator",
            TokenType.Multiplication => "multiplication operator",
            TokenType.Division       => "division operator",
            TokenType.Modulo         => "modulo operator",

            TokenType.Equal        => "equality operator",
            TokenType.NotEqual     => "inequality operator",
            TokenType.GreaterThan  => "greater than operator",
            TokenType.LessThan     => "less than operator",
            TokenType.GreaterEqual => "greater than or equal operator",
            TokenType.LessEqual    => "less than or equal operator",

            TokenType.And => "AND operator",
            TokenType.Or  => "OR operator",
            TokenType.Not => "NOT operator",

            TokenType.Assignment               => "assignment operator",
            TokenType.AdditionAssignment       => "addition assignment operator",
            TokenType.SubtractionAssignment    => "subtraction assignment operator",
            TokenType.MultiplicationAssignment => "multiplication assignment operator",
            TokenType.DivisionAssignment       => "division assignment operator",
            TokenType.ModuloAssignment         => "modulo assignment operator",

            TokenType.Increment         => "increment operator",
            TokenType.Decrement         => "decrement operator",
            TokenType.NullChecker       => "null-check operator",
            TokenType.QuestionMark      => "question mark",
            TokenType.NamespaceAccessor => "namespace accessor",

            TokenType.BooleanTypeName  => "boolean type keyword",
            TokenType.IntegerTypeName  => "integer type keyword",
            TokenType.DecimalTypeName  => "decimal type keyword",
            TokenType.StringTypeName   => "string type keyword",
            TokenType.FunctionTypeName => "function type keyword",
            TokenType.VoidTypeName     => "void type keyword",

            TokenType.BooleanLiteral => "boolean literal",
            TokenType.IntegerLiteral => "integer literal",
            TokenType.DecimalLiteral => "decimal literal",
            TokenType.StringLiteral  => "string literal",
            TokenType.NullLiteral    => "null literal",

            TokenType.If       => "if keyword",
            TokenType.Else     => "else keyword",
            TokenType.For      => "for loop keyword",
            TokenType.While    => "while loop keyword",
            TokenType.Break    => "break keyword",
            TokenType.Continue => "continue keyword",

            TokenType.Comma            => "comma",
            TokenType.SemiColon        => "semicolon",
            TokenType.EOF              => "end of file",
            TokenType.OpenBraces       => "opening brace",
            TokenType.CloseBraces      => "closing brace",
            TokenType.OpenParenthesis  => "opening parenthesis",
            TokenType.CloseParenthesis => "closing parenthesis",

            _ => ThrowHelper.ThrowArgumentOutOfRangeException<string>(
                $"The method \"{nameof(GetFriendlyName)}\" does not support this token: {type}"
            )
        };
    }
}
