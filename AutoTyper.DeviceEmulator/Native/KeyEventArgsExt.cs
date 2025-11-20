using System;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
///  KeyEventArgsExt Provides extended argument data for the <see cref="E:AutoTyper.DeviceEmulator.KeyboardObserver.KeyDown" /> or <see cref="E:AutoTyper.DeviceEmulator.KeyboardObserver.KeyUp" /> event.
///  </summary>
///  <remarks>
///  KeyboardEventArgsExt extends features from System.Windows.Forms.KeyEventArgs.
///  </remarks>
///  <revisionhistory>
///  YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
///  ==========  =======  ============  ============================================================================
///  2015-04-29  AS00447  v0.00.04.013  Initial Version
///  2015-05-07  AS00453  v0.00.04.017  Renamed the Class from KeyboardHookListener to KeyboardObserver
///  2015-05-28  AS00462  v0.00.04.024  Replace KeyboardHookStruct with KEYBDINPUT structure 
///  2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
///  2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the class
///  2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
///  2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
///  2016-07-13  AS00693  v1.00.03.018  Modified remarks
///  2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
///  2016-10-20  AS00755  v1.00.03.041  Added xml comment header on the internal consturctor
///  2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
///  2017-02-18  AS00836  v1.00.06.009  Change the type of timestamp to int from uint
///  2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
///  </revisionhistory>
internal class KeyEventArgsExt : KeyEventArgs
{
    /// <summary>
    /// The system tick count of when the event occured.
    /// </summary> 
    public int Timestamp { get; private set; }

    /// <summary>
    /// True if event singnals key down..
    /// </summary>
    public bool IsKeyDown { get; private set; }

    /// <summary>
    /// True if event singnals key up.
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