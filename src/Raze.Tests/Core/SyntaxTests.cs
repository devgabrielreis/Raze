using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;

namespace Raze.Tests.Core;

public class SyntaxTests
{
    [Theory]
    [InlineData("@")]
    [InlineData(".")]
    [InlineData("0.")]
    [InlineData("0.0.")]
    public void Evaluate_Expression_ThrowsUnexpectedCharacterException(string expression)
    {
        RazeAssert.ReturnsError<UnexpectedCharacterException>(expression);
    }

    [Theory]
    [InlineData("(10 + 4")]
    [InlineData("const integer nome = const")]
    [InlineData("var integer nome = +")]
    [InlineData("var nome = 10")]
    public void Evaluate_Expression_ThrowsUnexpectedTokenException(string expression)
    {
        RazeAssert.ReturnsError<UnexpectedTokenException>(expression);
    }

    [Theory]
    [InlineData("\"\"\"")]
    [InlineData("\"teste")]
    [InlineData("\"")]
    [InlineData("teste\"")]
    public void Evaluate_InvalidString_ThrowsInvalidStringException(string expression)
    {
        RazeAssert.ReturnsError<InvalidStringException>(expression);
    }

    [Fact]
    public void Evaluate_Statements_MustBeSeparatedBySemiColon()
    {
        var script = @"
            var integer test = 10
            test = test + 1
        ";

        RazeAssert.ReturnsError<UnexpectedTokenException>(script);
    }

    [Fact]
    public void Evaluate_LastStatement_DoesntNeedSemiColon()
    {
        var script = @"
            var integer test = 10;
            test = test + 1;
            test
        ";

        RazeAssert.EvaluatesToInteger(script, 11);
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

        RazeAssert.EvaluatesToInteger(script, 11);
    }

    [Fact]
    public void Evaluate_StatementInsideCodeBlockWithoutSemiColon_ThrowsUnexpectedTokenException()
    {
        var script = @"
            {
                var integer test = 10
            }
        ";

        RazeAssert.ReturnsError<UnexpectedTokenException>(script);
    }

    [Theory]
    [InlineData("_myvar")]
    [InlineData("my_var")]
    [InlineData("myvar_")]
    [InlineData("_")]
    [InlineData("___")]
    public void Evaluate_Identifiers_CanHaveUnderscores(string identifier)
    {
        var script = $@"
            var integer {identifier} = 10;
            {identifier} = {identifier} + 1;
            {identifier}
        ";

        RazeAssert.EvaluatesToInteger(script, 11);
    }

    [Theory]
    [InlineData("10++")]
    [InlineData("10.0--")]
    [InlineData("--1")]
    [InlineData("++1.0")]
    public void Evaluate_UnaryMutationExpressionNotOnVariable_ThrowsInvalidOperandException(string expression)
    {
        RazeAssert.ReturnsError<InvalidOperandException>(expression);
    }

    [Theory]
    [InlineData("null??")]
    [InlineData("10.0??")]
    [InlineData("false??")]
    public void Evaluate_NullCheckerNotOnVariable_ThrowsInvalidOperandException(string expression)
    {
        RazeAssert.ReturnsError<InvalidOperandException>(expression);
    }
}
