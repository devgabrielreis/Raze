using Raze.Script.Core;
using Raze.Script.Core.Builders;
using Raze.Script.Core.Builders.Types;

namespace Raze.Tests.Core.Builders;

public class TypeBuilderTests
{
    [Fact]
    public void Evaluate_DeclaringNullableVoidType_ThrowsInvalidOperationException()
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
                            .HasType(TypeBuilder.Void.AsNullable())
                            .HasNullValue();
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
