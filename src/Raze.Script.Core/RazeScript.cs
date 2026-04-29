using Raze.Script.Core.Builders;
using Raze.Script.Core.Constants;
using Raze.Script.Core.Engine;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Exceptions.RuntimeExceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Result;
using Raze.Script.Core.Runtime.Scopes;
using Raze.Script.Core.Runtime.Types;
using Raze.Shared.Utils;
using System.Text;

namespace Raze.Script.Core;

public static class RazeScript
{
    public static Version Version => typeof(RazeScript).Assembly.GetName().Version!;

    public static RazeResult ExecuteScript(
        FileInfo script,
        Dictionary<string, Action<ModuleBuilder>>? customModuleBuilders = null,
        string? entryPointNamespace = "main",
        string entryPointFunction = "main"
    )
    {
        if (!FileUtils.TryReadAllFileLines(script, out var source, out var errorMessage))
        {
            var error = new FileAccessException(errorMessage, new SourceInfo(script.FullName));
            return new RazeError(error);
        }

        var session = new RazeSession(
            Scope.CreateGlobalScope(),
            customModuleBuilders
        );
        var rootPath = script.Directory!.FullName;

        var result = Evaluate(source, script.FullName, rootPath, session);
        if (result is RazeError)
        {
            return result;
        }

        if (!ScopeHasValidEntryPoint(session.RootScope, out errorMessage, entryPointNamespace, entryPointFunction))
        {
            var error = new InvalidEntryPointException(errorMessage, new SourceInfo(script.FullName));
            return new RazeError(error);
        }

        var entryPoint = BuildEntryPointCode(entryPointNamespace, entryPointFunction);

        return Evaluate(entryPoint, "Script entry point", rootPath, session);
    }

    public static RazeResult Evaluate(
        string source,
        string sourceLocation,
        string rootPath,
        RazeSession? session = null
    )
    {
        return Evaluate(
            source,
            sourceLocation,
            rootPath,
            session,
            throwRazeError: false,
            customRootScope: null
        );
    }

    internal static RazeResult Evaluate(
        string source,
        string sourceLocation,
        string rootPath,
        RazeSession? session,
        bool throwRazeError,
        Scope? customRootScope
    )
    {
        session ??= new RazeSession();

        try
        {
            var tokens = Lexer.Tokenize(source, sourceLocation);
            var program = Parser.Parse(tokens, sourceLocation);
            var result = Interpreter.Evaluate(program, session, rootPath, customRootScope);

            return new RazeSuccess(result);
        }
        catch (RazeException razeEx)
        {
            if (throwRazeError)
            {
                throw;
            }

            return new RazeError(razeEx);
        }
    }

    private static bool ScopeHasValidEntryPoint(
        Scope scope,
        out string errorMessage,
        string? entryPointNamespace,
        string entryPointFunction
    )
    {
        var entryPointScope = scope;

        if (entryPointNamespace != null)
        {
            var mainNamespace = entryPointScope.TryGetNamespace(entryPointNamespace);
            if (mainNamespace == null)
            {
                errorMessage = $"The namespace \"{entryPointNamespace}\" was not found";
                return false;
            }

            entryPointScope = mainNamespace.Scope;
        }

        var mainFunction = entryPointScope.TryGetVariable(entryPointFunction);
        if (mainFunction == null)
        {
            errorMessage = $"The function \"{entryPointFunction}\" was not found";
            return false;
        }

        var expectedEntryPointType = TypeFactory.GetType(BaseType.UserFunction, false, RuntimeType.Integer);
        if (mainFunction.Type != expectedEntryPointType)
        {
            errorMessage = $"The function \"{entryPointFunction}\" has a unexpected type. Expected: {expectedEntryPointType}. Found: {mainFunction.Type}";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private static string BuildEntryPointCode(
        string? entryPointNamespace,
        string entryPointFunction
    )
    {
        var codeBuilder = new StringBuilder();

        if (entryPointNamespace != null)
        {
            codeBuilder.Append(entryPointNamespace);
            codeBuilder.Append(Operators.NAMESPACE_ACCESSOR);
        }

        codeBuilder.Append(entryPointFunction);
        codeBuilder.Append(Operators.OPEN_PARENTHESIS);
        codeBuilder.Append(Operators.CLOSE_PARENTHESIS);

        return codeBuilder.ToString();
    }
}
