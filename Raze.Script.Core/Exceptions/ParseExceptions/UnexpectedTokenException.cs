namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class UnexpectedTokenException : ParseException
{
    public UnexpectedTokenException(string foundToken, string lexeme, int line, int column)
        : base(
              $"Unexpected token of type {foundToken} found: {lexeme}",
              line,
              column,
              nameof(UnexpectedTokenException)
          )
    {
    }

    public UnexpectedTokenException(string foundToken, string expectedToken, string lexeme, int line, int column)
        : base(
              $"Unexpected token found. Expected: {expectedToken}. Found: {foundToken}" + (lexeme.Equals(string.Empty) ? "" : $". Value: {lexeme}"),
              line,
              column,
              nameof(UnexpectedTokenException)
          )
    {
    }
}
