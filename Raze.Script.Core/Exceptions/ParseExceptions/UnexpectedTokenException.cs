using Raze.Script.Core.Tokens;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class UnexpectedTokenException : ParseException
{
    public UnexpectedTokenException(string tokenType, string lexeme, int line, int column)
        : base(
              $"Unexpected token of type {tokenType} found: {lexeme}",
              line,
              column,
              nameof(UnexpectedTokenException)
          )
    {
    }
}
