using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using AutoTyper.DeviceEmulator.Native;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// KeyboardController provides methods to type text and keys. 
/// </summary>
/// <remarks>
/// KeyboardController provides methods that help simulate typing method,  including method that works like 
/// a human types. 
/// </remarks>
/// <visibility>public</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2013-10-15  AS00122  v0.00.00.122  Initial Version
/// 2013-10-21  AS00128  v0.00.00.128  Separate methods to Private methods and Public methods
/// 2013-10-22  AS00129  v0.00.00.129  Use List instead of array of INPUT to send at a time
/// 2013-10-26  AS00133  v0.00.00.133  Disable Left Shift on every SendInput
/// 2013-12-16  AS00172  v0.00.00.172  Added function to Disable Shift and Shift back
/// 2014-03-18  AS00203  v0.00.00.203  Moved to Henooh.Utility.Native Namespace
/// 2014-04-14  AS00230  v0.00.00.230  Moved to HenoohUtility as a Class Library Project (dll)
/// 2015-02-18  AS00362  v0.00.02.
/// 
362  Added comments regarding the state of CapsLock and NumLock status for  Type and TypeString
/// 2015-02-26  AS00372  v0.00.03.001  Modified the summary and remarks, added comments to fields
/// 2015-02-27  AS00373  v0.00.03.002  Removed Virtual Keyboard field and method, Removed NumLock and CapsLock status, Use SendInput method instead
/// 2015-04-02  AS00420  v0.00.04.000  Moved to HenoohInputSimulator Project
/// 2015-05-28  AS00462  v0.00.04.024  Changed Type method parameter from STRING_ARRAY to KEYBDINPUT_ARRAY
/// 2015-07-17  AS00490  v0.00.04.036  Removed string buffer and char buffer and simplify Type methods
/// 2015-07-23  AS00495  v0.00.04.041  Use VirtualKeyCode enumeration for dictionary type 
/// 2015-07-31  AS00499  v0.00.04.043  Removed comment and console output
/// 2015-08-19  AS00510  v0.00.04.051  Correctly calculate Sleep Interval based on word, word-1, char or char-1
/// 2015-08-20  AS00511  v0.00.04.052  Return early if string is white space or null
/// 2015-08-26  AS00514  v0.00.04.054  Change ReleaseKey and PressKey to remove List implementation
/// 2015-09-24  AS00532  v0.00.04.063  TypeString without interval now sends one key at a time instead of batch
/// 2015-09-30  AS00536  v0.00.04.064  Provide default value for interval parameter in TypeString method
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-03  AS00553  v1.00.00.004  Renamed all occurrences of Key to VirtualKeyCode and modified PressKey and ReleaseKey and other methods that use VirtualKeyCode
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to AutoTyper.DeviceEmulator
/// 2015-11-16  AS00565  v1.00.01.006  Removed static modifier from the class and methods
/// 2015-11-19  AS00569  v1.00.01.009  Added revision history above code
/// 2016-01-18  AS00603  v1.00.03.000  Added XML comment headers
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-08-04  AS00705  v1.00.03.020  Fixed the logic on interval delay when natural typing is called
/// 2016-08-29  AS00716  v1.00.03.022  Moved inputBuffer and inputList to BaseController
/// 2016-10-08  AS00743  v1.00.03.032  Removed use of inputList, use individual SendInput for each event
/// 2016-10-15  AS00750  v1.00.03.037  Removed use of Field inputBuffer and inputList
/// 2016-10-16  AS00751  v1.00.03.038  Added XML visibility tags for properties and methods
/// 2016-10-19  AS00754  v1.00.03.040  Removed const declaration for VirtualKeyCodeDictionary
/// 2016-10-30  AS00765  v1.00.04.003  Implement CancellationToken on KeyboardController
/// 2016-11-01  AS00767  v1.00.04.004  Call BaseController constructor and Sleep method from BaseController
/// 2016-11-22  AS00783  v1.00.04.011  Added XML comments to all methods
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2016-12-27  AS00803  v1.00.05.010  Refactor Type and TypeString method to expose more public methods
/// 2017-02-25  AS00842  v1.00.06.011  Follow Henooh Style Guidelines to not exceed 120 characters
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from AutoTyper.DeviceEmulator to AutoTyper.DeviceEmulator
/// 2018-12-17  AS01131  v1.01.03.003  Add visibility tags to XML comments
/// 2019-01-30  AS01143  v1.01.03.005  Modify the layout of the code, add XML header comments to all properties
/// 2019-04-11  AS01175  v1.01.04.005  Resolve CA1822 by adding static prefix to SendKeyEvent
/// </revisionhistory>
public class KeyboardController : BaseController
{
	/// <summary>
	/// Returns a dictionary that contains the char key as the key and VirtualKeyCode as value.
	/// </summary>
	/// <visibility>private</visibility>
	private static Dictionary<char, VirtualKeyCode> VirtualKeyCodeDictionary => new Dictionary<char, VirtualKeyCode>
	{
		{ '0', VirtualKeyCode.VK_0 },
		{ '1', VirtualKeyCode.VK_1 },
		{ '2', VirtualKeyCode.VK_2 },
		{ '3', VirtualKeyCode.VK_3 },
		{ '4', VirtualKeyCode.VK_4 },
		{ '5', VirtualKeyCode.VK_5 },
		{ '6', VirtualKeyCode.VK_6 },
		{ '7', VirtualKeyCode.VK_7 },
		{ '8', VirtualKeyCode.VK_8 },
		{ '9', VirtualKeyCode.VK_9 },
		{ 'a', VirtualKeyCode.VK_A },
		{ 'b', VirtualKeyCode.VK_B },
		{ 'c', VirtualKeyCode.VK_C },
		{ 'd', VirtualKeyCode.VK_D },
		{ 'e', VirtualKeyCode.VK_E },
		{ 'f', VirtualKeyCode.VK_F },
		{ 'g', VirtualKeyCode.VK_G },
		{ 'h', VirtualKeyCode.VK_H },
		{ 'i', VirtualKeyCode.VK_I },
		{ 'j', VirtualKeyCode.VK_J },
		{ 'k', VirtualKeyCode.VK_K },
		{ 'l', VirtualKeyCode.VK_L },
		{ 'm', VirtualKeyCode.VK_M },
		{ 'n', VirtualKeyCode.VK_N },
		{ 'o', VirtualKeyCode.VK_O },
		{ 'p', VirtualKeyCode.VK_P },
		{ 'q', VirtualKeyCode.VK_Q },
		{ 'r', VirtualKeyCode.VK_R },
		{ 's', VirtualKeyCode.VK_S },
		{ 't', VirtualKeyCode.VK_T },
		{ 'u', VirtualKeyCode.VK_U },
		{ 'v', VirtualKeyCode.VK_V },
		{ 'w', VirtualKeyCode.VK_W },
		{ 'x', VirtualKeyCode.VK_X },
		{ 'y', VirtualKeyCode.VK_Y },
		{ 'z', VirtualKeyCode.VK_Z },
		{ ' ', VirtualKeyCode.SPACE },
		{ '\n', VirtualKeyCode.RETURN },
		{ '\r', VirtualKeyCode.RETURN },
		{ '\t', VirtualKeyCode.TAB }
	};

