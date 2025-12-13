using Raze.Script.Core;
using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;
using Raze.Script.Core.Values;

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
        Assert.Throws<UnexpectedCharacterException>(() =>
        {
            RazeScript.Evaluate(expression, "Raze.Tests");
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
            RazeScript.Evaluate(expression, "Raze.Tests");
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
            RazeScript.Evaluate(expreesion, "Raze.Tests");
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
            RazeScript.Evaluate(script, "Raze.Tests");
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

        var result = RazeScript.Evaluate(script, "Raze.Tests");

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

        var result = RazeScript.Evaluate(script, "Raze.Tests");

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
            RazeScript.Evaluate(script, "Raze.Tests");
        });
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

        var result = RazeScript.Evaluate(script, "Raze.Tests");

        Assert.IsType<IntegerValue>(result);
        Assert.Equal(11, (result as IntegerValue)!.IntValue);
    }
}
