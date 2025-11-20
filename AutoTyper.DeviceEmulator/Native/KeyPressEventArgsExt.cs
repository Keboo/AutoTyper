using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
///  KeyboardPressEventArgsExt Provides extended data for the <see cref="E:AutoTyper.DeviceEmulator.KeyboardObserver.KeyPress" /> event.
///  </summary>
///  <remarks>
///  KeyboardPressEventArgsExt extends features from System.Windows.Forms.KeyPressEventArgs.
///  </remarks>
///  <visibility>internal</visibility>
///  <revisionhistory>
///  YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
///  ==========  =======  ============  ============================================================================
///  2015-04-29  AS00447  v0.00.04.013  Initial Version
///  2015-05-07  AS00453  v0.00.04.017  Renamed the Class from KeyboardHookListener to KeyboardObserver
///  2015-05-21  AS00459  v0.00.04.023  Removed dependency on KeyboardHookStruct and use KEYBDINPUT instead
///  2015-05-28  AS00462  v0.00.04.024  Replace KeyboardHookStruct with KEYBDINPUT structure 
///  2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
///  2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the class
///  2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
///  2015-11-16  AS00565  v1.00.01.006  Removed static modifier from the class and methods
///  2015-11-18  AS00568  v1.00.01.008  Modified remarks
///  2016-01-16  AS00601  v1.00.03.000  Fixed a bug that threw null exception
///  2016-02-16  AS00624  v1.00.03.003  Replaced tabs with spaces
///  2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
///  2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
///  2016-10-11  AS00746  v1.00.03.033  Renamed the class KEYBDINPUT to KeybdInput
///  2016-10-16  AS00751  v1.00.03.038  Follow Henooh style guidelines on Property casing, added xml headers
///  2016-10-19  AS00754  v1.00.03.040  Added comments to Keyboard property and CreateNonChar method
///  2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags
///  2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
///  2017-03-14  AS00851  v1.00.06.013  Changed the timestamp to become int instead of uint type
///  2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
///  2019-03-11  AS01156  v1.01.03.007  Resolve IDE0017 and IDE0018 messages and simplify
///  2019-04-22  AS01182  v1.01.04.009  Resolve CA1305 by provding CultureInfo on Convert method
///  2019-04-23  AS01183  v1.01.04.010  Resolve CA1822 by adding static prefix to CreateNonChar method
///  2019-04-24  AS01184  v1.01.04.011  Resolve CA1811 by adding static prefix to FromRawDataGlobal and App methods
///  2019-10-30  AS01244  v1.01.04.019  Remove Keyboard property as the class has been converted to static methods
///  </revisionhistory>
internal class KeyPressEventArgsExt : KeyPressEventArgs
{
	/// <summary>
	/// True if represents a system or functional non char key.
	/// </summary>
	/// <visibility>public</visibility>
	public bool IsNonChar { get; private set; }

	/// <summary>
	/// The system tick count of when the event occured.
	/// </summary>
	/// <visibility>public</visibility>
	public int Timestamp { get; private set; }

	/// <summary>
	/// Initializes a new instance of the KeyPressEventArgsExt class.
	/// </summary>
	/// <param name="keyChar">
	/// Character corresponding to the key pressed. 
	/// 0 char if represens a system or functional non char key.
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
