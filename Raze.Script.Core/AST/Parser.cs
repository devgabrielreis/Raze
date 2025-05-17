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
            ProcessCurrent();
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

    private bool HasEnded()
    {
        return (Current().TokenType == TokenType.EOF) || !(_currentIndex < _tokens.Count);
    }

    private void ProcessCurrent()
    {
        ProcessPrimaryExpression();
    }

    private void ProcessPrimaryExpression()
    {
        if (Current().TokenType == TokenType.Identifier)
        {
            _program.Body.Add(new IdentifierExpression(Current().Lexeme));
        }
        else if (Current().TokenType == TokenType.IntegerLiteral)
        {
            _program.Body.Add(new IntegerLiteralExpression(int.Parse(Current().Lexeme)));
        }
        else
        {
            throw new UnexpectedTokenException(Current());
        }

        Advance();
    }
}
