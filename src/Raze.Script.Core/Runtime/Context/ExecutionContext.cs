using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Runtime.Context;

internal sealed class ExecutionContext
{
    private Stack<Context> _contextStack;
    private ContextSignal _contextSignal;
    private ReturnedValue? _returnValue;

    internal ExecutionContext()
    {
        _contextStack = new Stack<Context>();
        _contextSignal = ContextSignal.None;
        _returnValue = null;
    }

    internal void Reset()
    {
        _contextStack.Clear();
        _contextSignal = ContextSignal.None;
        _returnValue = null;
    }

    internal bool HasAnyPendingSignal()
    {
        return _contextSignal != ContextSignal.None;
    }

    internal bool HasPending(ContextSignal signal)
    {
        Debug.Assert(signal != ContextSignal.None);
        return _contextSignal == signal;
    }

    internal void SignalBreak()
    {
        _contextSignal = ContextSignal.Break;
    }

    internal void SignalContinue()
    {
        _contextSignal = ContextSignal.Continue;
    }

    internal void SignalReturn(ref readonly ReturnedValue? returnValue)
    {
        _contextSignal = ContextSignal.Return;
        _returnValue = returnValue;
    }

    internal void ConsumeBreak()
    {
        Debug.Assert(_contextSignal == ContextSignal.Break);
        _contextSignal = ContextSignal.None;
    }

    internal void ConsumeContinue()
    {
        Debug.Assert(_contextSignal == ContextSignal.Continue);
        _contextSignal = ContextSignal.None;
    }

    internal void ConsumeReturn(out ReturnedValue? returnedValue)
    {
        Debug.Assert(_contextSignal == ContextSignal.Return);
        returnedValue = _returnValue;

        _returnValue = null;
        _contextSignal = ContextSignal.None;
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
        throw new CorruptedContextStackException("Current context does not match expected execution state", source);
    }
}
