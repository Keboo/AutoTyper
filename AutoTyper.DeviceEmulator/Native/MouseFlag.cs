using System;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Contains information about a simulated mouse event.
/// </summary>
/// <remarks>
/// The set of MouseFlags for use in the Flags property of the <see cref="T:AutoTyper.DeviceEmulator.Native.MouseInput" /> structure. 
/// (See: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
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
/// 2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the class
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-23  AS00573  v1.00.01.010  Modified summary and remarks section
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-11  AS00746  v1.00.03.033  Renamed the class from MOUSEINPUT under cref in remarks
/// 2016-10-19  AS00754  v1.00.03.040  Added visibility xml tag
/// 2017-02-25  AS00842  v1.00.06.011  Follow Henooh Style Guidelines to not exceed 120 characters
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
[Flags]
internal enum MouseFlag : uint
{
    /// <summary>
    /// Specifies that movement occurred.
    /// </summary>
    Move = 1u,
    /// <summary>
    /// Specifies that the left button was pressed.
    /// </summary>
    LeftDown = 2u,
    /// <summary>
    /// Specifies that the left button was released.
    /// </summary>
    LeftUp = 4u,
    /// <summary>
    /// Specifies that the right button was pressed.
    /// </summary>
    RightDown = 8u,
    /// <summary>
    /// Specifies that the right button was released.
    /// </summary>
    RightUp = 0x10u,
    /// <summary>
    /// Specifies that the middle button was pressed.
    /// </summary>
    MiddleDown = 0x20u,
    /// <summary>
    /// Specifies that the middle button was released.
    /// </summary>
    MiddleUp = 0x40u,
    /// <summary>
    /// Windows 2000/XP: Specifies that an X button was pressed.
    /// </summary>
    XDown = 0x80u,
    /// <summary>
    /// Windows 2000/XP: Specifies that an X button was released.
    /// </summary>
    XUp = 0x100u,
    /// <summary>
    /// Windows NT/2000/XP: Specifies that the wheel was moved, if the mouse has a wheel.
    /// The amount of movement is specified in mouseData. 
    /// </summary>
    VerticalWheel = 0x800u,
    /// <summary>
    /// Specifies that the wheel was moved horizontally, if the mouse has a wheel.
    /// The amount of movement is specified in mouseData. Windows 2000/XP:  Not supported.
    /// </summary>
    HorizontalWheel = 0x1000u,
    /// <summary>
    /// Windows 2000/XP: Maps coordinates to the entire desktop. Must be used with MOUSEEVENTF_ABSOLUTE.
    /// </summary>
    VirtualDesk = 0x4000u,
    /// <summary>
    /// Specifies that the dx and dy members contain normalized absolute coordinates.
    /// If the flag is not set, dxand dy contain relative data
    /// (the change in position since the last reported position).
    /// This flag can be set, or not set, regardless of what kind of mouse or other pointing device,
    /// if any, is connected to the system. For further information about relative mouse motion,
    /// see the following Remarks section.
    /// </summary>
    Absolute = 0x8000u
}