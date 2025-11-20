using System;
using System.Globalization;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides a way to obtain KeyState and KeyboardState to KeyboardPressEventArgs.
/// </summary>
/// <remarks>
/// Internal class that will obtain KeyState and KeyboardState through use of TryGetCharFromKeyboardState method.
/// </remarks>
/// <visibility>internal</visibility>
internal class Keyboard
{
    /// <summary>
    /// Method that returns Char from Keyboard State.
    /// </summary>
    /// <param name="virtualKeyCode"></param>
    /// <param name="scanCode"></param>
    /// <param name="fuState"></param>
    /// <param name="ch"></param>
    /// <returns></returns>
    /// <visibility>internal</visibility>
    internal static bool TryGetCharFromKeyboardState(ushort virtualKeyCode, ushort scanCode, uint fuState, out char ch)
    {
        bool flag = (SafeNativeMethods.GetKeyState(16) & 0x80) == 128;
        bool flag2 = ((SafeNativeMethods.GetKeyState(20) != 0));
        byte[] array = new byte[256];
        SafeNativeMethods.GetKeyboardState(array);
        byte[] array2 = new byte[2];
        int fuState2 = Convert.ToInt32(fuState);
        if (SafeNativeMethods.ToAscii(virtualKeyCode, scanCode, array, array2, fuState2) != 1)
        {
            ch = '\0';
            return false;
        }
        ch = (char)array2[0];
        if ((flag2 ^ flag) && char.IsLetter(ch))
        {
            ch = char.ToUpper(ch, CultureInfo.InvariantCulture);
        }
        return true;
    }
}