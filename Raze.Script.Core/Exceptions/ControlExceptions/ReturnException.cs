using Raze.Script.Core.Values;

namespace Raze.Script.Core.Exceptions.ControlExceptions;

internal class ReturnException : ControlException
{
    internal RuntimeValue ReturnedValue { get; private set; }

    internal ReturnException(RuntimeValue returnedValue)
        : base(nameof(ReturnException))
    {
        ReturnedValue = returnedValue;
    }
}
