using System.CommandLine;
using System.Runtime.InteropServices;

using TextCopy;

using static PInvoke.User32;

namespace AutoTyper;

public sealed class Program
{
    private static Task<int> Main(string[] args)
    {
        CliConfiguration configuration = GetConfiguration();
        return configuration.InvokeAsync(args);
    }

    public static CliConfiguration GetConfiguration()
    {
        CliOption<double> delay = new("--delay", "-d")
        {
            Description = "The amount of time (in seconds) to wait before reading the clipboard and typing",
            DefaultValueFactory = _ => 3.0
        };
        CliOption<string> content = new("--content", "-c")
        {
            Description = "The content to type rather than using the clipboard"
        };
        CliOption<bool> addNewLine = new("--append-new-line", "-n")
        {
            Description = "Appends a new line (Enter) key at the end of the content"
        };
        CliOption<bool> verbose = new("--verbose", "-v")
        {
            Description = "Prints additional information to the console"
        };
        CliOption<bool> fastTyping = new("--fast-typing", "-f")
        {
            Description = "Removes the delay between key strokes"
        };

        CliRootCommand rootCommand = new("A simple app to type int he contents of your clipboard")
        {
            delay,
            content,
            addNewLine,
            verbose,
            fastTyping
        };
        rootCommand.SetAction(async (ParseResult parseResult, CancellationToken token) =>
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Error.WriteLine("This app only works on Windows");
                return;
            }

            double? delayValue = parseResult.CommandResult.GetValue(delay);
            string? contentValue = parseResult.CommandResult.GetValue(content);
            bool verboseValue = parseResult.CommandResult.GetValue(verbose);

            string? text = contentValue ?? await ClipboardService.GetTextAsync(token);
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Error.WriteLine("Did not find any text to type");
                return;
            }

            if (delayValue is not null)
            {
                Console.WriteLine($"Waiting {delayValue.Value} seconds before typing...");
                await Task.Delay(TimeSpan.FromSeconds(delayValue.Value), token);
            }

            IntPtr activeWindow = GetForegroundWindow();
            string windowText = GetWindowText(activeWindow);

            //TODO: Replace this with direct calls and remove the library
            Henooh.DeviceEmulator.KeyboardController kc = new(token);
            if (parseResult.CommandResult.GetValue(fastTyping))
            {
                kc.NaturalTypingFlag = false;
            }
            kc.TypeString(text);

            if (parseResult.CommandResult.GetValue(addNewLine))
            {
                kc.Type(Henooh.DeviceEmulator.Native.VirtualKeyCode.RETURN);
            }

            if (verboseValue)
            {
                Console.WriteLine($"Sent '{text}' to {windowText}");
            }
            else
            {
                Console.WriteLine($"Sent text to {windowText}");
            }
        });
        
        return new CliConfiguration(rootCommand);
    }
}