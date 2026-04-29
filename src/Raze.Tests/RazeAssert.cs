using Raze.Script.Core;
using Raze.Script.Core.Builders;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Result;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Tests;

internal class RazeAssert
{
    internal static void EvaluatesToInteger(
        string script,
        Int128 expected,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    )
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Integer, success.ValueType);
        Assert.Equal(expected, success.AsInteger());
    }

    internal static void EvaluatesToInteger(
        FileInfo fileScript,
        Int128 expected,
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.ExecuteScript(fileScript, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Integer, success.ValueType);
        Assert.Equal(expected, success.AsInteger());
    }

    internal static void EvaluatesToDecimal(
        string script,
        decimal expected,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    )
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Decimal, success.ValueType);
        Assert.Equal(expected, success.AsDecimal());
    }

    internal static void EvaluatesToBoolean(
        string script,
        bool expected,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    )
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Boolean, success.ValueType);
        Assert.Equal(expected, success.AsBoolean());
    }

    internal static void EvaluatesToString(
        string script,
        string expected,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    )
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.String, success.ValueType);
        Assert.Equal(expected, success.AsString());
    }

    internal static void EvaluatesToNull(
        string script,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    )
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Null, success.ValueType);
    }

    internal static void EvaluatesToVoid(
        string script,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    )
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Void, success.ValueType);
    }

    internal static void ReturnsError<T>(
        string script,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    ) where T: RazeException
    {
        rootPath ??= Directory.GetCurrentDirectory();

        var result = RazeScript.Evaluate(script, context, rootPath, session);

        var error = Assert.IsType<RazeError>(result);
        Assert.IsType<T>(error.Error);
    }

    internal static void ReturnsError<T>(
        FileInfo fileScript,
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    ) where T : RazeException
    {
        var result = RazeScript.ExecuteScript(fileScript, customModules);

        var error = Assert.IsType<RazeError>(result);
        Assert.IsType<T>(error.Error);
    }

    internal static void ThrowsError<T>(
        string script,
        RazeSession? session = null,
        string context = "Raze.Tests",
        string? rootPath = null
    ) where T : Exception
    {
        rootPath ??= Directory.GetCurrentDirectory();

        Assert.Throws<T>(() =>
        {
            var result = RazeScript.Evaluate(script, context, rootPath, session);
        });
    }
}
