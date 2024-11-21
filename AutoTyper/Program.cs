using System.CommandLine;
using System.CommandLine.Invocation;
using System.Runtime.InteropServices;

using TextCopy;

using static PInvoke.User32;

namespace AutoTyper;

public sealed class Program
{
    private static Task<int> Main(string[] args)
    {
        return GetRootCommand().InvokeAsync(args);
    }

    public static RootCommand GetRootCommand()
    {
        Option<double> delay = new(["--delay", "-d"], () => 3.0)
        {
            Description = "The amount of time (in seconds) to wait before reading the clipboard and typing",
        };
        Option<string> content = new("--content", "-c")
        {
            Description = "The content to type rather than using the clipboard"
        };
        Option<bool> addNewLine = new("--append-new-line", "-n")
        {
            Description = "Appends a new line (Enter) key at the end of the content"
        };
        Option<bool> verbose = new("--verbose", "-v")
        {
            Description = "Prints additional information to the console"
        };
        Option<bool> fastTyping = new("--fast-typing", "-f")
        {
            Description = "Removes the delay between key strokes"
        };

        RootCommand rootCommand = new("A simple app to type int he contents of your clipboard")
        {
            delay,
            content,
            addNewLine,
            verbose,
            fastTyping
        };

        rootCommand.SetHandler(async (InvocationContext invocationContext) =>
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Error.WriteLine("This app only works on Windows");
                return;
            }

            double? delayValue = invocationContext.ParseResult.GetValueForOption(delay);
            string? contentValue = invocationContext.ParseResult.GetValueForOption(content);
            bool verboseValue = invocationContext.ParseResult.GetValueForOption(verbose);

            CancellationToken token = invocationContext.GetCancellationToken();

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
            if (invocationContext.ParseResult.GetValueForOption(fastTyping))
            {
                kc.NaturalTypingFlag = false;
            }
            kc.TypeString(text);

            if (invocationContext.ParseResult.GetValueForOption(addNewLine))
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
        
        return rootCommand;
    }
}