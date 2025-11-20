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
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-29  AS00447  v0.00.04.013  Initial Version
/// 2015-05-21  AS00459  v0.00.04.023  Change parameters for TryGetCharFromKeyboardState
/// 2015-07-28  AS00497  v0.00.04.042  Use enumeration from VirtualKeyCode instead of consts
/// 2015-07-31  AS00499  v0.00.04.043  Delete commented out code, write comments
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-16  AS00565  v1.00.01.006  Removed static modifier from the class and methods
/// 2015-11-19  AS00569  v1.00.01.009  Modified summary, remarks and commenting on the method.
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-09-24  AS00729  v1.00.03.028  Moved P/Invoke methods to SafeNativeMethods
/// 2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-04-22  AS01182  v1.01.04.009  Resolve CA1822 by changing prefix for methods as static if needs to be
/// 2019-04-23  AS01183  v1.01.04.010  Add InvariantCulture to ToUpper method call
/// 2019-04-24  AS01184  v1.01.04.011  Resolve CA1806 by assigning the result to a variable
/// 2019-10-24  AS01242  v1.01.04.018  Resolve IDE0049 by simplifying Char to char
/// </revisionhistory>
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
        bool flag2 = ((SafeNativeMethods.GetKeyState(20) != 0) ? true : false);
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