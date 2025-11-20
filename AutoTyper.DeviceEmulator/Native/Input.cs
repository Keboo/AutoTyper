namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// The INPUT structure is used by SendInput to store information for synthesizing input events such as keystrokes,
/// mouse movement, and mouse clicks.
/// </summary>
/// <remarks>
/// <code>
/// C++
/// typedef struct tagINPUT {
///    DWORD type;
///    union {
///       MOUSEINPUT    mi;
///       KEYBDINPUT    ki;
///       HARDWAREINPUT hi;
///    };
/// } INPUT, *PINPUT;
/// </code>
/// (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
/// Declared in Winuser.h, include Windows.h
/// This structure contains information identical to that used in the parameter list of the keybd_event or 
/// mouse_event function.
/// Windows 2000/XP: INPUT_KEYBOARD supports nonkeyboard input methods, such as handwriting recognition or 
/// voice recognition, as if it were text input by using the KEYEVENTF_UNICODE flag. For more information, 
/// see the remarks section of KEYBDINPUT.
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
/// 2015-11-16  AS00565  v1.00.01.006  Modified the summary and remarks
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-11  AS00746  v1.00.03.033  Renamed the class INPUT to Input
/// 2016-10-19  AS00754  v1.00.03.040  Modified summary and remarks and  rest to follow Henooh Style Guidelines
/// 2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
internal struct Input
{
    /// <summary>
    /// Specifies the type of the input event. This member can be one of the following values. 
    /// <see cref="F:AutoTyper.DeviceEmulator.Native.InputType.Mouse" /> The event is a mouse event. Use the mi structure of the union.
    /// <see cref="F:AutoTyper.DeviceEmulator.Native.InputType.Keyboard" /> The event is a keyboard event. Use the ki structure of the union.
    /// <see cref="F:AutoTyper.DeviceEmulator.Native.InputType.Hardware" /> Windows 95/98/Me: The event is from input hardware other than a 
    /// keyboard or mouse. Use the hi structure of the union.
    /// </summary>
    /// <visibility>internal</visibility>
    internal uint Type;

    /// <summary>
    /// The data structure that contains information about the simulated Mouse, Keyboard or Hardware event.
    /// </summary>
    /// <visibility>internal</visibility>
    internal MouseKeybdHardwareInput Data;
}