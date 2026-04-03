using Raze.Script.Core.Constants;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Tokens;
using Raze.Shared.Utils;
using System.Text;

namespace Raze.Script.Core.Engine;

internal sealed class Lexer
{
    private readonly string _sourceCode;
    private readonly string _sourceLocation;

    private int _currentIndex = 0;
    private int _currentLine = 0;
    private int _currentColumn = 0;

    private delegate Token TokenFactory(ref readonly SourceInfo s);

    private static readonly Dictionary<string, TokenFactory> _keywordTokens = new()
    {
        [Keywords.VARIABLE_DECLARATION]  = (ref readonly SourceInfo s) => new Token(TokenType.Var, Keywords.VARIABLE_DECLARATION, in s),
        [Keywords.CONSTANT_DECLARATION]  = (ref readonly SourceInfo s) => new Token(TokenType.Const, Keywords.CONSTANT_DECLARATION, in s),
        [Keywords.NAMESPACE_DECLARATION] = (ref readonly SourceInfo s) => new Token(TokenType.NamespaceDeclaration, Keywords.NAMESPACE_DECLARATION, in s),
        [Keywords.TRUE_LITERAL]          = (ref readonly SourceInfo s) => new Token(TokenType.BooleanLiteral, Keywords.TRUE_LITERAL, in s),
        [Keywords.FALSE_LITERAL]         = (ref readonly SourceInfo s) => new Token(TokenType.BooleanLiteral, Keywords.FALSE_LITERAL, in s),
        [Keywords.IF]                    = (ref readonly SourceInfo s) => new Token(TokenType.If, Keywords.IF, in s),
        [Keywords.ELSE]                  = (ref readonly SourceInfo s) => new Token(TokenType.Else, Keywords.ELSE, in s),
        [Keywords.FOR]                   = (ref readonly SourceInfo s) => new Token(TokenType.For, Keywords.FOR, in s),
        [Keywords.WHILE]                 = (ref readonly SourceInfo s) => new Token(TokenType.While, Keywords.WHILE, in s),
        [Keywords.BREAK]                 = (ref readonly SourceInfo s) => new Token(TokenType.Break, Keywords.BREAK, in s),
        [Keywords.CONTINUE]              = (ref readonly SourceInfo s) => new Token(TokenType.Continue, Keywords.CONTINUE, in s),
        [Keywords.FUNCTION_DECLARATION]  = (ref readonly SourceInfo s) => new Token(TokenType.FunctionDeclaration, Keywords.FUNCTION_DECLARATION, in s),
        [Keywords.RETURN]                = (ref readonly SourceInfo s) => new Token(TokenType.Return, Keywords.RETURN, in s),
        [TypeNames.INTEGER_TYPE_NAME]    = (ref readonly SourceInfo s) => new Token(TokenType.IntegerTypeName, TypeNames.INTEGER_TYPE_NAME, in s),
        [TypeNames.DECIMAL_TYPE_NAME]    = (ref readonly SourceInfo s) => new Token(TokenType.DecimalTypeName, TypeNames.DECIMAL_TYPE_NAME, in s),
        [TypeNames.BOOLEAN_TYPE_NAME]    = (ref readonly SourceInfo s) => new Token(TokenType.BooleanTypeName, TypeNames.BOOLEAN_TYPE_NAME, in s),
        [TypeNames.STRING_TYPE_NAME]     = (ref readonly SourceInfo s) => new Token(TokenType.StringTypeName, TypeNames.STRING_TYPE_NAME, in s),
        [TypeNames.FUNCTION_TYPE_NAME]   = (ref readonly SourceInfo s) => new Token(TokenType.FunctionTypeName, TypeNames.FUNCTION_TYPE_NAME, in s),
        [TypeNames.NULL_TYPE_NAME]       = (ref readonly SourceInfo s) => new Token(TokenType.NullLiteral, TypeNames.NULL_TYPE_NAME, in s),
        [TypeNames.VOID_TYPE_NAME]       = (ref readonly SourceInfo s) => new Token(TokenType.VoidTypeName, TypeNames.VOID_TYPE_NAME, in s)
    };

