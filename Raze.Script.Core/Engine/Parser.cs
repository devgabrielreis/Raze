using Raze.Script.Core.Exceptions.ParseExceptions;
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
    public bool HasProcessed { get; private set; } = false;

    private readonly IList<Token> _tokens;
    private int _currentIndex = 0;
    private ProgramExpression _program = new();

    public Parser(IList<Token> tokens)
    {
        if (tokens.Count == 0 || tokens.Last() is not EOFToken)
        {
            throw new InvalidTokenListException();
        }

        _tokens = tokens;
    }

    public void Reset()
    {
        _currentIndex = 0;
        _program = new();

        HasProcessed = false;
    }

    public ProgramExpression Parse()
    {
        if (HasProcessed)
        {
            return _program;
        }

        HasProcessed = true;

        _program = new();

        while (!HasEnded())
        {
            if (Current() is SemiColonToken)
            {
                Advance();
                continue;
            }

            _program.Body.Add(ParseCurrent());
            Advance();

            if (_program.Body.Last().RequireSemicolon)
            {
                Expect<SemiColonToken, EOFToken>();
            }
        }

        return _program;
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
                Current().Line,
                Current().Column
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
                Current().Line,
                Current().Column
            );
        }
    }

    private void Advance(int howMuch = 1)
    {
        _currentIndex += howMuch;
    }

    private void Recede()
    {
        _currentIndex--;
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
            BreakToken               => new BreakStatement(Current().Line, Current().Column),
            ContinueToken            => new ContinueStatement(Current().Line, Current().Column),
            ReturnToken              => ParseReturnStatement(),
            _                        => ParseAssignmentStatement()
        };
    }

    private ReturnStatement ParseReturnStatement()
    {
        Expect<ReturnToken>();
        int startLine = Current().Line;
        int startColumn = Current().Column;
        
        if (Peek() is SemiColonToken || Peek() is EOFToken)
        {
            return new ReturnStatement(null, startLine, startColumn);
        }

        Advance();
        var returnedValue = ParseOrExpression();

        return new ReturnStatement(returnedValue, startLine, startColumn);
    }

    private CodeBlockStatement ParseCodeBlock()
    {
        Expect<OpenBracesToken>();
        var codeBlock = new CodeBlockStatement(Current().Line, Current().Column);
        Advance();

        while (!HasEnded() && !(Current() is CloseBracesToken))
        {
            if (Current() is SemiColonToken)
            {
                Advance();
                continue;
            }

            codeBlock.Body.Add(ParseCurrent());

            Advance();

            if (codeBlock.Body.Last().RequireSemicolon)
            {
                Expect<SemiColonToken>();
            }
        }

        Expect<CloseBracesToken>();

        return codeBlock;
    }

    private IfElseStatement ParseIfElse()
    {
        Expect<IfToken>();
        int startLine = Current().Line;
        int startColumn = Current().Column;
        Advance();

        Expect<OpenParenthesisToken>();
        Advance();

        Expression condition = ParseOrExpression();
        Advance();

        Expect<CloseParenthesisToken>();
        Advance();

        Expect<OpenBracesToken>();
        CodeBlockStatement then = ParseCodeBlock();

        Statement? elseStmt = null;

        if (Peek() is ElseToken)
        {
            Advance(2);

            elseStmt = Current() switch
            {
                OpenBracesToken => ParseCodeBlock(),
                IfToken         => ParseIfElse(),
                _ => throw new UnexpectedTokenException(
                    Current().GetType().Name, Current().Lexeme, Current().Line, Current().Column
                )
            };
        }

        return new IfElseStatement(condition, then, elseStmt, startLine, startColumn);
    }

    private LoopStatement ParseForLoop()
    {
        Expect<ForToken>();

        int startLine = Current().Line;
        int startColumn = Current().Column;

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

            Advance();
        }

        Expect<SemiColonToken>();
        Advance();

        Expression? condition = null;

        if (Current() is not SemiColonToken)
        {
            condition = ParseOrExpression();
            Advance();
        }

        Expect<SemiColonToken>();
        Advance();

        Statement? update = null;

        if (Current() is not CloseParenthesisToken)
        {
            update = ParseAssignmentStatement();
            Advance();
        }

        Expect<CloseParenthesisToken>();
        Advance();

        CodeBlockStatement body = ParseCodeBlock();

        return new LoopStatement(initialization, condition, update, body, startLine, startColumn);
    }

    private LoopStatement ParseWhileLoop()
    {
        Expect<WhileToken>();

        int startLine = Current().Line;
        int startColumn = Current().Column;

        Advance();

        Expect<OpenParenthesisToken>();
        Advance();

        Expression condition = ParseOrExpression();
        Advance();

        Expect<CloseParenthesisToken>();
        Advance();

        CodeBlockStatement body = ParseCodeBlock();
        List<Statement> initialization = [];

        return new LoopStatement(initialization, condition, null, body, startLine, startColumn);
    }

    private VariableDeclarationStatement ParseVariableDeclaration()
    {
        Expect<VariableDeclarationToken>();

        bool isConstant = Current() is ConstToken;
        int startLine = Current().Line;
        int startColumn = Current().Column;
        Advance();

        RuntimeType type = ParseType();
        Advance();

        Expect<IdentifierToken>();
        string identifier = Current().Lexeme;
        Advance();

        if (Current() is not AssignmentToken)
        {
            Recede();
            return new VariableDeclarationStatement(identifier, type, null, isConstant, startLine, startColumn);
        }

        Advance();
        Expression value = ParseOrExpression();

        return new VariableDeclarationStatement(identifier, type, value, isConstant, startLine, startColumn);
    }

    private FunctionDeclarationStatement ParseFunctionDeclaration()
    {
        Expect<FunctionDeclarationToken>();
        int startLine = Current().Line;
        int startColumn = Current().Column;
        Advance();

        var returnType = ParseType();
        Advance();

        Expect<IdentifierToken>();
        string identifier = Current().Lexeme;
        Advance();

        var parameterList = ParseParameterList();
        Advance();

        var functionBody = ParseCodeBlock();

        return new FunctionDeclarationStatement(
            identifier, returnType, parameterList, functionBody, startLine, startColumn
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
            int startLine = Current().Line;
            int startColumn = Current().Column;

            bool isConstant = false;

            if (Current() is ConstToken)
            {
                isConstant = true;
                Advance();
            }

            RuntimeType type = ParseType();
            Advance();

            Expect<IdentifierToken>();
            string identifier = Current().Lexeme;
            Advance();

            Expression? defaultValue = null;
            if (Current() is AssignmentToken)
            {
                Advance();
                defaultValue = ParseOrExpression();
                isDefaultParameterRequired = true;
            }
            else if (isDefaultParameterRequired)
            {
                throw new InvalidParameterListException(
                    "A non-default parameter cannot appear after a default parameter", startLine, startColumn
                );
            }

            Advance();

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
                    startLine,
                    startColumn
                )
            );
        }

        Expect<CloseParenthesisToken>();

        return parameterList;
    }

    private RuntimeType ParseType()
    {
        Expect<PrimitiveTypeToken>();
        PrimitiveTypeToken type = (Current() as PrimitiveTypeToken)!;

        var isNullable = false;
        if (Peek() is QuestionMarkToken)
        {
            isNullable = true;
            Advance();
        }

        return (type) switch
        {
            BooleanPrimitiveToken => new BooleanType(isNullable),
            DecimalPrimitiveToken => new DecimalType(isNullable),
            IntegerPrimitiveToken => new IntegerType(isNullable),
            StringPrimitiveToken => new StringType(isNullable),
            _ => throw new UnexpectedTokenException(type.GetType().Name, type.Lexeme, type.Line, type.Column)
        };
    }

    private Statement ParseAssignmentStatement()
    {
        Expression left = ParseOrExpression();

        if (Peek() is not AssignmentToken)
        {
            return left;
        }

        Advance(2);
        Expression value = ParseOrExpression();
        
        return new AssignmentStatement(left, value, left.StartLine, left.StartColumn);
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

        while (Peek() is OpenParenthesisToken)
        {
            Advance();
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
            argumentList.Add(ParseOrExpression());
            Advance();

            if (Current() is CommaToken)
            {
                Advance();

                if (Current() is CloseParenthesisToken)
                {
                    throw new UnexpectedTokenException(
                        Current().GetType().Name, Current().Lexeme, Current().Line, Current().Column
                    );
                }
            }
        }

        Expect<CloseParenthesisToken>();

        return new CallExpression(caller, argumentList, caller.StartLine, caller.StartColumn);
    }

    private Expression ParseMemberExpression()
    {
        var obj = ParsePrimaryExpression();

        return obj;
    }

    private Expression ParsePrimaryExpression()
    {
        return Current() switch
        {
            IdentifierToken      => new IdentifierExpression(
                Current().Lexeme, Current().Line, Current().Column
            ),
            IntegerLiteralToken  => new IntegerLiteralExpression(
                Int128.Parse(Current().Lexeme), Current().Line, Current().Column
            ),
            DecimalLiteralToken  => new DecimalLiteralExpression(
                decimal.Parse(Current().Lexeme, CultureInfo.InvariantCulture), Current().Line, Current().Column
            ),
            BooleanLiteralToken  => new BooleanLiteralExpression(
                bool.Parse(Current().Lexeme), Current().Line, Current().Column
            ),
            StringLiteralToken   => new StringLiteralExpression(
                Current().Lexeme, Current().Line, Current().Column
            ),
            NullLiteralToken     => new NullLiteralExpression(
                Current().Line, Current().Column
            ),
            OpenParenthesisToken => ParseParenthesis(),
            _ => throw new UnexpectedTokenException(
                Current().GetType().Name, Current().Lexeme, Current().Line, Current().Column
            ),
        };
    }

    private Expression ParseParenthesis()
    {
        Expect<OpenParenthesisToken>();
        Advance();

        Expression expr = ParseOrExpression();
        Advance();

        Expect<CloseParenthesisToken>();
        return expr;
    }

    private Expression ParseBinaryExpression<TOperator>(Func<Expression> next) where TOperator : OperatorToken
    {
        Expression? left = next();
        Advance();

        while (Current() is TOperator)
        {
            OperatorToken op = (Current() as OperatorToken)!;
            Advance();

            Expression right = next();
            Advance();

            left = new BinaryExpression(left!, op, right, left!.StartLine, left.StartColumn);
        }

        // coloca o indice de volta no lugar
        Recede();

        return left;
    }
}
