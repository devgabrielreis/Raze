﻿using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Statements;
using Raze.Script.Core.Statements.Expressions;
using Raze.Script.Core.Statements.Expressions.LiteralExpressions;
using Raze.Script.Core.Tokens;
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
        if (tokens.Count == 0 || tokens.Last() is not EOF)
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
            _program.Body.Add(ParseCurrent());
            Advance();
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

    private void Advance(int howMuch = 1)
    {
        _currentIndex += howMuch;
    }

    private void Return()
    {
        _currentIndex--;
    }

    private bool HasEnded()
    {
        return _currentIndex >= _tokens.Count || Current() is EOF;
    }

    private Statement ParseCurrent()
    {
        if (Current() is VariableDeclarationToken)
        {
            return ParseVariableDeclaration();
        }

        return ParseAssignmentStatement();
    }

    private VariableDeclarationStatement ParseVariableDeclaration()
    {
        bool isConstant = Current() is Const;
        int startLine = Current().Line;
        int startColumn = Current().Column;
        Advance();

        Expect<PrimitiveTypeToken>();
        PrimitiveTypeToken type = (Current() as PrimitiveTypeToken)!;
        Advance();

        Expect<Identifier>();
        string identifier = Current().Lexeme;
        Advance();

        if (Current() is not AssignmentOperator)
        {
            Return();
            return new VariableDeclarationStatement(identifier, type, null, isConstant, startLine, startColumn);
        }

        Advance();
        Expression value = ParseOrExpression();

        return new VariableDeclarationStatement(identifier, type, value, isConstant, startLine, startColumn);
    }

    private Statement ParseAssignmentStatement()
    {
        Expression left = ParseOrExpression();

        if (Peek() is not AssignmentOperator)
        {
            return left;
        }

        Advance(2);
        Expression value = ParseOrExpression();
        
        return new AssignmentStatement(left, value, left.StartLine, left.StartColumn);
    }

    private Expression ParseOrExpression()
    {
        return ParseBinaryExpression<OrOperator>(ParseAndExpression);
    }

    private Expression ParseAndExpression()
    {
        return ParseBinaryExpression<AndOperator>(ParseEqualityExpression);
    }

    private Expression ParseEqualityExpression()
    {
        return ParseBinaryExpression<EqualityOperator>(ParseRelationalExpression);
    }

    private Expression ParseRelationalExpression()
    {
        return ParseBinaryExpression<RelationalOperator>(ParseAdditiveExpression);
    }

    private Expression ParseAdditiveExpression()
    {
        return ParseBinaryExpression<AdditiveOperator>(ParseMultiplicativeExpression);
    }

    private Expression ParseMultiplicativeExpression()
    {
        return ParseBinaryExpression<MultiplicativeOperator>(ParsePrimaryExpression);
    }

    private Expression ParsePrimaryExpression()
    {
        switch (Current())
        {
            case Identifier:
                return new IdentifierExpression(Current().Lexeme, Current().Line, Current().Column);
            case IntegerLiteral:
                return new IntegerLiteralExpression(Int128.Parse(Current().Lexeme), Current().Line, Current().Column);
            case DecimalLiteral:
                return new DecimalLiteralExpression(decimal.Parse(Current().Lexeme, CultureInfo.InvariantCulture), Current().Line, Current().Column);
            case BooleanLiteral:
                return new BooleanLiteralExpression(bool.Parse(Current().Lexeme), Current().Line, Current().Column);
            case StringLiteral:
                return new StringLiteralExpression(Current().Lexeme, Current().Line, Current().Column);
            case NullLiteral:
                return new NullLiteralExpression(Current().Line, Current().Column);
            case OpenParenthesis:
                Advance();
                Expression expr = ParseOrExpression();
                Advance();
                Expect<CloseParenthesis>();
                return expr;
            default:
                throw new UnexpectedTokenException(
                    Current().GetType().Name, Current().Lexeme, Current().Line, Current().Column
                );
        }
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
        Return();

        return left;
    }
}
