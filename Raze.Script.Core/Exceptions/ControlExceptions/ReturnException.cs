using Raze.Script.Core.Metadata;
using Raze.Script.Core.Values;

namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal class ReturnException : ControlException
{
    internal RuntimeValue ReturnedValue { get; private set; }

    internal ReturnException(RuntimeValue returnedValue, SourceInfo source)
        : base(source, nameof(ReturnException))
    {
        ReturnedValue = returnedValue;
    }
}
