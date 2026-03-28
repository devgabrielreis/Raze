using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime;

internal sealed class ExecutionContext
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

    internal void EnterLoop()
    {
        _contextStack.Push(Context.Loop);
    }

    internal bool IsInLoop()
    {
        return _contextStack.Count > 0 && _contextStack.Peek() == Context.Loop;
    }

    internal void ExitLoop(ref readonly SourceInfo source)
    {
        Exit(Context.Loop, in source);
    }

    internal void EnterFunction()
    {
        _contextStack.Push(Context.Function);
    }

    internal bool IsInFunction()
    {
        return _contextStack.Contains(Context.Function);
    }

    internal void ExitFunction(ref readonly SourceInfo source)
    {
        Exit(Context.Function, in source);
    }

    private void Exit(Context context, ref readonly SourceInfo source)
    {
        if (_contextStack.Count == 0 || _contextStack.Peek() != context)
        {
            ThrowCorruptedContextStackException(in source);
        }

        _contextStack.Pop();
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ThrowCorruptedContextStackException(ref readonly SourceInfo source)
    {
        throw new CorruptedContextStackException(source);
    }
}
