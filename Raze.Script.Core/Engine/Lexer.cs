using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Tokens;
using Raze.Script.Core.Tokens.ControlStructures;
using Raze.Script.Core.Tokens.Delimiters;
using Raze.Script.Core.Tokens.Grouping;
using Raze.Script.Core.Tokens.Literals;
using Raze.Script.Core.Tokens.Operators;
using Raze.Script.Core.Tokens.Operators.AdditiveOperators;
using Raze.Script.Core.Tokens.Operators.EqualityOperators;
using Raze.Script.Core.Tokens.Operators.MultiplicativeOperators;
using Raze.Script.Core.Tokens.Operators.RelationalOperators;
using Raze.Script.Core.Tokens.Primitives;
using Raze.Script.Core.Tokens.VariableDeclaration;
using Raze.Shared.Utils;

namespace Raze.Script.Core.Engine;

internal class Lexer
{
    public bool HasProcessed { get; private set; }

    private readonly string _sourceCode;
    private int _currentIndex = 0;
    private int _currentLine = 0;
    private int _currentColumn = 0;
    private List<Token> _tokens = [];

    private static readonly Dictionary<string, Func<string, int, int, Token>> _keywords = new()
    {
        ["var"]      = (string lexeme, int line, int column) => new VarToken(lexeme, line, column),
        ["const"]    = (string lexeme, int line, int column) => new ConstToken(lexeme, line, column),
        ["integer"]  = (string lexeme, int line, int column) => new IntegerPrimitiveToken(lexeme, line, column),
        ["decimal"]  = (string lexeme, int line, int column) => new DecimalPrimitiveToken(lexeme, line, column),
        ["boolean"]  = (string lexeme, int line, int column) => new BooleanPrimitiveToken(lexeme, line, column),
        ["string"]   = (string lexeme, int line, int column) => new StringPrimitiveToken(lexeme, line, column),
        ["true"]     = (string lexeme, int line, int column) => new BooleanLiteralToken(lexeme, line, column),
        ["false"]    = (string lexeme, int line, int column) => new BooleanLiteralToken(lexeme, line, column),
        ["null"]     = (string lexeme, int line, int column) => new NullLiteralToken(lexeme, line, column),
        ["if"]       = (string lexeme, int line, int column) => new IfToken(lexeme, line, column),
        ["else"]     = (string lexeme, int line, int column) => new ElseToken(lexeme, line, column),
        ["for"]      = (string lexeme, int line, int column) => new ForToken(lexeme, line, column),
        ["while"]    = (string lexeme, int line, int column) => new WhileToken(lexeme, line, column),
        ["break"]    = (string lexeme, int line, int column) => new BreakToken(lexeme, line, column),
        ["continue"] = (string lexeme, int line, int column) => new ContinueToken(lexeme, line, column),
        ["def"]      = (string lexeme, int line, int column) => new FunctionDeclarationToken(lexeme, line, column),
        ["return"]   = (string lexeme, int line, int column) => new ReturnToken(lexeme, line, column)
    };

    private static readonly Dictionary<string, Func<string, int, int, Token>> _doubleCharTokens = new()
    {
        ["=="] = (string lexeme, int line, int column) => new EqualToken(lexeme, line, column),
        ["!="] = (string lexeme, int line, int column) => new NotEqualToken(lexeme, line, column),
        [">="] = (string lexeme, int line, int column) => new GreaterOrEqualThanToken(lexeme, line, column),
        ["<="] = (string lexeme, int line, int column) => new LessOrEqualThanToken(lexeme, line, column),
        ["&&"] = (string lexeme, int line, int column) => new AndToken(lexeme, line, column),
        ["||"] = (string lexeme, int line, int column) => new OrToken(lexeme, line, column)
    };

