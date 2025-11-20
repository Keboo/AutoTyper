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