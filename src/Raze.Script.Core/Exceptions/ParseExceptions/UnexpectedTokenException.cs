using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public class UnexpectedTokenException : ParseException
{
    internal UnexpectedTokenException(string foundToken, string lexeme, SourceInfo source)
        : base(
              $"Unexpected token of type {foundToken} found: {lexeme}",
              source,
              nameof(UnexpectedTokenException)
          )
    {
    }

    internal UnexpectedTokenException(string foundToken, string expectedToken, string lexeme, SourceInfo source)
        : base(
              $"Unexpected token found. Expected: {expectedToken}. Found: {foundToken}" + (lexeme.Equals(string.Empty) ? "" : $". Value: {lexeme}"),
              source,
              nameof(UnexpectedTokenException)
          )
    {
    }
}
