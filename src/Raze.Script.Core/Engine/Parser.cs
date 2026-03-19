using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Tokens;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Engine;

internal class Parser
{
    private readonly Token[] _tokens;
    private readonly string _sourceLocation;

    private int _currentIndex;
    private Token _currentToken;

    private ref readonly Token CurrentToken => ref _currentToken;

    private Parser(Token[] tokens, string sourceLocation)
    {
        if (tokens.Length == 0 || tokens[^1].Type != TokenType.EOF)
        {
            throw new InvalidTokenListException(new SourceInfo(sourceLocation));
        }

        _tokens = tokens;
        _sourceLocation = sourceLocation;

        _currentIndex = 0;
        _currentToken = _tokens[0];
    }

    public static ProgramExpression Parse(Token[] tokens, string sourceLocation)
    {
        var parser = new Parser(tokens, sourceLocation);
        return parser.ParseInternal();
    }

    private ProgramExpression ParseInternal()
    {
        var programBody = new List<Statement>();

        while (!HasEnded())
        {
            if (CurrentToken.Type == TokenType.SemiColon)
            {
                Advance();
                continue;
            }

            programBody.Add(ParseCurrent());

            if (programBody[^1].RequireSemicolon)
            {
                Expect(TokenType.SemiColon, TokenType.EOF);
            }
        }

        var source = programBody.Any()
                        ? programBody.First().SourceInfo
                        : new SourceInfo(_sourceLocation);

        return new ProgramExpression(programBody, source);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Expect(TokenType expected)
    {
        if (CurrentToken.Type != expected)
        {
            ThrowUnexpectedTokenException(expected.GetFriendlyName());
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Expect(TokenType expected1, TokenType expected2)
    {
        if (CurrentToken.Type != expected1 && CurrentToken.Type != expected2)
        {
            ThrowUnexpectedTokenException<object>($"{expected1.GetFriendlyName()} or {expected2.GetFriendlyName()}");
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Expect(Func<TokenType, bool> condition, string expectedPrettyStr)
    {
        if (!condition(CurrentToken.Type))
        {
            ThrowUnexpectedTokenException(expectedPrettyStr);
        }
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void ThrowUnexpectedTokenException(string? expectedPrettyStr, Token? token = null)
    {
        throw CreateUnexpectedTokenException(expectedPrettyStr, (token is null ? CurrentToken : token.Value));
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private T ThrowUnexpectedTokenException<T>(string? expectedPrettyStr, Token? token = null)
    {
        throw CreateUnexpectedTokenException(expectedPrettyStr, (token is null ? CurrentToken : token.Value));
    }

    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static UnexpectedTokenException CreateUnexpectedTokenException(string? expectedPrettyStr, Token token)
    {
        var message = expectedPrettyStr == null
            ? $"Unexpected token found. Found: {token.Type.GetFriendlyName()}. Value: {token.Lexeme}"
            : $"Unexpected token found. Expected: {expectedPrettyStr}. Found: {token.Type.GetFriendlyName()}. Value: {token.Lexeme}";

        return new UnexpectedTokenException(
            message,
            token.SourceInfo
        );
    }

    private void Advance(int howMuch = 1)
    {
        _currentIndex += howMuch;
        _currentToken = _tokens[_currentIndex];
    }

    private bool HasEnded()
    {
        return _currentIndex >= _tokens.Length || CurrentToken.Type == TokenType.EOF;
    }

    private Statement ParseCurrent()
    {
        return CurrentToken.Type switch
        {
            TokenType.NamespaceDeclaration => ParseNamespaceDeclaration(),
            TokenType.Var                  => ParseVariableDeclaration(),
            TokenType.Const                => ParseVariableDeclaration(),
            TokenType.FunctionDeclaration  => ParseFunctionDeclaration(),
            TokenType.OpenBraces           => ParseCodeBlock(),
            TokenType.If                   => ParseIfElse(),
            TokenType.For                  => ParseForLoop(),
            TokenType.While                => ParseWhileLoop(),
            TokenType.Break                => ParseBreakStatement(),
            TokenType.Continue             => ParseContinueStatement(),
            TokenType.Return               => ParseReturnStatement(),
            _                              => ParseAssignmentStatement()
        };
    }

    private NamespaceDeclarationStatement ParseNamespaceDeclaration()
    {
        Expect(TokenType.NamespaceDeclaration);
        var source = CurrentToken.SourceInfo;
        Advance();

        Expect(TokenType.Identifier);
        var identifier = CurrentToken.Lexeme;
        Advance();

        Expect(TokenType.OpenBraces);
        var body = ParseCodeBlock();

        return new NamespaceDeclarationStatement(identifier, body, source);
    }

    private BreakStatement ParseBreakStatement()
    {
        Expect(TokenType.Break);
        var source = CurrentToken.SourceInfo;
        Advance();

        return new BreakStatement(source);
    }

    private ContinueStatement ParseContinueStatement()
    {
        Expect(TokenType.Continue);
        var source = CurrentToken.SourceInfo;
        Advance();

        return new ContinueStatement(source);
    }

    private ReturnStatement ParseReturnStatement()
    {
        Expect(TokenType.Return);
        var sourceStart = CurrentToken.SourceInfo;
        Advance();
        
        if (CurrentToken.Type == TokenType.SemiColon || CurrentToken.Type == TokenType.EOF)
        {
            return new ReturnStatement(null, sourceStart);
        }

        var returnedValue = ParseOperatorExpression();

        return new ReturnStatement(returnedValue, sourceStart);
    }

    private CodeBlockStatement ParseCodeBlock()
    {
        Expect(TokenType.OpenBraces);
        var sourceStart = CurrentToken.SourceInfo;
        Advance();

        var codeBlockBody = new List<Statement>();

        while (!HasEnded() && CurrentToken.Type != TokenType.CloseBraces)
        {
            if (CurrentToken.Type == TokenType.SemiColon)
            {
                Advance();
                continue;
            }

            codeBlockBody.Add(ParseCurrent());

            if (codeBlockBody[^1].RequireSemicolon)
            {
                Expect(TokenType.SemiColon);
            }
        }

        Expect(TokenType.CloseBraces);
        Advance();

        return new CodeBlockStatement(codeBlockBody, sourceStart);
    }

    private IfElseStatement ParseIfElse()
    {
        Expect(TokenType.If);
        var sourceStart = CurrentToken.SourceInfo;
        Advance();

        Expect(TokenType.OpenParenthesis);
        Advance();

        Expression condition = ParseOperatorExpression();

        Expect(TokenType.CloseParenthesis);
        Advance();

        Expect(TokenType.OpenBraces);
        CodeBlockStatement then = ParseCodeBlock();

        Statement? elseStmt = null;

        if (CurrentToken.Type == TokenType.Else)
        {
            Advance();

            elseStmt = CurrentToken.Type switch
            {
                TokenType.OpenBraces => ParseCodeBlock(),
                TokenType.If         => ParseIfElse(),
                _ => ThrowUnexpectedTokenException<Statement>(null)
            };
        }

        return new IfElseStatement(condition, then, elseStmt, sourceStart);
    }

    private LoopStatement ParseForLoop()
    {
        Expect(TokenType.For);
        var sourceStart = CurrentToken.SourceInfo;
        Advance();

        Expect(TokenType.OpenParenthesis);
        Advance();

        List<Statement> initialization = [];

        if (CurrentToken.Type != TokenType.SemiColon)
        {
            initialization.Add(
                CurrentToken.Type == TokenType.Var
                    ? ParseVariableDeclaration()
                    : ParseAssignmentStatement()
            );
        }

        Expect(TokenType.SemiColon);
        Advance();

        Expression? condition = null;

        if (CurrentToken.Type != TokenType.SemiColon)
        {
            condition = ParseOperatorExpression();
        }

        Expect(TokenType.SemiColon);
        Advance();

        Statement? update = null;

        if (CurrentToken.Type != TokenType.CloseParenthesis)
        {
            update = ParseAssignmentStatement();
        }

        Expect(TokenType.CloseParenthesis);
        Advance();

        CodeBlockStatement body = ParseCodeBlock();

        return new LoopStatement(initialization, condition, update, body, sourceStart);
    }

    private LoopStatement ParseWhileLoop()
    {
        Expect(TokenType.While);
        var sourceStart = CurrentToken.SourceInfo;
        Advance();

        Expect(TokenType.OpenParenthesis);
        Advance();

        Expression condition = ParseOperatorExpression();

        Expect(TokenType.CloseParenthesis);
        Advance();

        CodeBlockStatement body = ParseCodeBlock();

        return new LoopStatement([], condition, null, body, sourceStart);
    }

    private VariableDeclarationStatement ParseVariableDeclaration()
    {
        Expect(TokenType.Var, TokenType.Const);
        bool isConstant = CurrentToken.Type == TokenType.Const;
        var sourceStart = CurrentToken.SourceInfo;
        Advance();

        RuntimeType type = ParseType();

        Expect(TokenType.Identifier);
        string identifier = CurrentToken.Lexeme;
        Advance();

        if (CurrentToken.Type != TokenType.Assignment)
        {
            return new VariableDeclarationStatement(identifier, type, null, isConstant, sourceStart);
        }

        Advance();
        Expression value = ParseOperatorExpression();

        return new VariableDeclarationStatement(identifier, type, value, isConstant, sourceStart);
    }

    private FunctionDeclarationStatement ParseFunctionDeclaration()
    {
        Expect(TokenType.FunctionDeclaration);
        var sourcestart = CurrentToken.SourceInfo;
        Advance();

        var returnType = ParseType();

        Expect(TokenType.Identifier);
        string identifier = CurrentToken.Lexeme;
        Advance();

        var parameterList = ParseParameterList();

        var functionBody = ParseCodeBlock();

        return new FunctionDeclarationStatement(
            identifier, returnType, parameterList, functionBody, sourcestart
        );
    }

    private List<ParameterSymbol> ParseParameterList()
    {
        Expect(TokenType.OpenParenthesis);
        Advance();

        List<ParameterSymbol> parameterList = [];
        bool isDefaultParameterRequired = false;

        while (CurrentToken.Type == TokenType.Const || CurrentToken.Type.IsPrimitiveTypeKeyword())
        {
            var sourceStart = CurrentToken.SourceInfo;

            bool isConstant = false;

            if (CurrentToken.Type == TokenType.Const)
            {
                isConstant = true;
                Advance();
            }

            RuntimeType type = ParseType();

            Expect(TokenType.Identifier);
            string identifier = CurrentToken.Lexeme;
            Advance();

            Expression? defaultValue = null;
            if (CurrentToken.Type == TokenType.Assignment)
            {
                Advance();
                defaultValue = ParseOperatorExpression();

                isDefaultParameterRequired = true;
            }
            else if (isDefaultParameterRequired)
            {
                throw new InvalidParameterListException(
                    "A non-default parameter cannot appear after a default parameter", sourceStart
                );
            }

            if (CurrentToken.Type == TokenType.Comma)
            {
                Advance();
                Expect(
                    t => t == TokenType.Const || t.IsPrimitiveTypeKeyword(),
                    $"{TokenType.Const.GetFriendlyName()} or type name"
                );
            }

            parameterList.Add(
                new ParameterSymbol(
                    isConstant,
                    type,
                    identifier,
                    defaultValue,
                    ref sourceStart
                )
            );
        }

        Expect(TokenType.CloseParenthesis);
        Advance();

        return parameterList;
    }

    private RuntimeType ParseType()
    {
        Expect(t => t.IsPrimitiveTypeKeyword(), "type name");
        var typeToken = CurrentToken;
        Advance();

        List<RuntimeType> generics = [];
        if (CurrentToken.Type == TokenType.LessThan)
        {
            if (typeToken.Type != TokenType.FunctionTypeName)
            {
                throw new InvalidTypeDeclarationException("This type cannot have generics", CurrentToken.SourceInfo);
            }

            generics = ParseGenerics();
        }

        if (generics.Count == 0 && typeToken.Type == TokenType.FunctionTypeName)
        {
            throw new InvalidTypeDeclarationException(
                "Function type requires at least one generic",
                typeToken.SourceInfo
            );
        }

        var isNullable = false;
        if (CurrentToken.Type == TokenType.QuestionMark)
        {
            if (typeToken.Type == TokenType.VoidTypeName)
            {
                throw new InvalidTypeDeclarationException("void cannot be nullable", CurrentToken!.SourceInfo);
            }

            isNullable = true;
            Advance();
        }

        return (typeToken.Type) switch
        {
            TokenType.BooleanTypeName  => (isNullable) ? RuntimeType.NullableBoolean : RuntimeType.Boolean,
            TokenType.DecimalTypeName  => (isNullable) ? RuntimeType.NullableDecimal : RuntimeType.Decimal,
            TokenType.IntegerTypeName  => (isNullable) ? RuntimeType.NullableInteger : RuntimeType.Integer,
            TokenType.StringTypeName   => (isNullable) ? RuntimeType.NullableString : RuntimeType.String,
            TokenType.VoidTypeName     => RuntimeType.Void,
            TokenType.FunctionTypeName => TypeFactory.GetType(BaseType.UserFunction, isNullable, generics.ToArray()),
            _ => ThrowUnexpectedTokenException<RuntimeType>("type name", typeToken)
        };
    }

    private List<RuntimeType> ParseGenerics()
    {
        Expect(TokenType.LessThan);
        Advance();

        Expect(
            t => t == TokenType.GreaterThan || t.IsPrimitiveTypeKeyword(),
            $"{TokenType.GreaterThan.GetFriendlyName()} or type name"
        );

        List<RuntimeType> generics = [];
        while (CurrentToken.Type != TokenType.GreaterThan)
        {
            Expect(
                t => t.IsPrimitiveTypeKeyword() || t == TokenType.Comma,
                $"type name or {TokenType.Comma.GetFriendlyName()}"
            );

            if (CurrentToken.Type == TokenType.Comma)
            {
                Advance();
                Expect(t => t.IsPrimitiveTypeKeyword(), "type name");
                continue;
            }

            generics.Add(ParseType());

            Expect(TokenType.Comma, TokenType.GreaterThan);
        }

        Advance();

        return generics;
    }

    private Statement ParseAssignmentStatement()
    {
        Expression left = ParseOperatorExpression();

        if (!CurrentToken.Type.IsAssignmentOrCompoundAssignmentOperator())
        {
            return left;
        }

        var op = CurrentToken;
        Advance();

        Expression value = ParseOperatorExpression();

        if (left is not IdentifierExpression)
        {
            throw new InvalidAssignmentException("Invalid assignment target", left.SourceInfo);
        }

        if (op.Type.IsCompoundAssignmentOperator())
        {
            value = new BinaryExpression(left, op.Type.GetCompoundAssignmentTokenOperator(), value, op.SourceInfo);
        }
        
        return new AssignmentStatement((IdentifierExpression)left, value, left.SourceInfo);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Expression ParseOperatorExpression()
    {
        return ParseOrExpression();
    }

    private Expression ParseOrExpression()
    {
        return ParseBinaryExpression(ParseAndExpression, t => t == TokenType.Or);
    }

    private Expression ParseAndExpression()
    {
        return ParseBinaryExpression(ParseEqualityExpression, t => t == TokenType.And);
    }

    private Expression ParseEqualityExpression()
    {
        return ParseBinaryExpression(ParseRelationalExpression, TokenTypeFacts.IsEqualityOperator);
    }

    private Expression ParseRelationalExpression()
    {
        return ParseBinaryExpression(ParseAdditiveExpression, TokenTypeFacts.IsRelationalOperator);
    }

    private Expression ParseAdditiveExpression()
    {
        return ParseBinaryExpression(ParseMultiplicativeExpression, TokenTypeFacts.IsAdditiveOperator);
    }

    private Expression ParseMultiplicativeExpression()
    {
        return ParseBinaryExpression(ParseUnaryExpression, TokenTypeFacts.IsMultiplicativeOperator);
    }

    private Expression ParseUnaryExpression()
    {
        if (CurrentToken.Type.IsUnaryOperator())
        {
            var op = CurrentToken;
            Advance();

            var operand = ParseUnaryExpression();

            return new UnarySimpleExpression(operand, op.Lexeme, isPostfix: false, op.SourceInfo);
        }

        if (CurrentToken.Type.IsPrefixOperator())
        {
            var op = CurrentToken;
            Advance();

            var operand = ParseUnaryExpression();

            if (operand is not IdentifierExpression identifier)
            {
                throw new InvalidOperandException(
                    $"The {op.Lexeme} operator can only be applied to identifiers",
                    op.SourceInfo
                );
            }

            return new UnaryMutationExpression(identifier, op.Lexeme, isPostfix: false, op.SourceInfo);
        }

        return ParsePostfixExpression();
    }

    private Expression ParsePostfixExpression()
    {
        var expr = ParseMemberExpression();

        while (true)
        {
            if (CurrentToken.Type == TokenType.OpenParenthesis)
            {
                expr = ParseCallExpression(expr);
                continue;
            }

            if (CurrentToken.Type.IsPostfixOperator())
            {
                var op = CurrentToken;
                Advance();

                if (expr is not IdentifierExpression)
                {
                    throw new InvalidOperandException(
                        $"The {op.Lexeme} operator can only be applied to identifiers",
                        op.SourceInfo
                    );
                }

                expr = op.Type == TokenType.NullChecker
                    ? new NullCheckerExpression((IdentifierExpression)expr, expr.SourceInfo)
                    : new UnaryMutationExpression((IdentifierExpression)expr, op.Lexeme, isPostfix: true, expr.SourceInfo);
                continue;
            }

            break;
        }

        return expr;
    }

    private CallExpression ParseCallExpression(Expression caller)
    {
        Expect(TokenType.OpenParenthesis);
        Advance();

        List<Expression> argumentList = [];

        while (CurrentToken.Type != TokenType.CloseParenthesis)
        {
            argumentList.Add(ParseOperatorExpression());

            if (CurrentToken.Type == TokenType.Comma)
            {
                Advance();

                if (CurrentToken.Type == TokenType.CloseParenthesis)
                {
                    ThrowUnexpectedTokenException(null);
                }
            }
        }

        Expect(TokenType.CloseParenthesis);
        Advance();

        return new CallExpression(caller, argumentList, caller.SourceInfo);
    }

    private Expression ParseMemberExpression()
    {
        var obj = ParsePrimaryExpression();

        if (CurrentToken.Type == TokenType.NamespaceAccessor)
        {
            return ParseNamespaceAccessExpression(obj);
        }

        return obj;
    }

    private NamespaceAccessExpression ParseNamespaceAccessExpression(Expression namespaceIdentifier)
    {
        Expect(TokenType.NamespaceAccessor);
        var op = CurrentToken;
        Advance();

        if (namespaceIdentifier is not IdentifierExpression)
        {
            throw new InvalidOperandException(
                $"The {op.Lexeme} operator can only be applied to identifiers",
                op.SourceInfo
            );
        }

        Expect(TokenType.Identifier);
        var memberIdentifier = ParsePrimaryExpression();

        return new NamespaceAccessExpression(
            (namespaceIdentifier as IdentifierExpression)!,
            (memberIdentifier as IdentifierExpression)!,
            namespaceIdentifier.SourceInfo
        );
    }

    private Expression ParsePrimaryExpression()
    {
        switch (CurrentToken.Type)
        {
            case TokenType.Identifier:
                var identifierExpression = new IdentifierExpression(CurrentToken.Lexeme, CurrentToken.SourceInfo);
                Advance();
                return identifierExpression;

            case TokenType.IntegerLiteral:
                var integerValue = Int128.Parse(CurrentToken.Lexeme);
                var integerLiteralExpression = new IntegerLiteralExpression(integerValue, CurrentToken.SourceInfo);
                Advance();
                return integerLiteralExpression;

            case TokenType.DecimalLiteral:
                var decimalValue = decimal.Parse(CurrentToken.Lexeme, CultureInfo.InvariantCulture);
                var decimalLiteralExpression = new DecimalLiteralExpression(decimalValue, CurrentToken.SourceInfo);
                Advance();
                return decimalLiteralExpression;

            case TokenType.BooleanLiteral:
                var boolValue = bool.Parse(CurrentToken.Lexeme);
                var booleanLiteralExpression = new BooleanLiteralExpression(boolValue, CurrentToken.SourceInfo);
                Advance();
                return booleanLiteralExpression;

            case TokenType.StringLiteral:
                var stringLiteralExpression = new StringLiteralExpression(CurrentToken.Lexeme, CurrentToken.SourceInfo);
                Advance();
                return stringLiteralExpression;

            case TokenType.NullLiteral:
                var nullLiteralExpression = new NullLiteralExpression(CurrentToken.SourceInfo);
                Advance();
                return nullLiteralExpression;

            case TokenType.OpenParenthesis:
                return ParseParenthesis();

            default:
                return ThrowUnexpectedTokenException<Expression>(null);
        }
    }

    private Expression ParseParenthesis()
    {
        Expect(TokenType.OpenParenthesis);
        Advance();

        Expression expr = ParseOperatorExpression();

        Expect(TokenType.CloseParenthesis);
        Advance();

        return expr;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Expression ParseBinaryExpression(Func<Expression> next, Func<TokenType, bool> tokenCheck)
    {
        Expression left = next();

        while (tokenCheck(CurrentToken.Type))
        {
            Token op = CurrentToken;
            Advance();

            Expression right = next();

            left = new BinaryExpression(left, op.Lexeme, right, left.SourceInfo);
        }

        return left;
    }
}
