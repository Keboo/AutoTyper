using System;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Contains information about a simulated mouse event.
/// </summary>
/// <remarks>
/// The MOUSEINPUT structure contains information about a simulated mouse event. 
/// (see: http://msdn.microsoft.com/en-us/library/ms646273(VS.85).aspx)
/// Declared in Winuser.h, include Windows.h
///
/// If the mouse has moved, indicated by MOUSEEVENTF_MOVE, dx and dy specify information about that movement. 
/// The information is specified as absolute or relative integer values. 
/// If MOUSEEVENTF_ABSOLUTE value is specified, dx and dy contain normalized absolute coordinates 
/// between 0 and 65,535. The event procedure maps these coordinates onto the display surface. Coordinate (0,0) 
/// maps onto the upper-left corner of the display surface; coordinate (65535,65535) maps onto the lower-right 
/// corner. In a multimonitor system, the coordinates map to the primary monitor. 
/// Windows 2000/XP: If MOUSEEVENTF_VIRTUALDESK is specified, the coordinates map to the entire virtual desktop.
/// If the MOUSEEVENTF_ABSOLUTE value is not specified, dx and dy specify movement relative to the previous 
/// mouse event (the last reported position). Positive values mean the mouse moved right (or down); 
/// negative values mean the mouse moved left (or up). 
/// Relative mouse motion is subject to the effects of the mouse speed and the two-mouse threshold values. 
/// A user sets these three values with the Pointer Speed slider of the Control Panel's Mouse Properties sheet. 
/// You can obtain and set these values using the SystemParametersInfo function. 
/// The system applies two tests to the specified relative mouse movement. If the specified distance along either 
/// the x or y axis is greater than the first mouse threshold value, and the mouse speed is not zero, the system 
/// doubles the distance. If the specified distance along either the x or y axis is greater than the second mouse 
/// threshold value, and the mouse speed is equal to two, the system doubles the distance that resulted from 
/// applying the first threshold test. It is thus possible for the system to multiply specified relative mouse 
/// movement along the x or y axis by up to four times.
/// </remarks>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2013-10-15  AS00122  v0.00.00.122  Initial Version
/// 2014-03-18  AS00203  v0.00.00.203  Moved to Henooh.Utility.Native Namespace
/// 2014-04-14  AS00230  v0.00.00.230  Moved to HenoohUtility as a Class Library Project (dll)
/// 2015-04-02  AS00420  v0.00.04.000  Moved to HenoohInputSimulator Project
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the struct
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-23  AS00573  v1.00.01.010  Modified summary and remarks
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-11  AS00746  v1.00.03.033  Renamed the class from MOUSEINPUT
/// 2016-10-20  AS00755  v1.00.03.041  Modified public fields to internal fields, Follow Henooh Style Guidelines
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-10-24  AS01242  v1.01.04.018  Resolve IDE0048 by Simplify the type name of fields
/// </revisionhistory>
internal struct MouseInput
{
    /// <summary>
    /// Specifies the absolute position of the mouse, or the amount of motion since the last mouse event was 
    /// generated, depending on the value of the dwFlags member. Absolute data is specified as the x coordinate 
    /// of the mouse; relative data is specified as the number of pixels moved. 
    /// </summary>
    internal int X;

    /// <summary>
    /// Specifies the absolute position of the mouse, or the amount of motion since the last mouse event was 
    /// generated, depending on the value of the dwFlags member. Absolute data is specified as the y coordinate 
    /// of the mouse; relative data is specified as the number of pixels moved. 
    /// </summary>
    internal int Y;

    /// <summary>
    /// If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. 
    /// A positive value indicates that the wheel was rotated forward, away from the user; a negative value 
    /// indicates that the wheel was rotated backward, toward the user. One wheel click is defined as 
    /// WHEEL_DELTA, which is 120. 
    /// Windows Vista: If dwFlags contains MOUSEEVENTF_HWHEEL, then dwData specifies the amount of wheel 
    /// movement. A positive value indicates that the wheel was rotated to the right; a negative value 
    /// indicates that the wheel was rotated to the left. One wheel click is defined as WHEEL_DELTA, 
    /// which is 120.
    /// Windows 2000/XP: IfdwFlags does not contain MOUSEEVENTF_WHEEL, MOUSEEVENTF_XDOWN, or MOUSEEVENTF_XUP, 
    /// then mouseData should be zero. 
    /// If dwFlags contains MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP, then mouseData specifies which X buttons 
    /// were pressed or released. This value may be any combination of the following flags. 
    /// </summary>
    internal uint MouseData;

    /// <summary>
    /// A set of bit flags that specify various aspects of mouse motion and button clicks. The bits in this 
    /// member can be any reasonable combination of the following values. 
    /// The bit flags that specify mouse button status are set to indicate changes in status, not ongoing 
    /// conditions. For example, if the left mouse button is pressed and held down, MOUSEEVENTF_LEFTDOWN is 
    /// set when the left button is first pressed, but not for subsequent motions. Similarly, 
    /// MOUSEEVENTF_LEFTUP is set only when the button is first released. 
    /// You cannot specify both the MOUSEEVENTF_WHEEL flag and either MOUSEEVENTF_XDOWN or MOUSEEVENTF_XUP 
    /// flags simultaneously in the dwFlags parameter, because they both require use of the mouseData field. 
    /// </summary>
    internal uint Flags;

    /// <summary>
    /// Time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own 
    /// time stamp. 
    /// </summary>
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
    internal uint Time;

    /// <summary>
    /// Specifies an additional value associated with the mouse event. An application calls GetMessageExtraInfo 
    /// to obtain this extra information. 
    /// </summary>
    internal IntPtr ExtraInfo;
#pragma warning restore CS0649
}