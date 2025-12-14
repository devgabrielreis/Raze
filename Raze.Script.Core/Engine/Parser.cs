using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Symbols;
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
using Raze.Script.Core.Types;
using System.Globalization;

namespace Raze.Script.Core.Engine;

internal class Parser
{
    private readonly IList<Token> _tokens;
    private readonly string _sourceLocation;

    private int _currentIndex = 0;

    public Parser(IList<Token> tokens, string sourceLocation)
    {
        if (tokens.Count == 0 || tokens.Last() is not EOFToken)
        {
            throw new InvalidTokenListException(new SourceInfo(sourceLocation));
        }

        _tokens = tokens;
        _sourceLocation = sourceLocation;
    }

    public ProgramExpression Parse()
    {
        var programBody = new List<Statement>();

        Reset();

        while (!HasEnded())
        {
            if (Current() is SemiColonToken)
            {
                Advance();
                continue;
            }

            programBody.Add(ParseCurrent());

            if (programBody.Last().RequireSemicolon)
            {
                Expect<SemiColonToken, EOFToken>();
            }
        }

        var source = programBody.Any()
                        ? programBody.First().SourceInfo
                        : new SourceInfo(_sourceLocation);

        return new ProgramExpression(programBody, source);
    }

    private void Reset()
    {
        _currentIndex = 0;
    }

    private Token Current()
    {
        return _tokens[_currentIndex];
    }

    private Token? Peek(int howMuch = 1)
    {
        int target = _currentIndex + howMuch;

        if (target >= _tokens.Count)
        {
            return null;
        }

        return _tokens[target];
    }

    private void Expect<T>() where T : Token
    {
        if (Current() is not T)
        {
            throw new UnexpectedTokenException(
                Current().GetType().Name,
                typeof(T).Name,
                Current().Lexeme,
                Current().SourceInfo
            );
        }
    }

    private void Expect<T1, T2>() where T1 : Token where T2 : Token
    {
        if (Current() is not T1 && Current() is not T2)
        {
            throw new UnexpectedTokenException(
                Current().GetType().Name,
                $"{typeof(T1).Name} or {typeof(T2).Name}",
                Current().Lexeme,
                Current().SourceInfo
            );
        }
    }

    private void Advance(int howMuch = 1)
    {
        _currentIndex += howMuch;
    }

    private bool HasEnded()
    {
        return _currentIndex >= _tokens.Count || Current() is EOFToken;
    }

    private Statement ParseCurrent()
    {
        return Current() switch
        {
            VariableDeclarationToken => ParseVariableDeclaration(),
            FunctionDeclarationToken => ParseFunctionDeclaration(),
            OpenBracesToken          => ParseCodeBlock(),
            IfToken                  => ParseIfElse(),
            ForToken                 => ParseForLoop(),
            WhileToken               => ParseWhileLoop(),
            BreakToken               => ParseBreakStatement(),
            ContinueToken            => ParseContinueStatement(),
            ReturnToken              => ParseReturnStatement(),
            _                        => ParseAssignmentStatement()
        };
    }

    private BreakStatement ParseBreakStatement()
    {
        Expect<BreakToken>();
        var source = Current().SourceInfo;
        Advance();

        return new BreakStatement(source);
    }

    private ContinueStatement ParseContinueStatement()
    {
        Expect<ContinueToken>();
        var source = Current().SourceInfo;
        Advance();

        return new ContinueStatement(source);
    }

    private ReturnStatement ParseReturnStatement()
    {
        Expect<ReturnToken>();
        var sourceStart = Current().SourceInfo;
        Advance();
        
        if (Current() is SemiColonToken || Current() is EOFToken)
        {
            return new ReturnStatement(null, sourceStart);
        }

        var returnedValue = ParseOperatorExpression();

        return new ReturnStatement(returnedValue, sourceStart);
    }

