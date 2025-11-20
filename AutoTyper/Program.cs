using System.CommandLine;
using System.Runtime.InteropServices;

using AutoTyper.DeviceEmulator;
using AutoTyper.DeviceEmulator.Native;
using TextCopy;

namespace AutoTyper;

public sealed partial class Program
{
    private static Task<int> Main(string[] args)
    {
        RootCommand rootCommand = GetRootCommand();
        return rootCommand.Parse(args).InvokeAsync();
    }

    public static RootCommand GetRootCommand()
    {
        Option<double> delay = new("--delay", "-d")
        {
            Description = "The amount of time (in seconds) to wait before reading the clipboard and typing",
            DefaultValueFactory = _ => 3.0
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
        rootCommand.SetAction(async (ParseResult parseResult, CancellationToken token) =>
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.Error.WriteLine("This app only works on Windows");
                return;
            }

            double? delayValue = parseResult.GetValue(delay);
            string? contentValue = parseResult.GetValue(content);
            bool verboseValue = parseResult.GetValue(verbose);

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

            IntPtr activeWindow = NativeMethods.GetForegroundWindow();
            string windowText = GetWindowText(activeWindow);

            KeyboardController kc = new(token);
            if (parseResult.GetValue(fastTyping))
            {
                kc.TypeStringNaturally(text, text.Length * 60);
            }
            else
            {
                kc.TypeString(text);
            }

            if (parseResult.GetValue(addNewLine))
            {
                kc.Type(VirtualKeyCode.RETURN);
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

    private static string GetWindowText(IntPtr hwnd)
    {
        int length = NativeMethods.GetWindowTextLength(hwnd);
        if (length == 0)
        {
            return string.Empty;
        }

        unsafe
        {
            char* buffer = stackalloc char[length + 1];
            int copied = NativeMethods.GetWindowText(hwnd, buffer, length + 1);
            return new string(buffer, 0, copied);
        }
    }

    private static partial class NativeMethods
    {
        [LibraryImport("user32.dll", SetLastError = true)]
        internal static partial IntPtr GetForegroundWindow();

        [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static unsafe partial int GetWindowText(IntPtr hWnd, char* lpString, int nMaxCount);

        [LibraryImport("user32.dll", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
        internal static partial int GetWindowTextLength(IntPtr hWnd);
    }
}