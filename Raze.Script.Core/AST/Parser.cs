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
            _program.Body.Add(ProcessCurrent());
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

    private Statement ProcessCurrent()
    {
        return ProcessAdditiveExpression();
    }

    private BinaryExpression ProcessAdditiveExpression()
    {
        BinaryExpression? result = null;

        Expression? left = ProcessPrimaryExpression();
        Advance();
        
        while (Current().TokenType == TokenType.AdditionOperator || Current().TokenType == TokenType.SubtractionOperator)
        {
            if (left is null)
            {
                left = result;
            }

            string op = Current().Lexeme;
            Advance();

            PrimaryExpression right = ProcessPrimaryExpression();
            Advance();

            result = new BinaryExpression(left!, op, right, left!.StartLine, left.StartColumn);
            left = null;
        }

        if (result is null)
        {
            throw new InvalidExpressionException("Invalid additive expression", left!.StartLine, left.StartColumn);
        }

        // coloca o indice de volta no lugar
        Return();

        return result;
    }

    private PrimaryExpression ProcessPrimaryExpression()
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
