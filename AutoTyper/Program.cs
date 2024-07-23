﻿using System.CommandLine;

using PInvoke;

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
            Description = "The amount of time (in seconds) to wait before reading the clipboard and typing"
        };
        CliOption<string> content = new("--content", "-c")
        {
            Description = "The content to type rather than using the clipboard"
        };
        CliOption<bool> addNewLine = new("--append-new-line", "-n")
        {
            Description = "Appends a new line (Enter) key at the end of the content"
        };

        CliRootCommand rootCommand = new("A simple app to type int he contents of your clipboard")
        {
            delay,
            content,
            addNewLine
        };
        rootCommand.SetAction(async (ParseResult parseResult, CancellationToken token) =>
        {
            double? delayValue = parseResult.CommandResult.GetValue(delay);
            string? contentValue = parseResult.CommandResult.GetValue(content);
            
            if (delayValue is not null)
            {
                Console.WriteLine($"Waiting {delayValue.Value} seconds before typing...");
                await Task.Delay(TimeSpan.FromSeconds(delayValue.Value), token);
            }

            string? text = contentValue ?? await ClipboardService.GetTextAsync(token);
            if (string.IsNullOrWhiteSpace(text))
            {
                Console.Error.WriteLine("Did not find any text to type");
                return;
            }

            IntPtr activeWindow = GetForegroundWindow();
            string windowText = GetWindowText(activeWindow);

            Henooh.DeviceEmulator.KeyboardController kc = new(token);
            //TODO: Add support for KeyboardController.NaturalTypingFlag
            kc.TypeString(text);

            if (parseResult.CommandResult.GetValue(addNewLine))
            {
                kc.Type(Henooh.DeviceEmulator.Native.VirtualKeyCode.RETURN);
            }

            Console.WriteLine($"Sent '{text}' to {windowText}");
        });
        
        return new CliConfiguration(rootCommand);
    }
}