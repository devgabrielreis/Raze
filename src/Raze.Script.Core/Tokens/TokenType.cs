namespace Raze.Script.Core.Tokens;

internal enum TokenType : byte
{
    Identifier,
    NamespaceDeclaration,
    FunctionDeclaration,

    Return,

    Plus,
    Minus,
    Multiplication,
    Division,
    Modulo,

    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterEqual,
    LessEqual,

    And,
    Or,
    Not,

    Assignment,
    AdditionAssignment,
    SubtractionAssignment,
    MultiplicationAssignment,
    DivisionAssignment,
    ModuloAssignment,

    Increment,
    Decrement,
    NullChecker,

    QuestionMark,

    NamespaceAccessor,

    BooleanTypeName,
    IntegerTypeName,
    DecimalTypeName,
    StringTypeName,
    FunctionTypeName,
    VoidTypeName,

    BooleanLiteral,
    IntegerLiteral,
    DecimalLiteral,
    StringLiteral,
    NullLiteral,

    Const,
    Var,

    If,
    Else,

    For,
    While,
    Break,
    Continue,

    Comma,
    SemiColon,
    EOF,

    OpenBraces,
    CloseBraces,

    OpenParenthesis,
    CloseParenthesis,
}
