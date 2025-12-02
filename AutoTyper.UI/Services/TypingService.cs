using System.Runtime.InteropServices;

using AutoTyper.DeviceEmulator;
using AutoTyper.DeviceEmulator.Native;
using AutoTyper.UI.Models;

using static System.Net.Mime.MediaTypeNames;

namespace AutoTyper.UI.Services;

public class TypingService
{
    private readonly ImageDisplayService _imageDisplayService;

    public TypingService(ImageDisplayService imageDisplayService)
    {
        ArgumentNullException.ThrowIfNull(imageDisplayService);
        _imageDisplayService = imageDisplayService;
    }

    public async Task ExecuteSnippetAsync(Snippet snippet, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(snippet);

        if (snippet.SnippetType == SnippetType.Image)
        {
            await _imageDisplayService.DisplayImageAsync(snippet, cancellationToken);
        }
        else
        {
            await TypeSnippetAsync(snippet, cancellationToken);
        }
    }

    public async Task TypeSnippetAsync(Snippet snippet, CancellationToken cancellationToken)
    {
        // Get the content to type - either from clipboard or snippet content
        string contentToType = snippet.Content;
        if (snippet.UseClipboard)
        {
            try
            {
                contentToType = System.Windows.Clipboard.GetText();
                if (string.IsNullOrWhiteSpace(contentToType))
                {
                    throw new InvalidOperationException("Clipboard is empty or does not contain text");
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to read clipboard: {ex.Message}", ex);
            }
        }
        else if (string.IsNullOrWhiteSpace(contentToType))
        {
            throw new ArgumentException("Snippet content cannot be empty", nameof(snippet));
        }

        // Type the content using the keyboard controller
        KeyboardController kc = new();
        if (snippet.FastTyping)
        {
            await kc.TypeStringAsync(contentToType, cancellationToken);
        }
        else
        {
            await kc.TypeStringNaturallyAsync(contentToType, cancellationToken);
        }

        // Append new line if requested
        if (snippet.AppendNewLine)
        {
            await kc.TypeAsync(VirtualKeyCode.RETURN, cancellationToken);
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
