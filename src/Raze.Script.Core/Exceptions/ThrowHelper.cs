using Raze.Script.Core.Metadata;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Raze.Script.Core.Exceptions;

internal static class ThrowHelper
{
    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static void Throw<TError>(string message, ref readonly SourceInfo source)
        where TError : RazeException, IThrowableByThrowHelper<TError>
    {
        throw TError.Create(message, in source);
    }

    [DoesNotReturn]
    [StackTraceHidden]
    [MethodImpl(MethodImplOptions.NoInlining)]
    internal static TReturn Throw<TError, TReturn>(string message, ref readonly SourceInfo source)
        where TError : RazeException, IThrowableByThrowHelper<TError>
    {
        throw TError.Create(message, in source);
    }
}
