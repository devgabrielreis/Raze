using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Tokens;
using Raze.Script.Core.Tokens.Delimiters;
using Raze.Script.Core.Tokens.Grouping;
using Raze.Script.Core.Tokens.Literals;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Primitives;
using Raze.Script.Core.Tokens.VariableDeclaration;
using Raze.Shared.Utils;

namespace Raze.Script.Core.Engine;

internal class Lexer
{
    public bool HasProcessed { get; private set; }

    private readonly string _sourceCode;
    private int _currentIndex;
    private int _currentLine;
    private int _currentColumn;
    private List<Token> _tokens;

    private static Dictionary<string, Func<string, int, int, Token>> _keywords = new()
    {
        ["var"]     = (string lexeme, int line, int column) => new Var(lexeme, line, column),
        ["const"]   = (string lexeme, int line, int column) => new Const(lexeme, line, column),
        ["integer"] = (string lexeme, int line, int column) => new IntegerPrimitive(lexeme, line, column),
        ["decimal"] = (string lexeme, int line, int column) => new DecimalPrimitive(lexeme, line, column),
        ["boolean"] = (string lexeme, int line, int column) => new BooleanPrimitive(lexeme, line, column),
        ["true"]    = (string lexeme, int line, int column) => new BooleanLiteral(lexeme, line, column),
        ["false"]   = (string lexeme, int line, int column) => new BooleanLiteral(lexeme, line, column),
        ["NULL"]    = (string lexeme, int line, int column) => new NullLiteral(lexeme, line, column)
    };

    private static Dictionary<char, Func<string, int, int, Token>> _singleCharTokens = new()
    {
        [';'] = (string lexeme, int line, int column) => new SemiColon(lexeme, line, column),
        ['('] = (string lexeme, int line, int column) => new OpenParenthesis(lexeme, line, column),
        [')'] = (string lexeme, int line, int column) => new CloseParenthesis(lexeme, line, column),
        ['='] = (string lexeme, int line, int column) => new AssignmentOperator(lexeme, line, column),
        ['+'] = (string lexeme, int line, int column) => new AdditionOperator(lexeme, line, column),
        ['-'] = (string lexeme, int line, int column) => new SubtractionOperator(lexeme, line, column),
        ['*'] = (string lexeme, int line, int column) => new MultiplicationOperator(lexeme, line, column),
        ['/'] = (string lexeme, int line, int column) => new DivisionOperator(lexeme, line, column),
        ['%'] = (string lexeme, int line, int column) => new ModuloOperator(lexeme, line, column)
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

        _tokens.Add(new EOF(_currentLine, _currentColumn));

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

    private char? Peek(int howMuch = 1)
    {
        int target = _currentIndex + howMuch;

        if (target >= _sourceCode.Length)
        {
            return null;
        }

        return _sourceCode[target];
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
        if (_singleCharTokens.TryGetValue(Current(), out var tokenFunc))
        {
            _tokens.Add(tokenFunc(Current().ToString(), _currentLine, _currentColumn));
            Advance();
            return;
        }

        if (CharUtils.IsWhiteSpace(Current()))
        {
            Advance();
            return;
        }

        ProcessMultiCharacterToken();
    }

    private void ProcessMultiCharacterToken()
    {
        if (CharUtils.IsAsciiLetter(Current()))
        {
            ProcessIdentifier();
            return;
        }

        if (CharUtils.IsNumber(Current()))
        {
            ProcessNumber();
            return;
        }

        if (Current() == '"')
        {
            ProcessString();
            return;
        }

        throw new UnexpectedCharacterException(Current(), _currentLine, _currentColumn);
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

        if (_keywords.TryGetValue(identifier, out var tokenFunc))
        {
            _tokens.Add(tokenFunc(identifier, _currentLine, startColumn));
        }
        else
        {
            _tokens.Add(new Identifier(identifier, _currentLine, startColumn));
        }
    }

    private void ProcessNumber()
    {
        string number = "";
        var startColumn = _currentColumn;

        bool hasDot = false;

        while (!HasEnded() && (CharUtils.IsNumber(Current()) || Current() == '.'))
        {
            if (Current() == '.')
            {
                if (hasDot)
                {
                    throw new UnexpectedCharacterException(Current(), _currentLine, _currentColumn);
                }

                hasDot = true;
            }

            number += Current().ToString();
            Advance();
        }

        if (number.Last() == '.')
        {
            throw new UnexpectedCharacterException('.', _currentLine, _currentColumn - 1);
        }

        if (hasDot)
        {
            _tokens.Add(new DecimalLiteral(number, _currentLine, startColumn));
        }
        else
        {
            _tokens.Add(new IntegerLiteral(number, _currentLine, startColumn));
        }
    }

    private void ProcessString()
    {
        string str = "";
        var startColumn = _currentColumn;
        Advance();

        while (Current() != '"')
        {
            if (Current() == '\n')
            {
                throw new InvalidStringException("\"" + str, _currentLine, startColumn);
            }

            str += Current();

            if (Peek() is null)
            {
                throw new InvalidStringException("\"" + str, _currentLine, startColumn);
            }

            Advance();
        }

        Advance();

        _tokens.Add(new StringLiteral(str, _currentLine, startColumn));
    }
}