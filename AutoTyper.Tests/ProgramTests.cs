using System.CommandLine;

namespace AutoTyper.Tests;

public class ProgramTests
{
    [Fact]
    public async Task Invoke_WithHelpOption_DisplaysHelp()
    {
        using StringWriter stdOut = new();
        int exitCode = await Invoke("--help", stdOut);
        
        Assert.Equal(0, exitCode);
        Assert.Contains("--help", stdOut.ToString());
    }

    private static Task<int> Invoke(string commandLine, StringWriter console)
    {
        CliConfiguration configuration = Program.GetConfiguration();
        configuration.Output = console;
        return configuration.InvokeAsync(commandLine);
    }
}