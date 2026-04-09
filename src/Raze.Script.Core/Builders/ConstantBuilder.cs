using Raze.Script.Core.Builders.Types;
using Raze.Script.Core.Metadata;
using Raze.Script.Core.Runtime.Symbols;
using Raze.Script.Core.Runtime.Types;
using Raze.Script.Core.Runtime.Values;

namespace Raze.Script.Core.Builders;

internal sealed class ConstantBuilder
{
    internal string ConstantName {
        get
        {
            if (_constantName == null)
            {
                throw new InvalidOperationException(
                    $"Constant name must be set by calling \"{nameof(HasName)}()\""
                );
            }
            return _constantName;
        }
    }

    private string? _constantName;
    private RuntimeType? _type;
    private RuntimeValue? _value;

    internal ConstantBuilder()
    {
        _constantName = null;
        _type = null;
        _value = null;
    }

    internal ConstantBuilder HasType(TypeBuilder typeBuilder)
    {
        if (_type != null)
        {
            throw new InvalidOperationException(
                $"Constant type is already set to \"{_type}\". It cannot be changed"
            );
        }

        _type = typeBuilder.Build();

        return this;
    }

    internal ConstantBuilder HasName(string constantName)
    {
        if (_constantName != null)
        {
            throw new InvalidOperationException(
                $"Constant name is already set to \"{_constantName}\". It cannot be changed to \"{constantName}\""
            );
        }

        _constantName = constantName;

        return this;
    }

    internal ConstantBuilder HasValue<T>(T value)
    {
        if (_type == null)
        {
            throw new InvalidOperationException(
                $"Constant type must be set by calling \"{nameof(HasType)}()\""
            );
        }

        if (_value != null)
        {
            throw new InvalidOperationException($"Constant value is already set to \"{_value}\". It cannot be changed");
        }

        RuntimeValueUtils.ValueToRuntimeValue(value, out var runtimeValue);

        if (!_type!.IsCompatibleWith(in runtimeValue))
        {
            throw new InvalidOperationException("The value is not compatible with the constant type");
        }

        _value = runtimeValue;

        return this;
    }

    internal ConstantBuilder HasNullValue()
    {
        if (_type == null)
        {
            throw new InvalidOperationException(
                $"Constant type must be set by calling \"{nameof(HasType)}()\""
            );
        }

        if (_value != null)
        {
            throw new InvalidOperationException($"Constant value is already set to \"{_value}\". It cannot be changed");
        }

        if (!_type!.IsCompatibleWith(in RuntimeValue.Null))
        {
            throw new InvalidOperationException("The value is not compatible with the constant type");
        }

        _value = RuntimeValue.Null;

        return this;
    }

    internal VariableSymbol Build()
    {
        if (_type == null)
        {
            throw new InvalidOperationException(
                $"Constant type must be set by calling \"{nameof(HasType)}()\""
            );
        }

        if (_constantName == null)
        {
            throw new InvalidOperationException(
                $"Constant name must be set by calling \"{nameof(HasName)}()\""
            );
        }

        if (_value == null)
        {
            throw new InvalidOperationException(
                $"Constant value must be set by calling either \"{nameof(HasValue)}()\" or \"{nameof(HasNullValue)}()\""
            );
        }

        var source = new SourceInfo($"{_constantName} builder");

        return new VariableSymbol(_value, _type, true, in source);
    }
}
