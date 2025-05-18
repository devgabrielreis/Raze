namespace Raze.Script.Core.Lexer;

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

    AssignmentOperator,
    AdditionOperator,
    SubtractionOperator,
    MultiplicationOperator,
    DivisionOperator,
    ModuloOperator
}