	/// <summary>
	/// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.KeyboardController" /> with no arguments.
	/// </summary>
	/// <visibility>public</visibility>
	public KeyboardController()
	{
	}

	/// <summary>
	/// Default constructor with CancellationToken.
	/// </summary>
	/// <param name="aCancellationToken"></param>
	/// <visibility>public</visibility>
	public KeyboardController(CancellationToken aCancellationToken)
		: base(aCancellationToken)
	{
	}

	/// <summary>
	/// Method that sends input to press and release the VirtualKeyCode. 
	/// </summary>
	/// <param name="aVirtualKeyCode"></param>
	/// <visibility>public</visibility>
	public void Type(VirtualKeyCode aVirtualKeyCode)
	{
		if (RunMode == 4)
		{
			return;
		}
		PressKey(aVirtualKeyCode);
		ReleaseKey(aVirtualKeyCode);
	}

	/// <summary>
	/// Method that sends inputs to type a given string.
	/// </summary>
	/// <param name="aString"></param>
	/// <visibility>public</visibility>
	public void TypeString(string aString)
	{
		TypeString(aString, 0, naturalTyping: false, 0, 0);
	}

	/// <summary>
	/// Method that sends inputs to type a given string with given interval.
	/// </summary>
	/// <param name="aString"></param>
	/// <param name="aInterval"></param>
	/// <visibility>public</visibility>
	public void TypeString(string aString, int aInterval)
	{
		TypeString(aString, aInterval, naturalTyping: false, 0, 0);
	}

