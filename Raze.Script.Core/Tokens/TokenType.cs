namespace Raze.Script.Core.Tokens;

internal enum TokenType
{
    EOF,
    SemiColon,

    OpenParenthesis,
    CloseParenthesis,

    Identifier,

    KeywordVar,
    KeywordInteger,

    IntegerLiteral,
    NullLiteral,

    AssignmentOperator,
    AdditionOperator,
    SubtractionOperator,
    MultiplicationOperator,
    DivisionOperator,
    ModuloOperator
}
