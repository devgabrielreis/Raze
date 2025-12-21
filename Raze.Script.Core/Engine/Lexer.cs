using Raze.Script.Core.Constants;
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
using System.Text;

namespace Raze.Script.Core.Engine;

internal class Lexer
{
    private readonly string _sourceCode;
    private readonly string _sourceLocation;

    private int _currentIndex = 0;
    private int _currentLine = 0;
    private int _currentColumn = 0;

    private static readonly Dictionary<string, Func<string, SourceInfo, Token>> _keywords = new()
    {
        ["var"]      = (string lexeme, SourceInfo source) => new VarToken(lexeme, source),
        ["const"]    = (string lexeme, SourceInfo source) => new ConstToken(lexeme, source),
        ["true"]     = (string lexeme, SourceInfo source) => new BooleanLiteralToken(lexeme, source),
        ["false"]    = (string lexeme, SourceInfo source) => new BooleanLiteralToken(lexeme, source),
        ["if"]       = (string lexeme, SourceInfo source) => new IfToken(lexeme, source),
        ["else"]     = (string lexeme, SourceInfo source) => new ElseToken(lexeme, source),
        ["for"]      = (string lexeme, SourceInfo source) => new ForToken(lexeme, source),
        ["while"]    = (string lexeme, SourceInfo source) => new WhileToken(lexeme, source),
        ["break"]    = (string lexeme, SourceInfo source) => new BreakToken(lexeme, source),
        ["continue"] = (string lexeme, SourceInfo source) => new ContinueToken(lexeme, source),
        ["def"]      = (string lexeme, SourceInfo source) => new FunctionDeclarationToken(lexeme, source),
        ["return"]   = (string lexeme, SourceInfo source) => new ReturnToken(lexeme, source),
        [TypeNames.INTEGER_TYPE_NAME]  = (string lexeme, SourceInfo source) => new IntegerPrimitiveToken(lexeme, source),
        [TypeNames.DECIMAL_TYPE_NAME]  = (string lexeme, SourceInfo source) => new DecimalPrimitiveToken(lexeme, source),
        [TypeNames.BOOLEAN_TYPE_NAME]  = (string lexeme, SourceInfo source) => new BooleanPrimitiveToken(lexeme, source),
        [TypeNames.STRING_TYPE_NAME]   = (string lexeme, SourceInfo source) => new StringPrimitiveToken(lexeme, source),
        [TypeNames.FUNCTION_TYPE_NAME] = (string lexeme, SourceInfo source) => new FunctionPrimitiveToken(lexeme, source),
        [TypeNames.NULL_TYPE_NAME]     = (string lexeme, SourceInfo source) => new NullLiteralToken(lexeme, source),
        [TypeNames.VOID_TYPE_NAME]     = (string lexeme, SourceInfo source) => new VoidPrimitiveToken(lexeme, source)
    };

    private static readonly Dictionary<string, Func<string, SourceInfo, Token>> _doubleCharTokens = new()
    {
        ["=="] = (string lexeme, SourceInfo source) => new EqualToken(lexeme, source),
        ["!="] = (string lexeme, SourceInfo source) => new NotEqualToken(lexeme, source),
        [">="] = (string lexeme, SourceInfo source) => new GreaterOrEqualThanToken(lexeme, source),
        ["<="] = (string lexeme, SourceInfo source) => new LessOrEqualThanToken(lexeme, source),
        ["&&"] = (string lexeme, SourceInfo source) => new AndToken(lexeme, source),
        ["||"] = (string lexeme, SourceInfo source) => new OrToken(lexeme, source),
        ["++"] = (string lexeme, SourceInfo source) => new IncrementToken(lexeme, source),
        ["--"] = (string lexeme, SourceInfo source) => new DecrementToken(lexeme, source),
        ["??"] = (string lexeme, SourceInfo source) => new NullCheckerToken(lexeme, source),
        ["+="] = (string lexeme, SourceInfo source) => new CompoundAssignmentToken(new AdditionToken("+", source), lexeme, source),
        ["-="] = (string lexeme, SourceInfo source) => new CompoundAssignmentToken(new SubtractionToken("-", source), lexeme, source),
        ["*="] = (string lexeme, SourceInfo source) => new CompoundAssignmentToken(new MultiplicationToken("*", source), lexeme, source),
        ["/="] = (string lexeme, SourceInfo source) => new CompoundAssignmentToken(new DivisionToken("/", source), lexeme, source),
        ["%="] = (string lexeme, SourceInfo source) => new CompoundAssignmentToken(new ModuloToken("%", source), lexeme, source)
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
        ['?'] = (string lexeme, SourceInfo source) => new QuestionMarkToken(lexeme, source),
        ['!'] = (string lexeme, SourceInfo source) => new NotToken(lexeme, source)
    };

