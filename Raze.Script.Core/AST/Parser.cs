using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Lexer;

namespace Raze.Script.Core.AST;

internal class Parser
{
    public bool HasProcessed;

    private IList<Token> _tokens;
    private int _currentIndex;
    private ProgramStatement _program;

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

    public ProgramStatement Parse()
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
        if (Current().TokenType == TokenType.Identifier)
        {
            return new IdentifierExpression(Current().Lexeme, Current().Line, Current().Column);
        }
        else if (Current().TokenType == TokenType.IntegerLiteral)
        {
            return new IntegerLiteralExpression(int.Parse(Current().Lexeme), Current().Line, Current().Column);
        }

        throw new UnexpectedTokenException(Current());
    }
}
