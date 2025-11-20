using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
///  KeyboardPressEventArgsExt Provides extended data for the <see cref="E:AutoTyper.DeviceEmulator.KeyboardObserver.KeyPress" /> event.
///  </summary>
///  <remarks>
///  KeyboardPressEventArgsExt extends features from System.Windows.Forms.KeyPressEventArgs.
///  </remarks>
///  <visibility>internal</visibility>
internal class KeyPressEventArgsExt : KeyPressEventArgs
{
    /// <summary>
    /// True if represents a system or functional non char key.
    /// </summary>
    /// <visibility>public</visibility>
    public bool IsNonChar { get; private set; }

    /// <summary>
    /// The system tick count of when the event occurred.
    /// </summary>
    /// <visibility>public</visibility>
    public int Timestamp { get; private set; }

    /// <summary>
    /// Initializes a new instance of the KeyPressEventArgsExt class.
    /// </summary>
    /// <param name="keyChar">
    /// Character corresponding to the key pressed. 
    /// 0 char if represents a system or functional non char key.
    /// </param>
    /// <visibility>internal</visibility>
    internal KeyPressEventArgsExt(char keyChar)
        : base(keyChar)
    {
        IsNonChar = false;
        Timestamp = Convert.ToInt32(Environment.TickCount);
    }

    /// <summary>
    /// Method called when a key is released.
    /// </summary>
    /// <returns></returns>
    /// <visibility>private</visibility>
    private static KeyPressEventArgsExt CreateNonChar()
    {
        return new KeyPressEventArgsExt('\0')
        {
            IsNonChar = true,
            Timestamp = Convert.ToInt32(Environment.TickCount)
        };
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.KeyPressEventArgsExt" /> from Windows Message parameters.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <param name="isGlobal">Specifies if the hook is local or global.</param>
    /// <returns>A new KeyPressEventArgsExt object.</returns>
    /// <visibility>internal</visibility>
    internal static KeyPressEventArgsExt FromRawData(IntPtr wParam, IntPtr lParam, bool isGlobal)
    {
        return isGlobal ? FromRawDataGlobal(wParam, lParam) : FromRawDataApp(wParam, lParam);
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.KeyPressEventArgsExt" /> from Windows Message parameters,
    /// based upon a local application hook.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <returns>A new KeyPressEventArgsExt object.</returns>
    /// <visibility>private</visibility>
    private static KeyPressEventArgsExt FromRawDataApp(IntPtr wParam, IntPtr lParam)
    {
        uint num = 0u;
        num = (uint)(int)lParam;
        bool flag = (num & 0x40000000) != 0;
        bool flag2 = (num & 0x80000000u) != 0;
        if (!flag && !flag2)
        {
            return CreateNonChar();
        }
        ushort virtualKeyCode = Convert.ToUInt16(wParam, CultureInfo.InvariantCulture);
        ushort scanCode = checked((ushort)(num & 0xFF0000));
        if (!Keyboard.TryGetCharFromKeyboardState(virtualKeyCode, scanCode, 0u, out var ch))
        {
            return CreateNonChar();
        }
        return new KeyPressEventArgsExt(ch);
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.KeyPressEventArgsExt" /> from Windows Message parameters,
    /// based upon a system-wide hook.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <returns>A new KeyPressEventArgsExt object.</returns>
    /// <visibility>internal</visibility>
    internal static KeyPressEventArgsExt FromRawDataGlobal(IntPtr wParam, IntPtr lParam)
    {
        if ((int)wParam != 256)
        {
            return CreateNonChar();
        }
        KeybdInput keybdInput = (KeybdInput)Marshal.PtrToStructure(lParam, typeof(KeybdInput));
        ushort keyCode = keybdInput.KeyCode;
        ushort scan = keybdInput.Scan;
        uint flags = keybdInput.Flags;
        if (!Keyboard.TryGetCharFromKeyboardState(keyCode, scan, flags, out var ch))
        {
            return CreateNonChar();
        }
        return new KeyPressEventArgsExt(ch)
        {
            Timestamp = (int)keybdInput.Time
        };
    }
}