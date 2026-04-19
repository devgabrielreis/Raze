using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class FileAccessException
    : RuntimeException, IThrowableByThrowHelper<FileAccessException>
{
    internal FileAccessException(string message, SourceInfo source)
        : base(message, source, nameof(FileAccessException))
    {
    }

    static FileAccessException IThrowableByThrowHelper<FileAccessException>.Create(
        string message,
        ref readonly SourceInfo source
    )
    {
        return new FileAccessException(message, source);
    }
}
