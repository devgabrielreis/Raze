using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.ParseExceptions;

public sealed class InvalidAssignmentException : ParseException
{
    internal InvalidAssignmentException(string message, SourceInfo source)
        : base(message, source, nameof(InvalidAssignmentException))
    {
    }
}