    private CodeBlockStatement ParseCodeBlock()
    {
        Expect<OpenBracesToken>();
        var sourceStart = Current().SourceInfo;
        Advance();

        var codeBlockBody = new List<Statement>();

        while (!HasEnded() && !(Current() is CloseBracesToken))
        {
            if (Current() is SemiColonToken)
            {
                Advance();
                continue;
            }

            codeBlockBody.Add(ParseCurrent());

            if (codeBlockBody.Last().RequireSemicolon)
            {
                Expect<SemiColonToken>();
            }
        }

        Expect<CloseBracesToken>();
        Advance();

        return new CodeBlockStatement(codeBlockBody, sourceStart);
    }

    private IfElseStatement ParseIfElse()
    {
        Expect<IfToken>();
        var sourceStart = Current().SourceInfo;
        Advance();

        Expect<OpenParenthesisToken>();
        Advance();

        Expression condition = ParseOperatorExpression();

        Expect<CloseParenthesisToken>();
        Advance();

        Expect<OpenBracesToken>();
        CodeBlockStatement then = ParseCodeBlock();

        Statement? elseStmt = null;

        if (Current() is ElseToken)
        {
            Advance();

            elseStmt = Current() switch
            {
                OpenBracesToken => ParseCodeBlock(),
                IfToken         => ParseIfElse(),
                _ => throw new UnexpectedTokenException(
                    Current().GetType().Name, Current().Lexeme, Current().SourceInfo
                )
            };
        }

        return new IfElseStatement(condition, then, elseStmt, sourceStart);
    }

    private LoopStatement ParseForLoop()
    {
        Expect<ForToken>();
        var sourceStart = Current().SourceInfo;
        Advance();

        Expect<OpenParenthesisToken>();
        Advance();

        List<Statement> initialization = [];

        if (Current() is not SemiColonToken)
        {
            initialization.Add(
                Current() is VariableDeclarationToken
                    ? ParseVariableDeclaration()
                    : ParseAssignmentStatement()
            );
        }

        Expect<SemiColonToken>();
        Advance();

        Expression? condition = null;

        if (Current() is not SemiColonToken)
        {
            condition = ParseOperatorExpression();
        }

        Expect<SemiColonToken>();
        Advance();

        Statement? update = null;

        if (Current() is not CloseParenthesisToken)
        {
            update = ParseAssignmentStatement();
        }

        Expect<CloseParenthesisToken>();
        Advance();

        CodeBlockStatement body = ParseCodeBlock();

        return new LoopStatement(initialization, condition, update, body, sourceStart);
    }

    private LoopStatement ParseWhileLoop()
    {
        Expect<WhileToken>();
        var sourceStart = Current().SourceInfo;
        Advance();

        Expect<OpenParenthesisToken>();
        Advance();

        Expression condition = ParseOperatorExpression();

        Expect<CloseParenthesisToken>();
        Advance();

        CodeBlockStatement body = ParseCodeBlock();

        return new LoopStatement([], condition, null, body, sourceStart);
    }

    private VariableDeclarationStatement ParseVariableDeclaration()
    {
        Expect<VariableDeclarationToken>();
        bool isConstant = Current() is ConstToken;
        var sourceStart = Current().SourceInfo;
        Advance();

        RuntimeType type = ParseType();

        Expect<IdentifierToken>();
        string identifier = Current().Lexeme;
        Advance();

        if (Current() is not AssignmentToken)
        {
            return new VariableDeclarationStatement(identifier, type, null, isConstant, sourceStart);
        }

        Advance();
        Expression value = ParseOperatorExpression();

        return new VariableDeclarationStatement(identifier, type, value, isConstant, sourceStart);
    }

    private FunctionDeclarationStatement ParseFunctionDeclaration()
    {
        Expect<FunctionDeclarationToken>();
        var sourcestart = Current().SourceInfo;
        Advance();

        var returnType = ParseType();

        Expect<IdentifierToken>();
        string identifier = Current().Lexeme;
        Advance();

        var parameterList = ParseParameterList();

        var functionBody = ParseCodeBlock();

        return new FunctionDeclarationStatement(
            identifier, returnType, parameterList, functionBody, sourcestart
        );
    }

