using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Tokens;
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
        [Keywords.VARIABLE_DECLARATION]  = (SourceInfo s) => new Token(TokenType.Var, Keywords.VARIABLE_DECLARATION, s),
        [Keywords.CONSTANT_DECLARATION]  = (SourceInfo s) => new Token(TokenType.Const, Keywords.CONSTANT_DECLARATION, s),
        [Keywords.NAMESPACE_DECLARATION] = (SourceInfo s) => new Token(TokenType.NamespaceDeclaration, Keywords.NAMESPACE_DECLARATION, s),
        [Keywords.TRUE_LITERAL]          = (SourceInfo s) => new Token(TokenType.BooleanLiteral, Keywords.TRUE_LITERAL, s),
        [Keywords.FALSE_LITERAL]         = (SourceInfo s) => new Token(TokenType.BooleanLiteral, Keywords.FALSE_LITERAL, s),
        [Keywords.IF]                    = (SourceInfo s) => new Token(TokenType.If, Keywords.IF, s),
        [Keywords.ELSE]                  = (SourceInfo s) => new Token(TokenType.Else, Keywords.ELSE, s),
        [Keywords.FOR]                   = (SourceInfo s) => new Token(TokenType.For, Keywords.FOR, s),
        [Keywords.WHILE]                 = (SourceInfo s) => new Token(TokenType.While, Keywords.WHILE, s),
        [Keywords.BREAK]                 = (SourceInfo s) => new Token(TokenType.Break, Keywords.BREAK, s),
        [Keywords.CONTINUE]              = (SourceInfo s) => new Token(TokenType.Continue, Keywords.CONTINUE, s),
        [Keywords.FUNCTION_DECLARATION]  = (SourceInfo s) => new Token(TokenType.FunctionDeclaration, Keywords.FUNCTION_DECLARATION, s),
        [Keywords.RETURN]                = (SourceInfo s) => new Token(TokenType.Return, Keywords.RETURN, s),
        [TypeNames.INTEGER_TYPE_NAME]    = (SourceInfo s) => new Token(TokenType.IntegerTypeName, TypeNames.INTEGER_TYPE_NAME, s),
        [TypeNames.DECIMAL_TYPE_NAME]    = (SourceInfo s) => new Token(TokenType.DecimalTypeName, TypeNames.DECIMAL_TYPE_NAME, s),
        [TypeNames.BOOLEAN_TYPE_NAME]    = (SourceInfo s) => new Token(TokenType.BooleanTypeName, TypeNames.BOOLEAN_TYPE_NAME, s),
        [TypeNames.STRING_TYPE_NAME]     = (SourceInfo s) => new Token(TokenType.StringTypeName, TypeNames.STRING_TYPE_NAME, s),
        [TypeNames.FUNCTION_TYPE_NAME]   = (SourceInfo s) => new Token(TokenType.FunctionTypeName, TypeNames.FUNCTION_TYPE_NAME, s),
        [TypeNames.NULL_TYPE_NAME]       = (SourceInfo s) => new Token(TokenType.NullLiteral, TypeNames.NULL_TYPE_NAME, s),
        [TypeNames.VOID_TYPE_NAME]       = (SourceInfo s) => new Token(TokenType.VoidTypeName, TypeNames.VOID_TYPE_NAME, s)
    };

    private static readonly Dictionary<string, Func<SourceInfo, Token>>.AlternateLookup<ReadOnlySpan<char>> _keywordTokensSpanLookup
        = _keywordTokens.GetAlternateLookup<ReadOnlySpan<char>>();

    private static readonly Dictionary<string, Func<SourceInfo, Token>> _operatorTokens = new()
    {
        [Operators.EQUAL]              = (SourceInfo s) => new Token(TokenType.Equal, Operators.EQUAL, s),
        [Operators.NOT_EQUAL]          = (SourceInfo s) => new Token(TokenType.NotEqual, Operators.NOT_EQUAL, s),
        [Operators.GREATER_OR_EQUAL]   = (SourceInfo s) => new Token(TokenType.GreaterEqual, Operators.GREATER_OR_EQUAL, s),
        [Operators.LESS_OR_EQUAL]      = (SourceInfo s) => new Token(TokenType.LessEqual, Operators.LESS_OR_EQUAL, s),
        [Operators.AND]                = (SourceInfo s) => new Token(TokenType.And, Operators.AND, s),
        [Operators.OR]                 = (SourceInfo s) => new Token(TokenType.Or, Operators.OR, s),
        [Operators.INCREMENT]          = (SourceInfo s) => new Token(TokenType.Increment, Operators.INCREMENT, s),
        [Operators.DECREMENT]          = (SourceInfo s) => new Token(TokenType.Decrement, Operators.DECREMENT, s),
        [Operators.NULL_CHECKER]       = (SourceInfo s) => new Token(TokenType.NullChecker, Operators.NULL_CHECKER, s),
        [Operators.PLUS_ASSIGN]        = (SourceInfo s) => new Token(TokenType.AdditionAssignment, Operators.PLUS_ASSIGN, s),
        [Operators.MINUS_ASSIGN]       = (SourceInfo s) => new Token(TokenType.SubtractionAssignment, Operators.MINUS_ASSIGN, s),
        [Operators.MULTIPLY_ASSIGN]    = (SourceInfo s) => new Token(TokenType.MultiplicationAssignment, Operators.MULTIPLY_ASSIGN, s),
        [Operators.DIVIDE_ASSIGN]      = (SourceInfo s) => new Token(TokenType.DivisionAssignment, Operators.DIVIDE_ASSIGN, s),
        [Operators.MODULO_ASSIGN]      = (SourceInfo s) => new Token(TokenType.ModuloAssignment, Operators.MODULO_ASSIGN, s),
        [Operators.SEMICOLON]          = (SourceInfo s) => new Token(TokenType.SemiColon, Operators.SEMICOLON, s),
        [Operators.COMMA]              = (SourceInfo s) => new Token(TokenType.Comma, Operators.COMMA, s),
        [Operators.OPEN_PARENTHESIS]   = (SourceInfo s) => new Token(TokenType.OpenParenthesis, Operators.OPEN_PARENTHESIS, s),
        [Operators.CLOSE_PARENTHESIS]  = (SourceInfo s) => new Token(TokenType.CloseParenthesis, Operators.CLOSE_PARENTHESIS, s),
        [Operators.OPEN_BRACES]        = (SourceInfo s) => new Token(TokenType.OpenBraces, Operators.OPEN_BRACES, s),
        [Operators.CLOSE_BRACES]       = (SourceInfo s) => new Token(TokenType.CloseBraces, Operators.CLOSE_BRACES, s),
        [Operators.ASSIGNMENT]         = (SourceInfo s) => new Token(TokenType.Assignment, Operators.ASSIGNMENT, s),
        [Operators.PLUS]               = (SourceInfo s) => new Token(TokenType.Plus, Operators.PLUS, s),
        [Operators.MINUS]              = (SourceInfo s) => new Token(TokenType.Minus, Operators.MINUS, s),
        [Operators.MULTIPLICATION]     = (SourceInfo s) => new Token(TokenType.Multiplication, Operators.MULTIPLICATION, s),
        [Operators.DIVISION]           = (SourceInfo s) => new Token(TokenType.Division, Operators.DIVISION, s),
        [Operators.MODULO]             = (SourceInfo s) => new Token(TokenType.Modulo, Operators.MODULO, s),
        [Operators.GREATER_THAN]       = (SourceInfo s) => new Token(TokenType.GreaterThan, Operators.GREATER_THAN, s),
        [Operators.LESS_THAN]          = (SourceInfo s) => new Token(TokenType.LessThan, Operators.LESS_THAN, s),
        [Operators.QUESTION_MARK]      = (SourceInfo s) => new Token(TokenType.QuestionMark, Operators.QUESTION_MARK, s),
        [Operators.NOT]                = (SourceInfo s) => new Token(TokenType.Not, Operators.NOT, s),
        [Operators.NAMESPACE_ACCESSOR] = (SourceInfo s) => new Token(TokenType.NamespaceAccessor, Operators.NAMESPACE_ACCESSOR, s)
    };

    private static readonly Dictionary<string, Func<SourceInfo, Token>>.AlternateLookup<ReadOnlySpan<char>> _operatorTokensSpanLookup
        = _operatorTokens.GetAlternateLookup<ReadOnlySpan<char>>();

    private static readonly int _maxOperatorLength = _operatorTokens.Keys.Max(k => k.Length);

    private Lexer(string sourceCode, string sourceLocation)
    {
        _sourceCode = sourceCode;
        _sourceLocation = sourceLocation;
    }

    public static Token[] Tokenize(string sourceCode, string sourceLocation)
    {
        var lexer = new Lexer(sourceCode, sourceLocation);
        return lexer.TokenizeInternal();
    }

    private Token[] TokenizeInternal()
    {
        var estimatedTokenQuantity = _sourceCode.Length / 3;
        estimatedTokenQuantity = Math.Max(256, estimatedTokenQuantity);

        var tokens = new List<Token>(estimatedTokenQuantity);

        while (!HasEnded())
        {
            if (CharUtils.IsWhiteSpace(Current()))
            {
                Advance();
                continue;
            }

            tokens.Add(ProcessCurrentToken());
        }

        tokens.Add(new Token(TokenType.EOF, string.Empty, GetCurrentSourceInfo()));

        return tokens.ToArray();
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

        return new Token(TokenType.Identifier, identifierSpan.ToString(), sourceStart);
    }

    private Token ProcessNumber()
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

        var tokenType = hasDot
                ? TokenType.DecimalLiteral
                : TokenType.IntegerLiteral;

        return new Token(tokenType, number.ToString(), sourceStart);
    }

    private Token ProcessString()
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

        return new Token(TokenType.StringLiteral, strBuilder.ToString(), sourceStart);
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