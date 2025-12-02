using System.Runtime.InteropServices;
using System.Text;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// Provides message-based text input methods that directly send text to windows.
/// This is faster and more reliable than keyboard simulation but requires a target window handle.
/// </summary>
public static class MessageBasedTyping
{
    private const int WM_SETTEXT = 0x000C;
    private const int WM_CHAR = 0x0102;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_KEYUP = 0x0101;

    /// <summary>
    /// Sets the entire text of a window/control at once using WM_SETTEXT.
    /// This is the fastest method but replaces all existing text.
    /// </summary>
    /// <param name="windowHandle">Handle to the target window</param>
    /// <param name="text">Text to set</param>
    /// <returns>True if successful</returns>
    public static bool SetWindowText(IntPtr windowHandle, string text)
    {
        return NativeMethods.SendMessage(windowHandle, WM_SETTEXT, IntPtr.Zero, text) != IntPtr.Zero;
    }

    /// <summary>
    /// Types text character-by-character using WM_CHAR messages.
    /// This appends to existing text and is very fast.
    /// </summary>
    /// <param name="windowHandle">Handle to the target window</param>
    /// <param name="text">Text to type</param>
    /// <param name="delayBetweenChars">Optional delay between characters in milliseconds</param>
    public static void TypeTextWithMessages(IntPtr windowHandle, string text, int delayBetweenChars = 0)
    {
        foreach (char c in text)
        {
            NativeMethods.SendMessage(windowHandle, WM_CHAR, (IntPtr)c, IntPtr.Zero);
            if (delayBetweenChars > 0)
            {
                Thread.Sleep(delayBetweenChars);
            }
        }
    }

    /// <summary>
    /// Types text character-by-character using WM_CHAR messages asynchronously.
    /// </summary>
    /// <param name="windowHandle">Handle to the target window</param>
    /// <param name="text">Text to type</param>
    /// <param name="delayBetweenChars">Delay between characters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public static async Task TypeTextWithMessagesAsync(IntPtr windowHandle, string text, TimeSpan delayBetweenChars, CancellationToken cancellationToken = default)
    {
        foreach (char c in text)
        {
            NativeMethods.SendMessage(windowHandle, WM_CHAR, (IntPtr)c, IntPtr.Zero);
            if (delayBetweenChars > TimeSpan.Zero)
            {
                await Task.Delay(delayBetweenChars, cancellationToken);
            }
        }
    }

    /// <summary>
    /// Posts text character-by-character using PostMessage (asynchronous, doesn't block).
    /// </summary>
    /// <param name="windowHandle">Handle to the target window</param>
    /// <param name="text">Text to type</param>
    public static void PostTextCharacters(IntPtr windowHandle, string text)
    {
        foreach (char c in text)
        {
            NativeMethods.PostMessage(windowHandle, WM_CHAR, (IntPtr)c, IntPtr.Zero);
        }
    }

    /// <summary>
    /// Gets the currently focused window handle.
    /// </summary>
    /// <returns>Handle to the foreground window</returns>
    public static IntPtr GetForegroundWindow()
    {
        return NativeMethods.GetForegroundWindow();
    }

    /// <summary>
    /// Finds a child window (like a text box) within a parent window.
    /// </summary>
    /// <param name="parentHandle">Parent window handle</param>
    /// <param name="className">Class name of the child window (e.g., "Edit" for text boxes)</param>
    /// <returns>Handle to the child window, or IntPtr.Zero if not found</returns>
    public static IntPtr FindChildWindow(IntPtr parentHandle, string className)
    {
        return NativeMethods.FindWindowEx(parentHandle, IntPtr.Zero, className, null);
    }

    private static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string? windowTitle);
    }
}