    private List<ParameterSymbol> ParseParameterList()
    {
        Expect<OpenParenthesisToken>();
        Advance();

        List<ParameterSymbol> parameterList = [];
        bool isDefaultParameterRequired = false;

        while (Current() is ConstToken || Current() is PrimitiveTypeToken)
        {
            var sourceStart = Current().SourceInfo;

            bool isConstant = false;

            if (Current() is ConstToken)
            {
                isConstant = true;
                Advance();
            }

            RuntimeType type = ParseType();

            Expect<IdentifierToken>();
            string identifier = Current().Lexeme;
            Advance();

            Expression? defaultValue = null;
            if (Current() is AssignmentToken)
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

            if (Current() is CommaToken)
            {
                Advance();
                Expect<ConstToken, PrimitiveTypeToken>();
            }

            parameterList.Add(
                new ParameterSymbol(
                    isConstant,
                    type,
                    identifier,
                    defaultValue,
                    sourceStart
                )
            );
        }

        Expect<CloseParenthesisToken>();
        Advance();

        return parameterList;
    }

    private RuntimeType ParseType()
    {
        Expect<PrimitiveTypeToken>();
        PrimitiveTypeToken type = (Current() as PrimitiveTypeToken)!;
        Advance();

        var isNullable = false;
        if (Current() is QuestionMarkToken)
        {
            if (type is VoidPrimitiveToken)
            {
                throw new InvalidTypeDeclarationException("void cannot be nullable", Current()!.SourceInfo);
            }

            isNullable = true;
            Advance();
        }

        if (Current() is LessThanToken)
        {
            return type switch
            {
                FunctionPrimitiveToken => ParseFunctionType(isNullable, type.SourceInfo),
                _ => throw new InvalidTypeDeclarationException("This type cannot have generics", Current()!.SourceInfo)
            };
        }
        else if (type is FunctionPrimitiveToken)
        {
            throw new InvalidTypeDeclarationException(
                "Function type requires at least one generic",
                type.SourceInfo
            );
        }

        return (type) switch
        {
            BooleanPrimitiveToken => new BooleanType(isNullable),
            DecimalPrimitiveToken => new DecimalType(isNullable),
            IntegerPrimitiveToken => new IntegerType(isNullable),
            StringPrimitiveToken  => new StringType(isNullable),
            VoidPrimitiveToken    => new VoidType(),
            _ => throw new UnexpectedTokenException(type.GetType().Name, type.Lexeme, type.SourceInfo)
        };
    }

    private FunctionType ParseFunctionType(bool isNullable, SourceInfo declarationStart)
    {
        Expect<LessThanToken>();
        var generics = ParseGenerics();

        if (generics.Count == 0)
        {
            throw new InvalidTypeDeclarationException("Function type requires at least one generic", declarationStart);
        }

        var returnType = generics[generics.Count - 1];
        generics.RemoveAt(generics.Count - 1);

        return new FunctionType(isNullable, returnType, generics);
    }

    private List<RuntimeType> ParseGenerics()
    {
        Expect<LessThanToken>();
        Advance();

        Expect<PrimitiveTypeToken, GreaterThanToken>();

        List<RuntimeType> generics = [];
        while (Current() is not GreaterThanToken)
        {
            Expect<PrimitiveTypeToken, CommaToken>();

            if (Current() is CommaToken)
            {
                Advance();
                Expect<PrimitiveTypeToken>();
                continue;
            }

            generics.Add(ParseType());
        }

        Advance();

        return generics;
    }

    private Statement ParseAssignmentStatement()
    {
        Expression left = ParseOperatorExpression();

        if (Current() is not AssignmentToken)
        {
            return left;
        }

        Advance();
        Expression value = ParseOperatorExpression();
        
        return new AssignmentStatement(left, value, left.SourceInfo);
    }

    private Expression ParseOperatorExpression()
    {
        return ParseOrExpression();
    }

    private Expression ParseOrExpression()
    {
        return ParseBinaryExpression<OrToken>(ParseAndExpression);
    }

