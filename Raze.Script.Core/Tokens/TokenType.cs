namespace Raze.Script.Core.Tokens;

public enum TokenType
{
    EOF,
    SemiColon,

    OpenParenthesis,
    CloseParenthesis,

    Identifier,

    Var,
    IntegerType,

    IntegerLiteral,
    NullLiteral,

    AssignmentOperator,
    AdditionOperator,
    SubtractionOperator,
    MultiplicationOperator,
    DivisionOperator,
    ModuloOperator
}
