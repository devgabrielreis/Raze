﻿using Raze.Script.Core;
using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Values;

namespace Raze.Tests;

public class RazeScriptSyntaxTests
{
    [Theory]
    [InlineData("@")]
    [InlineData(".0")]
    [InlineData("0.")]
    [InlineData("0.0.")]
    public void Evaluate_Expression_ThrowsUnexpectedCharacterException(string expression)
    {
        Assert.Throws<UnexpectedCharacterException>(() =>
        {
            RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("(10 + 4")]
    [InlineData("const integer nome = const")]
    [InlineData("var integer nome = +")]
    [InlineData("var nome = 10")]
    public void Evaluate_Expression_ThrowsUnexpectedTokenException(string expression)
    {
        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(expression);
        });
    }

    [Theory]
    [InlineData("\"\"\"")]
    [InlineData("\"teste")]
    [InlineData("\"")]
    [InlineData("teste\"")]
    public void Evaluate_InvalidString_ThrowsInvalidStringException(string expreesion)
    {
        Assert.Throws<InvalidStringException>(() =>
        {
            RazeScript.Evaluate(expreesion);
        });
    }

    [Theory]
    [InlineData("\"hello\\zword\"")]
    [InlineData("\"\\'\"")]
    public void Evaluate_InvalidEscapeSequence_ThrowsUnrecognizedEscapeSequenceException(string expression)
    {
        Assert.Throws<UnrecognizedEscapeSequenceException>(() =>
        {
            RazeScript.Evaluate(expression);
        });
    }

    [Fact]
    public void Evaluate_Statements_MustBeSeparatedBySemiColon()
    {
        var script = @"
            var integer test = 10
            test = test + 1
        ";

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }

    [Fact]
    public void Evaluate_LastStatement_DoesntNeedSemiColon()
    {
        var script = @"
            var integer test = 10;
            test = test + 1;
            test
        ";

        var result = RazeScript.Evaluate(script);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(11, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_EmptyStatement_HasNoEffect()
    {
        var script = @"
            var integer test = 10;
            ;;;;;;;;
            test = test + 1;
            ;;;;;;;;
            test
        ";

        var result = RazeScript.Evaluate(script);

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(11, (result as IntegerValue)!.IntValue);
    }

    [Fact]
    public void Evaluate_StatementInsideCodeBlockWithoutSemiColon_ThrowsUnexpectedTokenException()
    {
        var script = @"
            {
                var integer test = 10
            }
        ";

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            RazeScript.Evaluate(script);
        });
    }
}
