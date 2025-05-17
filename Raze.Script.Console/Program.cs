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
    if (ex is RazeException)
    {
        Console.WriteLine(ex.Message);
    }
    else
    {
        throw ex;
    }
}
