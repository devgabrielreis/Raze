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

    [Fact]
    public void Evaluate_ImportingFiles_ReturnsExpectedValue()
    {
        var importScript = """
            namespace test {
                def integer getNumber() {
                    return 20;
                }
            }
        """;

        var importFile = CreateScriptFile(importScript);
        var importedFilePath = importFile.FullName.Replace('\\', '/');

        var mainScript = $$"""
            import "{{importedFilePath}}";

            namespace main {
                def integer main() {
                    return test::getNumber();
                }
            }
        """;

        var mainFile = CreateScriptFile(mainScript);

        RazeAssert.EvaluatesToInteger(mainFile, 20);
    }

    [Fact]
    public void Evaluate_ImportingFilesMultipleTimes_HasNoEffect()
    {
        var importScript = """
            namespace test {
                def integer getNumber() {
                    return 20;
                }
            }
        """;

        var importFile = CreateScriptFile(importScript);
        var importedFilePath = importFile.FullName.Replace('\\', '/');

        var mainScript = $$"""
            import "{{importedFilePath}}";
            import "{{importedFilePath}}";
            import "{{importedFilePath}}";

            namespace main {
                def integer main() {
                    return test::getNumber();
                }
            }
        """;

        var mainFile = CreateScriptFile(mainScript);

        RazeAssert.EvaluatesToInteger(mainFile, 20);
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
