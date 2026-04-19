using Raze.Script.Core.Exceptions.RuntimeExceptions;

namespace Raze.Tests.Core;

public class ScriptFileTests : IDisposable
{
    private readonly string _tempDirectory;

    public ScriptFileTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);
    }

    [Fact]
    public void Evaluate_ScriptFile_ReturnsExpectedValue()
    {
        var script = """
            namespace main {
                def integer main() {
                    return 99;
                }
            }
        """;

        var scriptFile = CreateScriptFile(script);
        RazeAssert.EvaluatesToInteger(scriptFile, 99);
    }

    [Fact]
    public void Evaluate_ScriptFileWithoutNamespaceMain_ThrowsInvalidEntryPointException()
    {
        var script = """
            def integer main() {
                return 0;
            }
        """;

        var scriptFile = CreateScriptFile(script);
        RazeAssert.ReturnsError<InvalidEntryPointException>(scriptFile);
    }

    [Fact]
    public void Evaluate_ScriptFileWithoutFunctionMain_ThrowsInvalidEntryPointException()
    {
        var script = """
            namespace main {
                def integer main2() {
                    return 0;
                }
            }
        """;

        var scriptFile = CreateScriptFile(script);
        RazeAssert.ReturnsError<InvalidEntryPointException>(scriptFile);
    }

    [Fact]
    public void Evaluate_ScriptFileWithInvalidMainSignature_ThrowsInvalidEntryPointException()
    {
        var script = """
            namespace main {
                def string main() {
                    return "0";
                }
            }
        """;

        var scriptFile = CreateScriptFile(script);
        RazeAssert.ReturnsError<InvalidEntryPointException>(scriptFile);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }

    private FileInfo CreateScriptFile(string content, string fileExtension = ".raze")
    {
        var fileName = Path.Combine(_tempDirectory, $"{Guid.NewGuid()}{fileExtension}");
        File.WriteAllText(fileName, content);

        return new FileInfo(fileName);
    }
}
