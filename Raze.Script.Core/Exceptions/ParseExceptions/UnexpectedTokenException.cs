using Raze.Script.Core.Tokens;

namespace Raze.Script.Core.Exceptions.ParseExceptions
{
    public class UnexpectedTokenException : ParseException
    {
        public UnexpectedTokenException(Token token)
            : base(
                  $"Unexpected token of type {token.TokenType.ToString()} found: {token.Lexeme}",
                  token.Line,
                  token.Column,
                  nameof(UnexpectedTokenException)
              )
        {
        }
    }
}
