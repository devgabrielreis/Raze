using Raze.Script.Core.Exceptions;
using Raze.Shared.Utils;

namespace Raze.Script.Core.Lexer;

internal class Lexer
{
    public bool HasProcessed { get; private set; }

    private readonly string _sourceCode;
    private int _currentIndex;
    private int _currentLine;
    private int _currentColumn;
    private List<Token> _tokens;

    private static Dictionary<string, TokenType> _keywords = new()
    {
        { "var", TokenType.Var},
        { "integer", TokenType.IntegerType }
    };

    public Lexer(string sourceCode)
    {
        _sourceCode = sourceCode;
        
        Reset();
    }

    public void Reset()
    {
        _currentIndex = 0;
        _currentLine = 0;
        _currentColumn = 0;
        _tokens = [];

        HasProcessed = false;
    }

    public IList<Token> Tokenize()
    {
        if (HasProcessed)
        {
            return _tokens;
        }

        HasProcessed = true;

        while (!HasEnded())
        {
            ProcessCurrentToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", _currentLine, _currentColumn));

        return _tokens;
    }

    private bool HasEnded()
    {
        return !(_currentIndex < _sourceCode.Length);
    }

    private char Current()
    {
        return _sourceCode[_currentIndex];
    }

    private void Advance()
    {
        if (Current() == '\n')
        {
            _currentLine++;
            _currentColumn = 0;
        }
        else
        {
            _currentColumn++;
        }

        _currentIndex++;
    }

    private void ProcessCurrentToken()
    {
        switch (Current())
        {
            case ';':
                ProcessSingleCharacter(TokenType.SemiColon);
                break;
            case '=':
                ProcessSingleCharacter(TokenType.AssignmentOperator);
                break;
            case '+':
                ProcessSingleCharacter(TokenType.AdditionOperator);
                break;
            case '-':
                ProcessSingleCharacter(TokenType.SubtractionOperator);
                break;
            case '*':
                ProcessSingleCharacter(TokenType.MultiplicationOperator);
                break;
            case '/':
                ProcessSingleCharacter(TokenType.DivisionOperator);
                break;
            case '%':
                ProcessSingleCharacter(TokenType.ModuloOperator);
                break;
            default:
                if (CharUtils.IsAsciiLetter(Current()))
                {
                    ProcessIdentifier();
                }
                else if (CharUtils.IsNumber(Current()))
                {
                    ProcessNumber();
                }
                else if (CharUtils.IsWhiteSpace(Current()))
                {
                    Advance();
                }
                else
                {
                    throw new UnexpectedCharacterException(Current(), _currentLine, _currentColumn);
                }
                break;
        }
    }

    private void ProcessSingleCharacter(TokenType tokenType)
    {
        _tokens.Add(new Token(tokenType, Current().ToString(), _currentLine, _currentColumn));
        Advance();
    }

    private void ProcessIdentifier()
    {
        string identifier = "";
        var startColumn = _currentColumn;

        while (!HasEnded() && (CharUtils.IsAsciiLetter(Current()) || CharUtils.IsNumber(Current())))
        {
            identifier += Current();
            Advance();
        }

        TokenType tokenType;

        if (_keywords.TryGetValue(identifier, out TokenType token))
        {
            tokenType = token;
        }
        else
        {
            tokenType = TokenType.Identifier;
        }

        _tokens.Add(new Token(tokenType, identifier, _currentLine, startColumn));
    }

    private void ProcessNumber()
    {
        string number = "";
        var startColumn = _currentColumn;

        while (!HasEnded() && CharUtils.IsNumber(Current()))
        {
            number += Current().ToString();
            Advance();
        }

        _tokens.Add(new Token(TokenType.IntegerLiteral, number, _currentLine, startColumn));
    }
}
