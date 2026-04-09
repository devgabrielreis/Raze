using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Exceptions;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Builders;

internal sealed class FunctionParameterBuilder
{
    private string? _parameterName;
    private RuntimeType? _type;
    private RuntimeValue? _defaultValue;

    internal FunctionParameterBuilder()
    {
        _parameterName = null;
        _type = null;
        _defaultValue = null;
    }

    internal FunctionParameterBuilder HasName(string parameterName)
    {
        if (_parameterName != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter name is already set to \"{_parameterName}\". It cannot be changed"
            );
        }

        _parameterName = parameterName;

        return this;
    }

    internal FunctionParameterBuilder HasType(TypeBuilder typeBuilder)
    {
        if (_type != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter type is already set to \"{_type}\". It cannot be changed"
            );
        }

        _type = typeBuilder.Build();

        return this;
    }

    internal FunctionParameterBuilder HasDefaultValue<T>(T value)
    {
        if (_type == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter type must be set by calling \"{nameof(HasType)}()\""
            );
        }

        if (_defaultValue != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter default value is already set to \"{_defaultValue}\". It cannot be changed"
            );
        }

        RuntimeValueUtils.ValueToRuntimeValue(value, out var runtimeValue);

        if (!_type!.IsCompatibleWith(in runtimeValue))
        {
            ThrowHelper.ThrowInvalidOperationException("The value is not compatible with the parameter type");
        }

        _defaultValue = runtimeValue;

        return this;
    }

    internal FunctionParameterBuilder HasNullDefaultValue()
    {
        if (_type == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter type must be set by calling \"{nameof(HasType)}()\""
            );
        }

        if (_defaultValue != null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter default value is already set to \"{_defaultValue}\". It cannot be changed"
            );
        }

        if (!_type!.IsCompatibleWith(in RuntimeValue.Null))
        {
            ThrowHelper.ThrowInvalidOperationException("The value is not compatible with the parameter type");
        }

        _defaultValue = RuntimeValue.Null;

        return this;
    }

    internal ParameterSymbol Build()
    {
        if (_parameterName == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter name must be set by calling \"{nameof(HasName)}()\""
            );
        }

        if (_type == null)
        {
            ThrowHelper.ThrowInvalidOperationException(
                $"Parameter type must be set by calling \"{nameof(HasType)}()\""
            );
        }

        var source = new SourceInfo($"{_parameterName} builder");

        return new ParameterSymbol(false, _type, _parameterName, in source, _defaultValue);
    }
}
