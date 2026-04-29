using Raze.Script.Core;
using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;

namespace Raze.Tests.Core.Builders;

public class ConstantBuilderTests
{
    [Fact]
    public void Evaluate_DeclaringConstantWithoutName_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasType(TypeBuilder.Integer)
                            .HasValue((Int128)10);
                    });
            }
        };

        string script = $"""
            import testModule
        """;
        var session = new RazeSession(customModules);

        RazeAssert.ThrowsError<InvalidOperationException>(script, session);
    }

    [Fact]
    public void Evaluate_EmptyConstantName_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("")
                            .HasType(TypeBuilder.Integer)
                            .HasValue((Int128)10);
                    });
            }
        };

        string script = $"""
            import testModule
        """;
        var session = new RazeSession(customModules);

        RazeAssert.ThrowsError<InvalidOperationException>(script, session);
    }

    [Fact]
    public void Evaluate_DeclaringConstantValueBeforeType_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasValue((Int128)10)
                            .HasType(TypeBuilder.Integer);
                    });
            }
        };

        string script = $"""
            import testModule
        """;
        var session = new RazeSession(customModules);

        RazeAssert.ThrowsError<InvalidOperationException>(script, session);
    }

    [Fact]
    public void Evaluate_DeclaringConstantValueTwice_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.Integer)
                            .HasValue((Int128)10)
                            .HasValue((Int128)20);
                    });
            }
        };

        string script = $"""
            import testModule
        """;
        var session = new RazeSession(customModules);

        RazeAssert.ThrowsError<InvalidOperationException>(script, session);
    }

    [Fact]
    public void Evaluate_DeclaringConstantWithIncompatibleValue_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.String)
                            .HasValue((Int128)10);
                    });
            }
        };

        string script = $"""
            import testModule
        """;
        var session = new RazeSession(customModules);

        RazeAssert.ThrowsError<InvalidOperationException>(script, session);
    }

    [Fact]
    public void Evaluate_DeclaringConstantWithoutValue_ThrowsInvalidOperationException()
    {
        var customModules = new Dictionary<string, Action<ModuleBuilder>>()
        {
            ["testModule"] = moduleBuilder =>
            {
                moduleBuilder
                    .HasConstant(constantBuilder =>
                    {
                        constantBuilder
                            .HasName("testConstant")
                            .HasType(TypeBuilder.Integer);
                    });
            }
        };

        string script = $"""
            import testModule
        """;
        var session = new RazeSession(customModules);

        RazeAssert.ThrowsError<InvalidOperationException>(script, session);
    }
}
