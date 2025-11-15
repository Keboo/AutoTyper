using System.Runtime.InteropServices;
using AutoTyper.UI.Models;

namespace AutoTyper.UI.Services;

public class TypingService
{
    public async Task TypeSnippetAsync(Snippet snippet, CancellationToken cancellationToken = default)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            throw new PlatformNotSupportedException("This feature only works on Windows");
        }

        if (string.IsNullOrWhiteSpace(snippet.Content))
        {
            throw new ArgumentException("Snippet content cannot be empty", nameof(snippet));
        }

        // Wait for the configured delay
        if (snippet.Delay > 0)
        {
            await Task.Delay(TimeSpan.FromSeconds(snippet.Delay), cancellationToken);
        }

        // Get the active window before typing
        IntPtr activeWindow = NativeMethods.GetForegroundWindow();

        // Type the content using the keyboard controller
        Henooh.DeviceEmulator.KeyboardController kc = new(cancellationToken);
        if (snippet.FastTyping)
        {
            kc.NaturalTypingFlag = false;
        }
        kc.TypeString(snippet.Content);

        // Append new line if requested
        if (snippet.AppendNewLine)
        {
            kc.Type(Henooh.DeviceEmulator.Native.VirtualKeyCode.RETURN);
        }
    }

    public string GetActiveWindowTitle()
    {
        IntPtr activeWindow = NativeMethods.GetForegroundWindow();
        return GetWindowText(activeWindow);
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

    private static class NativeMethods
    {
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern unsafe int GetWindowText(IntPtr hWnd, char* lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int GetWindowTextLength(IntPtr hWnd);
    }
}
