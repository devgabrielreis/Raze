using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core;

public class ModuleTests
{
    [Fact]
    public void Evaluate_ImportingSameBuiltInModuleMultipleTimes_HasNoEffect()
    {
        var script = """
            import math;
            import math;
            import math;
            import math;

            math::PI
        """;

        RazeAssert.EvaluatesToDecimal(script, 3.14159m);
    }

    [Fact]
    public void Evaluate_ImportingNonExistingModule_ThrowsUndefinedIdentifierException()
    {
        var script = """
            import idontexist
        """;

        RazeAssert.ReturnsError<UndefinedIdentifierException>(script);
    }

    [Theory]
    [InlineData("math")]
    public void Evaluate_CreatingNamespaceWithBuiltInModuleName_ThrowsRedeclarationException(string builtInName)
    {
        string script = $$"""
            namespace {{builtInName}} {
            }
        """;

        RazeAssert.ReturnsError<RedeclarationException>(script);
    }

    [Fact]
    public void Evaluate_ImportingModuleInLocalScope_ThrowsScopeDeclarationException()
    {
        var script = """
            {
                import math;
            }
        """;

        RazeAssert.ReturnsError<ScopeDeclarationException>(script);
    }
}
