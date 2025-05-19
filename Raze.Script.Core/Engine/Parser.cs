using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Tokens;

namespace Raze.Script.Core.Engine;

internal class Parser
{
    public bool HasProcessed;

    private IList<Token> _tokens;
    private int _currentIndex;
    private ProgramExpression _program;

    public Parser(IList<Token> tokens)
    {
        if (tokens.Count == 0 || tokens.Last().TokenType != TokenType.EOF)
        {
            throw new Exception("Invalid token list");
        }

        _tokens = tokens;

        Reset();
    }

    public void Reset()
    {
        _currentIndex = 0;
        _program = new();

        HasProcessed = false;
    }

    public ProgramExpression Parse()
    {
        if (HasProcessed)
        {
            return _program;
        }

        _program = new();

        while (!HasEnded())
        {
            _program.Body.Add(ParseCurrent());
            Advance();
        }

        return _program;
    }

    private Token Current()
    {
        return _tokens[_currentIndex];
    }

    private void Advance()
    {
        _currentIndex++;
    }

    private void Return()
    {
        _currentIndex--;
    }

    private bool HasEnded()
    {
        return Current().TokenType == TokenType.EOF;
    }

    private Statement ParseCurrent()
    {
        return ParseAdditiveExpression();
    }

    private Expression ParseAdditiveExpression()
    {
        Expression? left = ParseMultiplicativeExpression();
        Advance();

        while (Current().TokenType == TokenType.AdditionOperator || Current().TokenType == TokenType.SubtractionOperator)
        {
            string op = Current().Lexeme;
            Advance();

            Expression right = ParseMultiplicativeExpression();
            Advance();

            left = new BinaryExpression(left!, op, right, left!.StartLine, left.StartColumn);
        }

        // coloca o indice de volta no lugar
        Return();

        return left;
    }

    private Expression ParseMultiplicativeExpression()
    {
        Expression? left = ParsePrimaryExpression();
        Advance();

        while (Current().TokenType == TokenType.MultiplicationOperator
            || Current().TokenType == TokenType.DivisionOperator
            || Current().TokenType == TokenType.ModuloOperator)
        {
            string op = Current().Lexeme;
            Advance();

            Expression right = ParsePrimaryExpression();
            Advance();

            left = new BinaryExpression(left!, op, right, left!.StartLine, left.StartColumn);
        }

        // coloca o indice de volta no lugar
        Return();

        return left;
    }

    private Expression ParsePrimaryExpression()
    {
        switch (Current().TokenType)
        {
            case TokenType.Identifier:
                return new IdentifierExpression(Current().Lexeme, Current().Line, Current().Column);
            case TokenType.IntegerLiteral:
                return new IntegerLiteralExpression(int.Parse(Current().Lexeme), Current().Line, Current().Column);
            case TokenType.NullLiteral:
                return new NullLiteralExpression(Current().Line, Current().Column);
            case TokenType.OpenParenthesis:
                Advance();
                Expression expr = ParseAdditiveExpression();
                Advance();
                if (Current().TokenType != TokenType.CloseParenthesis)
                {
                    throw new UnexpectedTokenException(
                        Current().TokenType.ToString(),
                        TokenType.CloseParenthesis.ToString(),
                        Current().Lexeme,
                        Current().Line,
                        Current().Column
                    );
                }
                return expr;
            default:
                throw new UnexpectedTokenException(
                    Current().TokenType.ToString(), Current().Lexeme, Current().Line, Current().Column
                );

        }
    }
}
