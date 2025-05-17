namespace Raze.Script.Core.Lexer;

public enum TokenType
{
    EOF,
    SemiColon,

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