	/// <summary>
	/// Method that sends inputs to type a given string with natural typing interval.
	/// </summary>
	/// <param name="aString"></param>
	/// <param name="aWordPerMinute"></param>
	/// <visibility>public</visibility>
	public void TypeStringNaturally(string aString, int aWordPerMinute)
	{
		TypeStringNaturally(aString, aWordPerMinute, 0);
	}

	/// <summary>
	/// Method that sends inputs to type a given string with natural typing interval.
	/// Allows variance in typing speed.
	/// </summary>
	/// <param name="aString"></param>
	/// <param name="aWordPerMinute"></param>
	/// <param name="aPlusMinusVariance"></param>
	/// <visibility>public</visibility>
	public void TypeStringNaturally(string aString, int aWordPerMinute, int aPlusMinusVariance)
	{
		TypeString(aString, 0, naturalTyping: true, aWordPerMinute, aPlusMinusVariance);
	}

	/// <summary>
	/// Method that sends inputs to type a given string.
	/// </summary>
	/// <param name="aString"></param>
	/// <param name="aInterval"></param>
	/// <param name="naturalTyping"></param>
	/// <param name="aWordPerMinute"></param>
	/// <param name="aPlusMinusVariance"></param>
	/// <visibility>public</visibility>
	public void TypeString(string aString, int aInterval, bool naturalTyping, int aWordPerMinute, int aPlusMinusVariance)
	{
		if (string.IsNullOrWhiteSpace(aString))
		{
			return;
		}
		double num = 0.0;
		if (naturalTyping)
		{
			double num2 = 60000.0 / ((double)aWordPerMinute * 5.0);
			Random random = new Random();
			num = num2 \u002B (double)random.Next(-1 * aPlusMinusVariance, aPlusMinusVariance);
		}
		int num3 = 0;
		char[] array = aString.ToCharArray();
		foreach (char c in array)
		{
			num3\u002B\u002B;
			if (char.IsUpper(c))
			{
				TypeChar(c, aShiftDown: true);
			}
			else
			{
				TypeChar(c, aShiftDown: false);
			}
			if ((aInterval != 0 || naturalTyping) && num3 < aString.Length)
			{
				if (naturalTyping)
				{
					Sleep((int)num);
				}
				else
				{
					Sleep(aInterval);
				}
			}
		}
	}

	/// <summary>
	/// Method that sends inputs to type an array of VirtualKeyCodes.
	/// </summary>
	/// <param name="aVirtualKeyCodeArray"></param>
	/// <visibility>public</visibility>
	public void Type(params VirtualKeyCode[] aVirtualKeyCodeArray)
	{
		if (RunMode == 4)
		{
			return;
		}
		foreach (VirtualKeyCode aVirtualKeyCode in aVirtualKeyCodeArray)
		{
			PressKey(aVirtualKeyCode);
			ReleaseKey(aVirtualKeyCode);
		}
	}

	/// <summary>
	/// Method that sends inputs to type a char.
	/// </summary>
	/// <param name="aChar"></param>
	/// <param name="aShiftDown"></param>
	/// <visibility>public</visibility>
	public void TypeChar(char aChar, bool aShiftDown)
	{
		if (RunMode == 4)
		{
			return;
		}
		char key = char.ToLower(aChar);
		if (!VirtualKeyCodeDictionary.TryGetValue(key, out var value))
		{
			return;
		}
		if (aShiftDown)
		{
			PressKey(VirtualKeyCode.SHIFT);
		}
		PressKey(value);
		ReleaseKey(value);
		if (aShiftDown)
		{
			ReleaseKey(VirtualKeyCode.SHIFT);
		}
	}

	/// <summary>
	/// Method that simulates holding down of modifier key and pressing VirtualKeyCode.
	/// </summary>
	/// <param name="aModifierVirtualKeyCode"></param>
	/// <param name="aVirtualKeyCode"></param>
	/// <visibility>public</visibility>
	public void ModifiedKeyStroke(VirtualKeyCode aModifierVirtualKeyCode, VirtualKeyCode aVirtualKeyCode)
	{
		if (RunMode == 4)
		{
			return;
		}
		PressKey(aModifierVirtualKeyCode);
		PressKey(aVirtualKeyCode);
		ReleaseKey(aVirtualKeyCode);
		ReleaseKey(aModifierVirtualKeyCode);
	}

