using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;

namespace Raze.Script.Core.Runtime;

internal class ExecutionContext
{
    private enum Context
    {
        Loop,
        Function
    }

    private Stack<Context> _contextStack;

    internal ExecutionContext()
    {
        _contextStack = new Stack<Context>();
    }

    internal void Reset()
    {
        _contextStack.Clear();
    }

    internal void EnterLoop() => _contextStack.Push(Context.Loop);

    internal bool IsInLoop() => _contextStack.Count > 0 && _contextStack.Peek() == Context.Loop;

    internal void ExitLoop(SourceInfo source) => Exit(Context.Loop, source);

    internal void EnterFunction() => _contextStack.Push(Context.Function);

    internal bool IsInFunction() => _contextStack.Contains(Context.Function);

    internal void ExitFunction(SourceInfo source) => Exit(Context.Function, source);

    private void Exit(Context context, SourceInfo source)
    {
        if (_contextStack.Count == 0 || _contextStack.Peek() != context)
        {
            throw new CorruptedContextStackException(source);
        }

        _contextStack.Pop();
    }
}