    private static readonly Dictionary<char, Func<string, int, int, Token>> _singleCharTokens = new()
    {
        [';'] = (string lexeme, int line, int column) => new SemiColonToken(lexeme, line, column),
        [','] = (string lexeme, int line, int column) => new CommaToken(lexeme, line, column),
        ['('] = (string lexeme, int line, int column) => new OpenParenthesisToken(lexeme, line, column),
        [')'] = (string lexeme, int line, int column) => new CloseParenthesisToken(lexeme, line, column),
        ['{'] = (string lexeme, int line, int column) => new OpenBracesToken(lexeme, line, column),
        ['}'] = (string lexeme, int line, int column) => new CloseBracesToken(lexeme, line, column),
        ['='] = (string lexeme, int line, int column) => new AssignmentToken(lexeme, line, column),
        ['+'] = (string lexeme, int line, int column) => new AdditionToken(lexeme, line, column),
        ['-'] = (string lexeme, int line, int column) => new SubtractionToken(lexeme, line, column),
        ['*'] = (string lexeme, int line, int column) => new MultiplicationToken(lexeme, line, column),
        ['/'] = (string lexeme, int line, int column) => new DivisionToken(lexeme, line, column),
        ['%'] = (string lexeme, int line, int column) => new ModuloToken(lexeme, line, column),
        ['>'] = (string lexeme, int line, int column) => new GreaterThanToken(lexeme, line, column),
        ['<'] = (string lexeme, int line, int column) => new LessThanToken(lexeme, line, column),
        ['?'] = (string lexeme, int line, int column) => new QuestionMarkToken(lexeme, line, column)
    };

    public Lexer(string sourceCode)
    {
        _sourceCode = sourceCode;
    }

    public void Reset()
    {
        _currentIndex = 0;
        _currentLine = 0;
        _currentColumn = 0;
        _tokens.Clear();

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

        _tokens.Add(new EOFToken(_currentLine, _currentColumn));

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

    private void Advance(int howMuch = 1)
    {
        for (int i = 0; i < howMuch; i++)
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
    }

    private void ProcessCurrentToken()
    {
        if (Peek() is char next)
        {
            string doubleToken = $"{Current()}{next}";

            if (_doubleCharTokens.TryGetValue(doubleToken, out var doubleTokenFunc))
            {
                _tokens.Add(doubleTokenFunc(doubleToken, _currentLine, _currentColumn));
                Advance(2);
                return;
            }
        }

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

        if (CharUtils.IsNumber(Current()) || Current() == '.')
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
            _tokens.Add(new IdentifierToken(identifier, _currentLine, startColumn));
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
            _tokens.Add(new DecimalLiteralToken(number, _currentLine, startColumn));
        }
        else
        {
            _tokens.Add(new IntegerLiteralToken(number, _currentLine, startColumn));
        }
    }

    private void ProcessString()
    {
        string str = "";
        var startColumn = _currentColumn;
        Advance();

        if (HasEnded())
        {
            throw new InvalidStringException("\"" + StringUtils.UnescapeString(str), _currentLine, startColumn);
        }

        while (Current() != '"')
        {
            if (Current() == '\n' || Peek() is null)
            {
                throw new InvalidStringException("\"" + StringUtils.UnescapeString(str), _currentLine, startColumn);
            }

            if (Current() == '\\')
            {
                str += GetCurrentEscapedChar();

                if (Peek() is null)
                {
                    throw new InvalidStringException("\"" + StringUtils.UnescapeString(str), _currentLine, startColumn);
                }
            }
            else
            {
                str += Current();
            }

            Advance();
        }

        Advance();

        _tokens.Add(new StringLiteralToken(str, _currentLine, startColumn));
    }

    private char GetCurrentEscapedChar()
    {
        Advance();

        return Current() switch
        {
            '"'  => '"',
            'n'  => '\n',
            't'  => '\t',
            'r'  => '\r',
            '\\' => '\\',
            _    => throw new UnrecognizedEscapeSequenceException("\\" + Current(), _currentLine, _currentColumn - 1),
        };
    }
}