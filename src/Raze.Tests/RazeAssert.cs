using Raze.Script.Core;
using Raze.Script.Core.Builders;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Result;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Types;

namespace Raze.Tests;

internal class RazeAssert
{
    internal static void EvaluatesToInteger(
        string script,
        Int128 expected,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Integer, success.ValueType);
        Assert.Equal(expected, success.AsInteger());
    }

    internal static void EvaluatesToDecimal(
        string script,
        decimal expected,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Decimal, success.ValueType);
        Assert.Equal(expected, success.AsDecimal());
    }

    internal static void EvaluatesToBoolean(
        string script,
        bool expected,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Boolean, success.ValueType);
        Assert.Equal(expected, success.AsBoolean());
    }

    internal static void EvaluatesToString(
        string script,
        string expected,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.String, success.ValueType);
        Assert.Equal(expected, success.AsString());
    }

    internal static void EvaluatesToNull(
        string script,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Null, success.ValueType);
    }

    internal static void EvaluatesToVoid(
        string script,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    )
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var success = Assert.IsType<RazeSuccess>(result);
        Assert.Equal(BaseType.Void, success.ValueType);
    }

    internal static void ReturnsError<T>(
        string script,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    ) where T: RazeException
    {
        var result = RazeScript.Evaluate(script, context, scope, customModules);

        var error = Assert.IsType<RazeError>(result);
        Assert.IsType<T>(error.Error);
    }

    internal static void ThrowsError<T>(
        string script,
        Scope? scope = null,
        string context = "Raze.Tests",
        Dictionary<string, Action<ModuleBuilder>>? customModules = null
    ) where T : Exception
    {
        Assert.Throws<T>(() =>
        {
            RazeScript.Evaluate(script, context, scope, customModules);
        });
    }
}
