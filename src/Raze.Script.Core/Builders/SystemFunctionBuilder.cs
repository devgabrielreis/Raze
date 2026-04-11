using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Builders;

public sealed class SystemFunctionBuilder
{
    internal string FunctionName
    {
        get
        {
            if (_functionName == null)
            {
                ThrowHelper.ThrowInvalidOperationException(
                    $"Function name must be set by calling \"{nameof(HasName)}()\""
                );
            }
            return _functionName;
        }
    }

    private string? _functionName;
    private RuntimeType? _returnType;
    private List<ParameterSymbol> _parameters;
    private Func<RazeFunctionParameters, RazeFunctionReturnValue>? _body;

    internal SystemFunctionBuilder()
    {
        _functionName = null;
        _returnType = null;
        _body = null;
        _parameters = [];
    }

    public SystemFunctionBuilder HasName(string functionName)
    {
        if (_functionName != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Function name is already set to \"{_functionName}\". It cannot be changed to \"{functionName}\""
            );
        }

        _functionName = functionName;

        return this;
    }

    public SystemFunctionBuilder HasReturnType(TypeBuilder typeBuilder)
    {
        if (_returnType != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"The return type is already set to \"{_returnType}\". It cannot be changed"
            );
        }

        _returnType = typeBuilder.Build();

        return this;
    }

    public SystemFunctionBuilder HasParameter(Action<FunctionParameterBuilder> parameterBuilderFunction)
    {
        var parameterBuilder = new FunctionParameterBuilder();

        parameterBuilderFunction(parameterBuilder);

        var newParameter = parameterBuilder.Build();

        if (_parameters.Any(p => p.Identifier == newParameter.Identifier))
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"There already is a parameter called ${newParameter.Identifier} in this function"
            );
        }

        if (
            _parameters.Count > 0
            && _parameters.Last().DefaultValue != null
            && newParameter.DefaultValue == null
        )
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"After declaring a parameter with a default value, every next parameter must also have a default value"
            );
        }

        _parameters.Add(newParameter);

        return this;
    }

    public SystemFunctionBuilder HasBody(Func<RazeFunctionParameters, RazeFunctionReturnValue> body)
    {
        if (_body != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                "The function body is already set. It cannot be changed"
            );
        }

        _body = body;

        return this;
    }

    internal VariableSymbol Build()
    {
        if (_functionName == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Function name must be set by calling \"{nameof(HasName)}()\""
            );
        }

        if (_returnType == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Funtion return type must be set by calling \"{nameof(HasReturnType)}()\""
            );
        }

        if (_body == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Funtion body must be set by calling \"{nameof(HasBody)}()\""
            );
        }

        var functionValue = new SystemFunctionValue(_returnType, _parameters, _body);
        var realValue = new RuntimeValue(functionValue);

        var source = new SourceInfo($"{_functionName} builder");

        return new VariableSymbol(realValue, realValue.Type, true, in source);
    }
}
