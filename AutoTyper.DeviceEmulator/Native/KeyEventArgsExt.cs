using System;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
///  KeyEventArgsExt Provides extended argument data for the <see cref="E:AutoTyper.DeviceEmulator.KeyboardObserver.KeyDown" /> or <see cref="E:AutoTyper.DeviceEmulator.KeyboardObserver.KeyUp" /> event.
///  </summary>
///  <remarks>
///  KeyboardEventArgsExt extends features from System.Windows.Forms.KeyEventArgs.
///  </remarks>
internal class KeyEventArgsExt : KeyEventArgs
{
    /// <summary>
    /// The system tick count of when the event occurred.
    /// </summary> 
    public int Timestamp { get; private set; }

    /// <summary>
    /// True if event signals key down.
    /// </summary>
    public bool IsKeyDown { get; private set; }

    /// <summary>
    /// True if event signals key up.
    /// </summary>
    public bool IsKeyUp { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.Native.KeyEventArgsExt" /> class.
    /// </summary>
    /// <param name="keyData"></param>
    public KeyEventArgsExt(Keys keyData)
        : base(keyData)
    {
    }

    /// <summary>
    /// Constructor that accepts values to be initialized.
    /// </summary>
    internal KeyEventArgsExt(Keys keyData, int timestamp, bool isKeyDown, bool isKeyUp)
        : this(keyData)
    {
        Timestamp = timestamp;
        IsKeyDown = isKeyDown;
        IsKeyUp = isKeyUp;
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.KeyEventArgsExt" /> from Windows Message parameters.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <param name="isGlobal">Specifies if the hook is local or global.</param>
    /// <returns>A new KeyEventArgsExt object.</returns>
    internal static KeyEventArgsExt FromRawData(IntPtr wParam, IntPtr lParam, bool isGlobal)
    {
        return isGlobal ? FromRawDataGlobal(wParam, lParam) : FromRawDataApp(wParam, lParam);
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.KeyEventArgsExt" /> from Windows Message parameters, based upon
    /// a local application hook.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <returns>A new KeyEventArgsExt object.</returns>
    private static KeyEventArgsExt FromRawDataApp(IntPtr wParam, IntPtr lParam)
    {
        int timestamp = Convert.ToInt32(Environment.TickCount);
        uint num = 0u;
        num = (uint)(int)lParam;
        bool flag = (num & 0x40000000) != 0;
        bool flag2 = (num & 0x80000000u) != 0;
        Keys keys = (Keys)(int)wParam;
        bool isKeyDown = !flag && !flag2;
        bool isKeyUp = flag && flag2;
        return new KeyEventArgsExt(keys, timestamp, isKeyDown, isKeyUp);
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.KeyEventArgsExt" /> from Windows Message parameters, based upon
    /// a system-wide hook.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <returns>A new KeyEventArgsExt object.</returns>
    private static KeyEventArgsExt FromRawDataGlobal(IntPtr wParam, IntPtr lParam)
    {
        KeybdInput keybdInput = (KeybdInput)Marshal.PtrToStructure(lParam, typeof(KeybdInput));
        Keys keyCode = (Keys)keybdInput.KeyCode;
        bool isKeyDown = (int)wParam == 256 || (int)wParam == 260;
        bool isKeyUp = (int)wParam == 257 || (int)wParam == 261;
        return new KeyEventArgsExt(keyCode, (int)keybdInput.Time, isKeyDown, isKeyUp);
    }
}