    private Expression ParseAndExpression()
    {
        return ParseBinaryExpression<AndToken>(ParseEqualityExpression);
    }

    private Expression ParseEqualityExpression()
    {
        return ParseBinaryExpression<EqualityOperatorToken>(ParseRelationalExpression);
    }

    private Expression ParseRelationalExpression()
    {
        return ParseBinaryExpression<RelationalOperatorToken>(ParseAdditiveExpression);
    }

    private Expression ParseAdditiveExpression()
    {
        return ParseBinaryExpression<AdditiveOperatorToken>(ParseMultiplicativeExpression);
    }

    private Expression ParseMultiplicativeExpression()
    {
        return ParseBinaryExpression<MultiplicativeOperatorToken>(ParseCallMemberExpression);
    }

    private Expression ParseCallMemberExpression()
    {
        var member = ParseMemberExpression();

        while (Current() is OpenParenthesisToken)
        {
            member = ParseCallExpression(member);
        }

        return member;
    }

    private CallExpression ParseCallExpression(Expression caller)
    {
        Expect<OpenParenthesisToken>();
        Advance();

        List<Expression> argumentList = [];

        while (Current() is not CloseParenthesisToken)
        {
            argumentList.Add(ParseOperatorExpression());

            if (Current() is CommaToken)
            {
                Advance();

                if (Current() is CloseParenthesisToken)
                {
                    throw new UnexpectedTokenException(
                        Current().GetType().Name, Current().Lexeme, Current().SourceInfo
                    );
                }
            }
        }

        Expect<CloseParenthesisToken>();
        Advance();

        return new CallExpression(caller, argumentList, caller.SourceInfo);
    }

    private Expression ParseMemberExpression()
    {
        var obj = ParsePrimaryExpression();

        return obj;
    }

    private Expression ParsePrimaryExpression()
    {
        switch (Current())
        {
            case IdentifierToken:
                var identifierExpression = new IdentifierExpression(Current().Lexeme, Current().SourceInfo);
                Advance();
                return identifierExpression;

            case IntegerLiteralToken:
                var integerValue = Int128.Parse(Current().Lexeme);
                var integerLiteralExpression = new IntegerLiteralExpression(integerValue, Current().SourceInfo);
                Advance();
                return integerLiteralExpression;

            case DecimalLiteralToken:
                var decimalValue = decimal.Parse(Current().Lexeme, CultureInfo.InvariantCulture);
                var decimalLiteralExpression = new DecimalLiteralExpression(decimalValue, Current().SourceInfo);
                Advance();
                return decimalLiteralExpression;

            case BooleanLiteralToken:
                var boolValue = bool.Parse(Current().Lexeme);
                var booleanLiteralExpression = new BooleanLiteralExpression(boolValue, Current().SourceInfo);
                Advance();
                return booleanLiteralExpression;

            case StringLiteralToken:
                var stringLiteralExpression = new StringLiteralExpression(Current().Lexeme, Current().SourceInfo);
                Advance();
                return stringLiteralExpression;

            case NullLiteralToken:
                var nullLiteralExpression = new NullLiteralExpression(Current().SourceInfo);
                Advance();
                return nullLiteralExpression;

            case OpenParenthesisToken:
                return ParseParenthesis();

            default:
                throw new UnexpectedTokenException(
                    Current().GetType().Name, Current().Lexeme, Current().SourceInfo
                );
        }
    }

    private Expression ParseParenthesis()
    {
        Expect<OpenParenthesisToken>();
        Advance();

        Expression expr = ParseOperatorExpression();

        Expect<CloseParenthesisToken>();
        Advance();

        return expr;
    }

    private Expression ParseBinaryExpression<TOperator>(Func<Expression> next) where TOperator : OperatorToken
    {
        Expression left = next();

        while (Current() is TOperator)
        {
            OperatorToken op = (Current() as OperatorToken)!;
            Advance();

            Expression right = next();

            left = new BinaryExpression(left, op, right, left.SourceInfo);
        }

        return left;
    }
}