    private static readonly Dictionary<string, TokenFactory>.AlternateLookup<ReadOnlySpan<char>> _keywordTokensSpanLookup
        = _keywordTokens.GetAlternateLookup<ReadOnlySpan<char>>();

    private static readonly Dictionary<string, TokenFactory> _operatorTokens = new()
    {
        [Operators.EQUAL]              = (ref readonly SourceInfo s) => new Token(TokenType.Equal, Operators.EQUAL, in s),
        [Operators.NOT_EQUAL]          = (ref readonly SourceInfo s) => new Token(TokenType.NotEqual, Operators.NOT_EQUAL, in s),
        [Operators.GREATER_OR_EQUAL]   = (ref readonly SourceInfo s) => new Token(TokenType.GreaterEqual, Operators.GREATER_OR_EQUAL, in s),
        [Operators.LESS_OR_EQUAL]      = (ref readonly SourceInfo s) => new Token(TokenType.LessEqual, Operators.LESS_OR_EQUAL, in s),
        [Operators.AND]                = (ref readonly SourceInfo s) => new Token(TokenType.And, Operators.AND, in s),
        [Operators.OR]                 = (ref readonly SourceInfo s) => new Token(TokenType.Or, Operators.OR, in s),
        [Operators.INCREMENT]          = (ref readonly SourceInfo s) => new Token(TokenType.Increment, Operators.INCREMENT, in s),
        [Operators.DECREMENT]          = (ref readonly SourceInfo s) => new Token(TokenType.Decrement, Operators.DECREMENT, in s),
        [Operators.NULL_CHECKER]       = (ref readonly SourceInfo s) => new Token(TokenType.NullChecker, Operators.NULL_CHECKER, in s),
        [Operators.PLUS_ASSIGN]        = (ref readonly SourceInfo s) => new Token(TokenType.AdditionAssignment, Operators.PLUS_ASSIGN, in s),
        [Operators.MINUS_ASSIGN]       = (ref readonly SourceInfo s) => new Token(TokenType.SubtractionAssignment, Operators.MINUS_ASSIGN, in s),
        [Operators.MULTIPLY_ASSIGN]    = (ref readonly SourceInfo s) => new Token(TokenType.MultiplicationAssignment, Operators.MULTIPLY_ASSIGN, in s),
        [Operators.DIVIDE_ASSIGN]      = (ref readonly SourceInfo s) => new Token(TokenType.DivisionAssignment, Operators.DIVIDE_ASSIGN, in s),
        [Operators.MODULO_ASSIGN]      = (ref readonly SourceInfo s) => new Token(TokenType.ModuloAssignment, Operators.MODULO_ASSIGN, in s),
        [Operators.SEMICOLON]          = (ref readonly SourceInfo s) => new Token(TokenType.SemiColon, Operators.SEMICOLON, in s),
        [Operators.COMMA]              = (ref readonly SourceInfo s) => new Token(TokenType.Comma, Operators.COMMA, in s),
        [Operators.OPEN_PARENTHESIS]   = (ref readonly SourceInfo s) => new Token(TokenType.OpenParenthesis, Operators.OPEN_PARENTHESIS, in s),
        [Operators.CLOSE_PARENTHESIS]  = (ref readonly SourceInfo s) => new Token(TokenType.CloseParenthesis, Operators.CLOSE_PARENTHESIS, in s),
        [Operators.OPEN_BRACES]        = (ref readonly SourceInfo s) => new Token(TokenType.OpenBraces, Operators.OPEN_BRACES, in s),
        [Operators.CLOSE_BRACES]       = (ref readonly SourceInfo s) => new Token(TokenType.CloseBraces, Operators.CLOSE_BRACES, in s),
        [Operators.ASSIGNMENT]         = (ref readonly SourceInfo s) => new Token(TokenType.Assignment, Operators.ASSIGNMENT, in s),
        [Operators.PLUS]               = (ref readonly SourceInfo s) => new Token(TokenType.Plus, Operators.PLUS, in s),
        [Operators.MINUS]              = (ref readonly SourceInfo s) => new Token(TokenType.Minus, Operators.MINUS, in s),
        [Operators.MULTIPLICATION]     = (ref readonly SourceInfo s) => new Token(TokenType.Multiplication, Operators.MULTIPLICATION, in s),
        [Operators.DIVISION]           = (ref readonly SourceInfo s) => new Token(TokenType.Division, Operators.DIVISION, in s),
        [Operators.MODULO]             = (ref readonly SourceInfo s) => new Token(TokenType.Modulo, Operators.MODULO, in s),
        [Operators.GREATER_THAN]       = (ref readonly SourceInfo s) => new Token(TokenType.GreaterThan, Operators.GREATER_THAN, in s),
        [Operators.LESS_THAN]          = (ref readonly SourceInfo s) => new Token(TokenType.LessThan, Operators.LESS_THAN, in s),
        [Operators.QUESTION_MARK]      = (ref readonly SourceInfo s) => new Token(TokenType.QuestionMark, Operators.QUESTION_MARK, in s),
        [Operators.NOT]                = (ref readonly SourceInfo s) => new Token(TokenType.Not, Operators.NOT, in s),
        [Operators.NAMESPACE_ACCESSOR] = (ref readonly SourceInfo s) => new Token(TokenType.NamespaceAccessor, Operators.NAMESPACE_ACCESSOR, in s)
    };

