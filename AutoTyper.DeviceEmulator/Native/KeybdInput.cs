using System;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// The KEYBDINPUT structure contains information about a simulated keyboard event.  
/// </summary>
/// <remarks>
/// C++
/// typedef struct tagKEYBDINPUT {
///    WORD wVk;
///    WORD wScan;
///    DWORD dwFlags;
///    DWORD time;
///    ULONG_PTR dwExtraInfo;
/// } KEYBDINPUT, *PKEYBDINPUT;
///
/// (see: http://msdn.microsoft.com/en-us/library/ms646271(VS.85).aspx)
/// Declared in Winuser.h, include Windows.h
/// Windows 2000/XP: INPUT_KEYBOARD supports nonkeyboard-input methods
/// —such as handwriting recognition or voice recognition—as if it were text input by using the 
/// KEYEVENTF_UNICODE flag. If KEYEVENTF_UNICODE is specified, SendInput sends a WM_KEYDOWN or WM_KEYUP 
/// message to the foreground thread's message queue with wParam equal to VK_PACKET. Once GetMessage or 
/// PeekMessage obtains this message, passing the message to TranslateMessage posts a WM_CHAR message with 
/// the Unicode character originally specified by wScan. This Unicode character will automatically be converted 
/// to the appropriate ANSI value if it is posted to an ANSI window.
/// Windows 2000/XP: Set the KEYEVENTF_SCANCODE flag to define keyboard input in terms of the scan code. 
/// This is useful to simulate a physical keystroke regardless of which keyboard is currently being used. 
/// The virtual key value of a key may alter depending on the current keyboard layout or what other keys 
/// were pressed, but the scan code will always be the same.
/// </remarks>
/// <visibility>internal</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2013-10-15  AS00122  v0.00.00.122  Initial Version
/// 2014-03-18  AS00203  v0.00.00.203  Moved to Henooh.Utility.Native Namespace
/// 2014-04-14  AS00230  v0.00.00.230  Moved to HenoohUtility as a Class Library Project (dll)
/// 2015-04-02  AS00420  v0.00.04.000  Moved to HenoohInputSimulator Project
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-19  AS00569  v1.00.01.009  Modified summary and remarks
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-01  AS00736  v1.00.03.030  Modified access modifer to this struct as internal along with its fields
/// 2016-10-11  AS00746  v1.00.03.033  Renamed the class from KEYBDINPUT
/// 2016-10-18  AS00753  v1.00.03.039  Added visibilty xml tags, follow Henooh Style Guidelines for 120 char 
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
internal struct KeybdInput
{
	/// <summary>
	/// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
	/// The Winuser.h header file provides macro definitions (VK_*) for each value. 
	/// If the dwFlags member specifies KEYEVENTF_UNICODE, wVk must be 0. 
	/// </summary>
	/// <visibility>internal</visibility>
	internal ushort KeyCode;

	/// <summary>
	/// Specifies a hardware scan code for the key. If dwFlags specifies KEYEVENTF_UNICODE, 
	/// wScan specifies a Unicode character which is to be sent to the foreground application. 
	/// </summary>
	/// <visibility>internal</visibility>
	internal ushort Scan;

	/// <summary>
	/// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
	/// KEYEVENTF_EXTENDEDKEY - If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).
	/// KEYEVENTF_KEYUP - If specified, the key is being released. If not specified, the key is being pressed.
	/// KEYEVENTF_SCANCODE - If specified, wScan identifies the key and wVk is ignored. 
	/// KEYEVENTF_UNICODE - Windows 2000/XP: If specified, the system synthesizes a VK_PACKET keystroke. The wVk 
	/// parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. For more information, 
	/// see the Remarks section. 
	/// </summary>
	/// <visibility>internal</visibility>
	internal uint Flags;

	/// <summary>
	/// Time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its 
	/// own time stamp. 
	/// </summary>
	/// <visibility>internal</visibility>
	internal uint Time;

	/// <summary>
	/// Specifies an additional value associated with the keystroke. Use the GetMessageExtraInfo function to 
	/// obtain this information. 
	/// </summary>
	/// <visibility>internal</visibility>
	internal IntPtr ExtraInfo;
}
