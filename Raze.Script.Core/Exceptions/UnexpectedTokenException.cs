using Raze.Script.Core.Lexer;

namespace Raze.Script.Core.Exceptions
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
