using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Metadata;
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
    private readonly string _sourceLocation;
    private int _currentIndex = 0;
    private int _currentLine = 0;
    private int _currentColumn = 0;
    private List<Token> _tokens = [];

    private static readonly Dictionary<string, Func<string, SourceInfo, Token>> _keywords = new()
    {
        ["var"]      = (string lexeme, SourceInfo source) => new VarToken(lexeme, source),
        ["const"]    = (string lexeme, SourceInfo source) => new ConstToken(lexeme, source),
        ["integer"]  = (string lexeme, SourceInfo source) => new IntegerPrimitiveToken(lexeme, source),
        ["decimal"]  = (string lexeme, SourceInfo source) => new DecimalPrimitiveToken(lexeme, source),
        ["boolean"]  = (string lexeme, SourceInfo source) => new BooleanPrimitiveToken(lexeme, source),
        ["string"]   = (string lexeme, SourceInfo source) => new StringPrimitiveToken(lexeme, source),
        ["function"] = (string lexeme, SourceInfo source) => new FunctionPrimitiveToken(lexeme, source),
        ["void"]     = (string lexeme, SourceInfo source) => new VoidPrimitiveToken(lexeme, source),
        ["true"]     = (string lexeme, SourceInfo source) => new BooleanLiteralToken(lexeme, source),
        ["false"]    = (string lexeme, SourceInfo source) => new BooleanLiteralToken(lexeme, source),
        ["null"]     = (string lexeme, SourceInfo source) => new NullLiteralToken(lexeme, source),
        ["if"]       = (string lexeme, SourceInfo source) => new IfToken(lexeme, source),
        ["else"]     = (string lexeme, SourceInfo source) => new ElseToken(lexeme, source),
        ["for"]      = (string lexeme, SourceInfo source) => new ForToken(lexeme, source),
        ["while"]    = (string lexeme, SourceInfo source) => new WhileToken(lexeme, source),
        ["break"]    = (string lexeme, SourceInfo source) => new BreakToken(lexeme, source),
        ["continue"] = (string lexeme, SourceInfo source) => new ContinueToken(lexeme, source),
        ["def"]      = (string lexeme, SourceInfo source) => new FunctionDeclarationToken(lexeme, source),
        ["return"]   = (string lexeme, SourceInfo source) => new ReturnToken(lexeme, source)
    };

    private static readonly Dictionary<string, Func<string, SourceInfo, Token>> _doubleCharTokens = new()
    {
        ["=="] = (string lexeme, SourceInfo source) => new EqualToken(lexeme, source),
        ["!="] = (string lexeme, SourceInfo source) => new NotEqualToken(lexeme, source),
        [">="] = (string lexeme, SourceInfo source) => new GreaterOrEqualThanToken(lexeme, source),
        ["<="] = (string lexeme, SourceInfo source) => new LessOrEqualThanToken(lexeme, source),
        ["&&"] = (string lexeme, SourceInfo source) => new AndToken(lexeme, source),
        ["||"] = (string lexeme, SourceInfo source) => new OrToken(lexeme, source)
    };

    private static readonly Dictionary<char, Func<string, SourceInfo, Token>> _singleCharTokens = new()
    {
        [';'] = (string lexeme, SourceInfo source) => new SemiColonToken(lexeme, source),
        [','] = (string lexeme, SourceInfo source) => new CommaToken(lexeme, source),
        ['('] = (string lexeme, SourceInfo source) => new OpenParenthesisToken(lexeme, source),
        [')'] = (string lexeme, SourceInfo source) => new CloseParenthesisToken(lexeme, source),
        ['{'] = (string lexeme, SourceInfo source) => new OpenBracesToken(lexeme, source),
        ['}'] = (string lexeme, SourceInfo source) => new CloseBracesToken(lexeme, source),
        ['='] = (string lexeme, SourceInfo source) => new AssignmentToken(lexeme, source),
        ['+'] = (string lexeme, SourceInfo source) => new AdditionToken(lexeme, source),
        ['-'] = (string lexeme, SourceInfo source) => new SubtractionToken(lexeme, source),
        ['*'] = (string lexeme, SourceInfo source) => new MultiplicationToken(lexeme, source),
        ['/'] = (string lexeme, SourceInfo source) => new DivisionToken(lexeme, source),
        ['%'] = (string lexeme, SourceInfo source) => new ModuloToken(lexeme, source),
        ['>'] = (string lexeme, SourceInfo source) => new GreaterThanToken(lexeme, source),
        ['<'] = (string lexeme, SourceInfo source) => new LessThanToken(lexeme, source),
        ['?'] = (string lexeme, SourceInfo source) => new QuestionMarkToken(lexeme, source)
    };

    public Lexer(string sourceCode, string sourceLocation)
    {
        _sourceCode = sourceCode;
        _sourceLocation = sourceLocation;
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

        _tokens.Add(new EOFToken(GetCurrentSourceInfo()));

        return _tokens;
    }

    private SourceInfo GetCurrentSourceInfo(int lineOffset = 0, int columnOffset = 0)
    {
        // de indice baseado em 0 para indice baseado em 1
        var position = new SourcePosition(
            _currentLine + 1 + lineOffset,
            _currentColumn + 1 + columnOffset
        );

        return new SourceInfo(position, _sourceLocation);
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
                _tokens.Add(doubleTokenFunc(doubleToken, GetCurrentSourceInfo()));
                Advance(2);
                return;
            }
        }

        if (_singleCharTokens.TryGetValue(Current(), out var tokenFunc))
        {
            _tokens.Add(tokenFunc(Current().ToString(), GetCurrentSourceInfo()));
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
        if (CurrentIsValidIdentifierChar(isFirstChar: true))
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

        throw new UnexpectedCharacterException(Current(), GetCurrentSourceInfo());
    }

    private void ProcessIdentifier()
    {
        string identifier = "";
        var sourceStart = GetCurrentSourceInfo();

        while (!HasEnded() && CurrentIsValidIdentifierChar(isFirstChar: identifier.Length == 0))
        {
            identifier += Current();
            Advance();
        }

        if (_keywords.TryGetValue(identifier, out var tokenFunc))
        {
            _tokens.Add(tokenFunc(identifier, sourceStart));
        }
        else
        {
            _tokens.Add(new IdentifierToken(identifier, sourceStart));
        }
    }

    private bool CurrentIsValidIdentifierChar(bool isFirstChar)
    {
        if (HasEnded())
        {
            return false;
        }

        if (CharUtils.IsAsciiLetter(Current()) || Current() == '_')
        {
            return true;
        }

        return CharUtils.IsNumber(Current()) && !isFirstChar;
    }

    private void ProcessNumber()
    {
        string number = "";
        var sourceStart = GetCurrentSourceInfo();

        bool hasDot = false;

        while (!HasEnded() && (CharUtils.IsNumber(Current()) || Current() == '.'))
        {
            if (Current() == '.')
            {
                if (hasDot)
                {
                    throw new UnexpectedCharacterException(Current(), GetCurrentSourceInfo());
                }

                hasDot = true;
            }

            number += Current().ToString();
            Advance();
        }

        if (number.Last() == '.')
        {
            throw new UnexpectedCharacterException('.', GetCurrentSourceInfo(0, -1));
        }

        if (hasDot)
        {
            _tokens.Add(new DecimalLiteralToken(number, sourceStart));
        }
        else
        {
            _tokens.Add(new IntegerLiteralToken(number, sourceStart));
        }
    }

    private void ProcessString()
    {
        string str = "";
        var sourceStart = GetCurrentSourceInfo();
        Advance();

        if (HasEnded())
        {
            throw new InvalidStringException("\"" + StringUtils.UnescapeString(str), sourceStart);
        }

        while (Current() != '"')
        {
            if (Current() == '\n' || Peek() is null)
            {
                throw new InvalidStringException("\"" + StringUtils.UnescapeString(str), sourceStart);
            }

            if (Current() == '\\')
            {
                str += GetCurrentEscapedChar();

                if (Peek() is null)
                {
                    throw new InvalidStringException("\"" + StringUtils.UnescapeString(str), sourceStart);
                }
            }
            else
            {
                str += Current();
            }

            Advance();
        }

        Advance();

        _tokens.Add(new StringLiteralToken(str, sourceStart));
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
            _    => throw new UnrecognizedEscapeSequenceException("\\" + Current(), GetCurrentSourceInfo(0, -1)),
        };
    }
}