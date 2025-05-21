using Raze.Script.Core;
using Raze.Script.Core.Exceptions.LexerExceptions;
using Raze.Script.Core.Exceptions.ParseExceptions;

namespace Raze.Tests;

public class RazeScriptSyntaxTests
{
    [Theory]
    [InlineData("@")]
    [InlineData(".0")]
    [InlineData("0.")]
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
}