	/// <summary>
	/// Method that simulates holding down of modifier key and pressing VirtualKeyCode.
	/// </summary>
	/// <param name="aModifierVirtualKeyCode"></param>
	/// <param name="aVirtualKeyCodeArray"></param>
	/// <visibility>public</visibility>
	public void ModifiedKeyStroke(VirtualKeyCode aModifierVirtualKeyCode, params VirtualKeyCode[] aVirtualKeyCodeArray)
	{
		if (RunMode == 4)
		{
			return;
		}
		PressKey(aModifierVirtualKeyCode);
		foreach (VirtualKeyCode aVirtualKeyCode in aVirtualKeyCodeArray)
		{
			PressKey(aVirtualKeyCode);
			ReleaseKey(aVirtualKeyCode);
		}
		ReleaseKey(aModifierVirtualKeyCode);
	}

	/// <summary>
	/// Method that simulates holding down of two modifier keys and pressing VirtualKeyCode.
	/// </summary>
	/// <param name="aFirstModifierVirtualKeyCode"></param>
	/// <param name="aSecondModifierVirtualKeyCode"></param>
	/// <param name="aVirtualKeyCode"></param>
	/// <visibility>public</visibility>
	public void ModifiedKeyStroke(VirtualKeyCode aFirstModifierVirtualKeyCode, VirtualKeyCode aSecondModifierVirtualKeyCode, VirtualKeyCode aVirtualKeyCode)
	{
		if (RunMode == 4)
		{
			return;
		}
		PressKey(aFirstModifierVirtualKeyCode);
		PressKey(aSecondModifierVirtualKeyCode);
		PressKey(aVirtualKeyCode);
		ReleaseKey(aVirtualKeyCode);
		ReleaseKey(aSecondModifierVirtualKeyCode);
		ReleaseKey(aFirstModifierVirtualKeyCode);
	}

	/// <summary>
	/// Method that simulates holding down of two modifier keys and pressing VirtualKeyCodes.
	/// </summary>
	/// <param name="aFirstModifierVirtualKeyCode"></param>
	/// <param name="aSecondModifierVirtualKeyCode"></param>
	/// <param name="aVirtualKeyCodeArray"></param>
	/// <visibility>public</visibility>
	public void ModifiedKeyStroke(VirtualKeyCode aFirstModifierVirtualKeyCode, VirtualKeyCode aSecondModifierVirtualKeyCode, params VirtualKeyCode[] aVirtualKeyCodeArray)
	{
		if (RunMode == 4)
		{
			return;
		}
		PressKey(aFirstModifierVirtualKeyCode);
		PressKey(aSecondModifierVirtualKeyCode);
		foreach (VirtualKeyCode aVirtualKeyCode in aVirtualKeyCodeArray)
		{
			PressKey(aVirtualKeyCode);
			ReleaseKey(aVirtualKeyCode);
		}
		ReleaseKey(aSecondModifierVirtualKeyCode);
		ReleaseKey(aFirstModifierVirtualKeyCode);
	}

	/// <summary>
	/// Method that sends input to simulate a single key press event.
	/// </summary>
	/// <param name="aVirtualKeyCode"></param>
	/// <visibility>public</visibility>
	public void PressKey(VirtualKeyCode aVirtualKeyCode)
	{
		if (RunMode == 4)
		{
			return;
		}
		Input[] array = new Input[1];
		array[0].Type = 1u;
		array[0].Data.Keyboard.KeyCode = (ushort)aVirtualKeyCode;
		array[0].Data.Keyboard.Scan = 0;
		array[0].Data.Keyboard.Flags = 0u;
		array[0].Data.Keyboard.Time = 0u;
		array[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
		SendKeyEvent(array);
	}

	/// <summary>
	/// Method that sends input to simulate a single key release event.
	/// </summary>
	/// <param name="aVirtualKeyCode"></param>
	/// <visibility>public</visibility>
	public void ReleaseKey(VirtualKeyCode aVirtualKeyCode)
	{
		if (RunMode == 4)
		{
			return;
		}
		Input[] array = new Input[1];
		array[0].Type = 1u;
		array[0].Data.Keyboard.KeyCode = (ushort)aVirtualKeyCode;
		array[0].Data.Keyboard.Scan = 0;
		array[0].Data.Keyboard.Flags = 2u;
		array[0].Data.Keyboard.Time = 0u;
		array[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
		SendKeyEvent(array);
	}

	/// <summary>
	/// Method called to send the Input in using SendInput.
	/// </summary>
	/// <param name="input"></param>
	/// <visibility>private</visibility>
	private static void SendKeyEvent(Input[] input)
	{
		uint num = SafeNativeMethods.SendInput((uint)input.Length, input, Marshal.SizeOf(typeof(Input)));
	}
}
