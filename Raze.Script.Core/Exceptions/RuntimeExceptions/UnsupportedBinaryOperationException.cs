﻿namespace Raze.Script.Core.Exceptions.RuntimeExceptions;

public class UnsupportedBinaryOperationException : RuntimeException
{
    public UnsupportedBinaryOperationException(string leftType, string rightType, string op, int line, int column)
        : base(
            $"{leftType} {op} {rightType}",
            line,
            column,
            nameof(UnsupportedBinaryOperationException)
        )
    {
    }
}