    private Lexer(string sourceCode, string sourceLocation)
    {
        _sourceCode = sourceCode;
        _sourceLocation = sourceLocation;
    }

    public static IList<Token> Tokenize(string sourceCode, string sourceLocation)
    {
        var lexer = new Lexer(sourceCode, sourceLocation);
        return lexer.TokenizeInternal();
    }

    private IList<Token> TokenizeInternal()
    {
        List<Token> tokens = [];

        while (!HasEnded())
        {
            if (CharUtils.IsWhiteSpace(Current()))
            {
                Advance();
                continue;
            }

            tokens.Add(ProcessCurrentToken());
        }

        tokens.Add(new EOFToken(GetCurrentSourceInfo()));

        return tokens;
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

    private Token ProcessCurrentToken()
    {
        if (Peek() is char next)
        {
            string doubleToken = $"{Current()}{next}";

            if (_doubleCharTokens.TryGetValue(doubleToken, out var doubleTokenFunc))
            {
                var source = GetCurrentSourceInfo();
                Advance(2);
                return doubleTokenFunc(doubleToken, source);
            }
        }

        if (_singleCharTokens.TryGetValue(Current(), out var tokenFunc))
        {
            Token token = tokenFunc(Current().ToString(), GetCurrentSourceInfo());
            Advance();

            return token;
        }

        return ProcessMultiCharacterToken();
    }

    private Token ProcessMultiCharacterToken()
    {
        if (CurrentIsValidIdentifierChar(isFirstChar: true))
        {
            return ProcessIdentifier();
        }

        if (CharUtils.IsNumber(Current()) || Current() == '.')
        {
            return ProcessNumber();
        }

        if (Current() == '"')
        {
            return ProcessString();
        }

        throw new UnexpectedCharacterException(Current(), GetCurrentSourceInfo());
    }

    private Token ProcessIdentifier()
    {
        var identifierBuilder = new StringBuilder(25);
        var sourceStart = GetCurrentSourceInfo();

        while (!HasEnded() && CurrentIsValidIdentifierChar(isFirstChar: identifierBuilder.Length == 0))
        {
            identifierBuilder.Append(Current());
            Advance();
        }

        var identifier = identifierBuilder.ToString();

        return _keywords.TryGetValue(identifier, out var tokenFunc)
                ? tokenFunc(identifier, sourceStart)
                : new IdentifierToken(identifier, sourceStart);
    }

    private LiteralToken ProcessNumber()
    {
        var numberBuilder = new StringBuilder(10);
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

            numberBuilder.Append(Current());
            Advance();
        }

        var number = numberBuilder.ToString();

        if (number.Last() == '.')
        {
            throw new UnexpectedCharacterException('.', GetCurrentSourceInfo(0, -1));
        }

        return hasDot
                ? new DecimalLiteralToken(number, sourceStart)
                : new IntegerLiteralToken(number, sourceStart);
    }

    private StringLiteralToken ProcessString()
    {
        var strBuilder = new StringBuilder(50);
        var sourceStart = GetCurrentSourceInfo();
        Advance();

        if (HasEnded())
        {
            throw new InvalidStringException("\"" + StringUtils.UnescapeString(strBuilder.ToString()), sourceStart);
        }

        while (Current() != '"')
        {
            if (Current() == '\n' || Peek() is null)
            {
                throw new InvalidStringException("\"" + StringUtils.UnescapeString(strBuilder.ToString()), sourceStart);
            }

            if (Current() == '\\')
            {
                strBuilder.Append(GetCurrentEscapedChar());

                if (Peek() is null)
                {
                    throw new InvalidStringException("\"" + StringUtils.UnescapeString(strBuilder.ToString()), sourceStart);
                }
            }
            else
            {
                strBuilder.Append(Current());
            }

            Advance();
        }

        Advance();

        return new StringLiteralToken(strBuilder.ToString(), sourceStart);
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