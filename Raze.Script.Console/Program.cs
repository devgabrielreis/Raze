using Raze.Script.Core;
using Raze.Script.Core.Exceptions;

try
{
    var tokens = new RazeScript().Tokenize("nome 10 foo bar 12");

    foreach (var token in tokens)
    {
        Console.WriteLine(token);
    }
}
catch (Exception ex)
{
    if (ex is LexerException)
    {
        Console.WriteLine(ex);
    }
    else
    {
        throw ex;
    }
}