    private static readonly Dictionary<string, TokenFactory>.AlternateLookup<ReadOnlySpan<char>> _operatorTokensSpanLookup
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

        var tokenSource = GetCurrentSourceInfo();

        tokens.Add(new Token(TokenType.EOF, string.Empty, in tokenSource));

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
                return operatorFunc(in source);
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

        var errorSource = GetCurrentSourceInfo();
        return ThrowHelper.Throw<UnexpectedCharacterException, Token>(
            $"Unexpected character found: {Current()}",
            in errorSource
        );
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
            return keywordFunc(in sourceStart);
        }

        return new Token(TokenType.Identifier, identifierSpan.ToString(), in sourceStart);
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
                    var errorSource = GetCurrentSourceInfo();
                    ThrowHelper.Throw<UnexpectedCharacterException>(
                        $"Unexpected character found: {Current()}",
                        in errorSource
                    );
                }

                hasDot = true;
            }

            Advance();
        }

        var number = _sourceCode.AsSpan(numberStart, _currentIndex - numberStart);

        if (number[^1] == '.')
        {
            var errorSource = GetCurrentSourceInfo(0, -1);
            ThrowHelper.Throw<UnexpectedCharacterException>(
                "Unexpected character found: .",
                in errorSource
            );
        }

        var tokenType = hasDot
                ? TokenType.DecimalLiteral
                : TokenType.IntegerLiteral;

        return new Token(tokenType, number.ToString(), in sourceStart);
    }

    private Token ProcessString()
    {
        var strBuilder = new StringBuilder(50);
        var sourceStart = GetCurrentSourceInfo();
        Advance();

        if (HasEnded())
        {
            var invalidString = "\"" + StringUtils.UnescapeString(strBuilder.ToString());
            ThrowHelper.Throw<InvalidStringException>($"Invalid string declaration: {invalidString}", in sourceStart);
        }

        while (Current() != '"')
        {
            if (Current() == '\n' || Peek() is null)
            {
                var invalidString = "\"" + StringUtils.UnescapeString(strBuilder.ToString());
                ThrowHelper.Throw<InvalidStringException>($"Invalid string declaration: {invalidString}", in sourceStart);
            }

            if (Current() == '\\')
            {
                strBuilder.Append(GetCurrentEscapedChar());

                if (Peek() is null)
                {
                    var invalidString = "\"" + StringUtils.UnescapeString(strBuilder.ToString());
                    ThrowHelper.Throw<InvalidStringException>($"Invalid string declaration: {invalidString}", in sourceStart);
                }
            }
            else
            {
                strBuilder.Append(Current());
            }

            Advance();
        }

        Advance();

        return new Token(TokenType.StringLiteral, strBuilder.ToString(), in sourceStart);
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

        switch (Current())
        {
            case '"':
                return '"';
            case 'n':
                return '\n';
            case 't':
                return '\t';
            case 'r':
                return '\r';
            case '\\':
                return '\\';
            default:
                var errorSource = GetCurrentSourceInfo(0, -1);
                return ThrowHelper.Throw<UnrecognizedEscapeSequenceException, char>(
                    "\\" + Current(),
                    in errorSource
                );
        }
    }
}