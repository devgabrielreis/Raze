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

    private static readonly Dictionary<string, Func<SourceInfo, Token>> _keywordTokens = new()
    {
        [Keywords.VARIABLE_DECLARATION] = (SourceInfo s) => new VarToken(Keywords.VARIABLE_DECLARATION, s),
        [Keywords.CONSTANT_DECLARATION] = (SourceInfo s) => new ConstToken(Keywords.CONSTANT_DECLARATION, s),
        [Keywords.TRUE_LITERAL]         = (SourceInfo s) => new BooleanLiteralToken(Keywords.TRUE_LITERAL, s),
        [Keywords.FALSE_LITERAL]        = (SourceInfo s) => new BooleanLiteralToken(Keywords.FALSE_LITERAL, s),
        [Keywords.IF]                   = (SourceInfo s) => new IfToken(Keywords.IF, s),
        [Keywords.ELSE]                 = (SourceInfo s) => new ElseToken(Keywords.ELSE, s),
        [Keywords.FOR]                  = (SourceInfo s) => new ForToken(Keywords.FOR, s),
        [Keywords.WHILE]                = (SourceInfo s) => new WhileToken(Keywords.WHILE, s),
        [Keywords.BREAK]                = (SourceInfo s) => new BreakToken(Keywords.BREAK, s),
        [Keywords.CONTINUE]             = (SourceInfo s) => new ContinueToken(Keywords.CONTINUE, s),
        [Keywords.FUNCTION_DECLARATION] = (SourceInfo s) => new FunctionDeclarationToken(Keywords.FUNCTION_DECLARATION, s),
        [Keywords.RETURN]               = (SourceInfo s) => new ReturnToken(Keywords.RETURN, s),
        [TypeNames.INTEGER_TYPE_NAME]   = (SourceInfo s) => new IntegerPrimitiveToken(TypeNames.INTEGER_TYPE_NAME, s),
        [TypeNames.DECIMAL_TYPE_NAME]   = (SourceInfo s) => new DecimalPrimitiveToken(TypeNames.DECIMAL_TYPE_NAME, s),
        [TypeNames.BOOLEAN_TYPE_NAME]   = (SourceInfo s) => new BooleanPrimitiveToken(TypeNames.BOOLEAN_TYPE_NAME, s),
        [TypeNames.STRING_TYPE_NAME]    = (SourceInfo s) => new StringPrimitiveToken(TypeNames.STRING_TYPE_NAME, s),
        [TypeNames.FUNCTION_TYPE_NAME]  = (SourceInfo s) => new FunctionPrimitiveToken(TypeNames.FUNCTION_TYPE_NAME, s),
        [TypeNames.NULL_TYPE_NAME]      = (SourceInfo s) => new NullLiteralToken(TypeNames.NULL_TYPE_NAME, s),
        [TypeNames.VOID_TYPE_NAME]      = (SourceInfo s) => new VoidPrimitiveToken(TypeNames.VOID_TYPE_NAME, s)
    };

    private static readonly Dictionary<string, Func<SourceInfo, Token>>.AlternateLookup<ReadOnlySpan<char>> _keywordTokensSpanLookup
        = _keywordTokens.GetAlternateLookup<ReadOnlySpan<char>>();

    private static readonly Dictionary<string, Func<SourceInfo, Token>> _operatorTokens = new()
    {
        [Operators.EQUAL]             = (SourceInfo s) => new EqualToken(Operators.EQUAL, s),
        [Operators.NOT_EQUAL]         = (SourceInfo s) => new NotEqualToken(Operators.NOT_EQUAL, s),
        [Operators.GREATER_OR_EQUAL]  = (SourceInfo s) => new GreaterOrEqualThanToken(Operators.GREATER_OR_EQUAL, s),
        [Operators.LESS_OR_EQUAL]     = (SourceInfo s) => new LessOrEqualThanToken(Operators.LESS_OR_EQUAL, s),
        [Operators.AND]               = (SourceInfo s) => new AndToken(Operators.AND, s),
        [Operators.OR]                = (SourceInfo s) => new OrToken(Operators.OR, s),
        [Operators.INCREMENT]         = (SourceInfo s) => new IncrementToken(Operators.INCREMENT, s),
        [Operators.DECREMENT]         = (SourceInfo s) => new DecrementToken(Operators.DECREMENT, s),
        [Operators.NULL_CHECKER]      = (SourceInfo s) => new NullCheckerToken(Operators.NULL_CHECKER, s),
        [Operators.PLUS_ASSIGN]       = (SourceInfo s) => new CompoundAssignmentToken(Operators.PLUS, Operators.PLUS_ASSIGN, s),
        [Operators.MINUS_ASSIGN]      = (SourceInfo s) => new CompoundAssignmentToken(Operators.MINUS, Operators.MINUS_ASSIGN, s),
        [Operators.MULTIPLY_ASSIGN]   = (SourceInfo s) => new CompoundAssignmentToken(Operators.MULTIPLICATION, Operators.MULTIPLY_ASSIGN, s),
        [Operators.DIVIDE_ASSIGN]     = (SourceInfo s) => new CompoundAssignmentToken(Operators.DIVISION, Operators.DIVIDE_ASSIGN, s),
        [Operators.MODULO_ASSIGN]     = (SourceInfo s) => new CompoundAssignmentToken(Operators.MODULO, Operators.MODULO_ASSIGN, s),
        [Operators.SEMICOLON]         = (SourceInfo s) => new SemiColonToken(Operators.SEMICOLON, s),
        [Operators.COMMA]             = (SourceInfo s) => new CommaToken(Operators.COMMA, s),
        [Operators.OPEN_PARENTHESIS]  = (SourceInfo s) => new OpenParenthesisToken(Operators.OPEN_PARENTHESIS, s),
        [Operators.CLOSE_PARENTHESIS] = (SourceInfo s) => new CloseParenthesisToken(Operators.CLOSE_PARENTHESIS, s),
        [Operators.OPEN_BRACES]       = (SourceInfo s) => new OpenBracesToken(Operators.OPEN_BRACES, s),
        [Operators.CLOSE_BRACES]      = (SourceInfo s) => new CloseBracesToken(Operators.CLOSE_BRACES, s),
        [Operators.ASSIGNMENT]        = (SourceInfo s) => new AssignmentToken(Operators.ASSIGNMENT, s),
        [Operators.PLUS]              = (SourceInfo s) => new AdditionToken(Operators.PLUS, s),
        [Operators.MINUS]             = (SourceInfo s) => new SubtractionToken(Operators.MINUS, s),
        [Operators.MULTIPLICATION]    = (SourceInfo s) => new MultiplicationToken(Operators.MULTIPLICATION, s),
        [Operators.DIVISION]          = (SourceInfo s) => new DivisionToken(Operators.DIVISION, s),
        [Operators.MODULO]            = (SourceInfo s) => new ModuloToken(Operators.MODULO, s),
        [Operators.GREATER_THAN]      = (SourceInfo s) => new GreaterThanToken(Operators.GREATER_THAN, s),
        [Operators.LESS_THAN]         = (SourceInfo s) => new LessThanToken(Operators.LESS_THAN, s),
        [Operators.QUESTION_MARK]     = (SourceInfo s) => new QuestionMarkToken(Operators.QUESTION_MARK, s),
        [Operators.NOT]               = (SourceInfo s) => new NotToken(Operators.NOT, s)
    };

    private static readonly Dictionary<string, Func<SourceInfo, Token>>.AlternateLookup<ReadOnlySpan<char>> _operatorTokensSpanLookup
        = _operatorTokens.GetAlternateLookup<ReadOnlySpan<char>>();

    private static readonly int _maxOperatorLength = _operatorTokens.Keys.Max(k => k.Length);

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
        if (TryReadOperator() is Token operatorToken)
        {
            return operatorToken;
        }

        return ProcessMultiCharacterToken();
    }

    private Token? TryReadOperator()
    {
        int remainingSourceLength = _sourceCode.Length - _currentIndex;
        int max = Math.Min(_maxOperatorLength, remainingSourceLength);

        ReadOnlySpan<char> slice = _sourceCode.AsSpan(_currentIndex, max);

        for (int length = max; length > 0; length--)
        {
            ReadOnlySpan<char> operatorSpan = slice.Slice(0, length);

            if (_operatorTokensSpanLookup.TryGetValue(operatorSpan, out var operatorFunc))
            {
                var source = GetCurrentSourceInfo();
                Advance(length);
                return operatorFunc(source);
            }
        }

        return null;
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
        var sourceStart = GetCurrentSourceInfo();
        var identifierStart = _currentIndex;

        while (!HasEnded() && CurrentIsValidIdentifierChar(isFirstChar: (_currentIndex - identifierStart) == 0))
        {
            Advance();
        }

        var identifierSpan = _sourceCode.AsSpan(identifierStart, _currentIndex - identifierStart);

        if (_keywordTokensSpanLookup.TryGetValue(identifierSpan, out var keywordFunc))
        {
            return keywordFunc(sourceStart);
        }

        return new IdentifierToken(identifierSpan.ToString(), sourceStart);
    }

    private LiteralToken ProcessNumber()
    {
        var sourceStart = GetCurrentSourceInfo();
        var numberStart = _currentIndex;

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

            Advance();
        }

        var number = _sourceCode.AsSpan(numberStart, _currentIndex - numberStart);

        if (number[^1] == '.')
        {
            throw new UnexpectedCharacterException('.', GetCurrentSourceInfo(0, -1));
        }

        return hasDot
                ? new DecimalLiteralToken(number.ToString(), sourceStart)
                : new IntegerLiteralToken(number.ToString(), sourceStart);
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