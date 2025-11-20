using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides extended data for System.Windows.Forms.MouseEventArgs.
/// </summary>
/// <remarks>
/// MouseEventExtArgs.cs class allows MouseEvents to be triggered.
/// </remarks>
/// <visibility>internal</visibility>
internal class MouseEventExtArgs : MouseEventArgs
{
    /// <summary>
    /// Set this property to <b>true</b> inside your event handler to prevent further processing of the event in other applications.
    /// </summary>
    /// <visibility>internal</visibility>
    internal bool Handled { get; set; }

    /// <summary>
    /// True if event contains information about wheel scroll.
    /// </summary>
    /// <visibility>internal</visibility>
    internal bool WheelScrolled => base.Delta != 0;

    /// <summary>
    /// True if event signals a click. False if it was only a move or wheel scroll.
    /// </summary>
    /// <visibility>internal</visibility>
    internal bool Clicked => base.Clicks > 0;

    /// <summary>
    /// True if event signals mouse button down.
    /// </summary>
    /// <visibility>internal</visibility>
    internal bool IsMouseKeyDown { get; private set; }

    /// <summary>
    /// True if event signals mouse button up.
    /// </summary>
    /// <visibility>internal</visibility>
    internal bool IsMouseKeyUp { get; private set; }

    /// <summary>
    /// The system tick count of when the event occurred.
    /// </summary>
    /// <visibility>internal</visibility>
    internal int Timestamp { get; private set; }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.MouseEventExtArgs" /> from Windows Message parameters.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <param name="isGlobal">Specifies if the hook is local or global.</param>
    /// <returns>A new MouseEventExtArgs object.</returns>
    /// <visibility>internal</visibility>
    internal static MouseEventExtArgs FromRawData(IntPtr wParam, IntPtr lParam, bool isGlobal)
    {
        return isGlobal ? FromRawDataGlobal(wParam, lParam) : FromRawDataApp(wParam, lParam);
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.MouseEventExtArgs" /> from Windows Message parameters, 
    /// based upon a local application hook.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <returns>A new MouseEventExtArgs object.</returns>
    /// <visibility>internal</visibility>
    private static MouseEventExtArgs FromRawDataApp(IntPtr wParam, IntPtr lParam)
    {
        return FromRawDataUniversal(wParam, ((AppMouseStruct)Marshal.PtrToStructure(lParam, typeof(AppMouseStruct))).ToMouseStruct());
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.MouseEventExtArgs" /> from Windows Message parameters, 
    /// based upon a system-wide global hook.
    /// </summary>
    /// <param name="wParam">The first Windows Message parameter.</param>
    /// <param name="lParam">The second Windows Message parameter.</param>
    /// <returns>A new MouseEventExtArgs object.</returns>
    /// <visibility>internal</visibility>
    internal static MouseEventExtArgs FromRawDataGlobal(IntPtr wParam, IntPtr lParam)
    {
        MouseStruct mouseInfo = (MouseStruct)Marshal.PtrToStructure(lParam, typeof(MouseStruct));
        return FromRawDataUniversal(wParam, mouseInfo);
    }

    /// <summary>
    /// Creates <see cref="T:AutoTyper.DeviceEmulator.Native.MouseEventExtArgs" /> from relevant mouse data. 
    /// </summary>
    /// <param name="wParam">First Windows Message parameter.</param>
    /// <param name="mouseInfo">A MouseStruct containing information from which to contruct MouseEventExtArgs.</param>
    /// <returns>A new MouseEventExtArgs object.</returns>
    /// <visibility>private</visibility>
    private static MouseEventExtArgs FromRawDataUniversal(IntPtr wParam, MouseStruct mouseInfo)
    {
        MouseButtons buttons = MouseButtons.None;
        short num = 0;
        int num2 = 0;
        bool isMouseKeyDown = false;
        bool isMouseKeyUp = false;
        switch ((int)wParam)
        {
            case 513:
                isMouseKeyDown = true;
                buttons = MouseButtons.Left;
                num2 = 1;
                break;
            case 514:
                isMouseKeyUp = true;
                buttons = MouseButtons.Left;
                num2 = 1;
                break;
            case 515:
                isMouseKeyDown = true;
                buttons = MouseButtons.Left;
                num2 = 2;
                break;
            case 516:
                isMouseKeyDown = true;
                buttons = MouseButtons.Right;
                num2 = 1;
                break;
            case 517:
                isMouseKeyUp = true;
                buttons = MouseButtons.Right;
                num2 = 1;
                break;
            case 518:
                isMouseKeyDown = true;
                buttons = MouseButtons.Right;
                num2 = 2;
                break;
            case 519:
                isMouseKeyDown = true;
                buttons = MouseButtons.Middle;
                num2 = 1;
                break;
            case 520:
                isMouseKeyUp = true;
                buttons = MouseButtons.Middle;
                num2 = 1;
                break;
            case 521:
                isMouseKeyDown = true;
                buttons = MouseButtons.Middle;
                num2 = 2;
                break;
            case 522:
                num = mouseInfo.MouseData;
                break;
            case 523:
                buttons = ((mouseInfo.MouseData == 1) ? MouseButtons.XButton1 : MouseButtons.XButton2);
                isMouseKeyDown = true;
                num2 = 1;
                break;
            case 524:
                buttons = ((mouseInfo.MouseData == 1) ? MouseButtons.XButton1 : MouseButtons.XButton2);
                isMouseKeyUp = true;
                num2 = 1;
                break;
            case 525:
                isMouseKeyDown = true;
                buttons = ((mouseInfo.MouseData == 1) ? MouseButtons.XButton1 : MouseButtons.XButton2);
                num2 = 2;
                break;
        }
        return new MouseEventExtArgs(buttons, num2, mouseInfo.Point, num, mouseInfo.Timestamp, isMouseKeyDown, isMouseKeyUp);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:AutoTyper.DeviceEmulator.Native.MouseEventExtArgs" /> class. 
    /// </summary>
    /// <param name="buttons">One of the MouseButtons values indicating which mouse button was pressed.</param>
    /// <param name="clicks">The number of times a mouse button was pressed.</param>
    /// <param name="point">The x and y -coordinate of a mouse click, in pixels.</param>
    /// <param name="delta">A signed count of the number of detents the wheel has rotated.</param>
    /// <param name="timestamp">The system tick count when the event occured.</param>
    /// <param name="isMouseKeyDown">True if event singnals mouse button down.</param>
    /// <param name="isMouseKeyUp">True if event singnals mouse button up.</param>
    /// <visibility>internal</visibility>
    internal MouseEventExtArgs(MouseButtons buttons, int clicks, Point point, int delta, int timestamp, bool isMouseKeyDown, bool isMouseKeyUp)
        : base(buttons, clicks, point.X, point.Y, delta)
    {
        IsMouseKeyDown = isMouseKeyDown;
        IsMouseKeyUp = isMouseKeyUp;
        Timestamp = timestamp;
    }

    internal MouseEventExtArgs ToDoubleClickEventArgs()
    {
        return new MouseEventExtArgs(base.Button, 2, new Point(base.X, base.Y), base.Delta, Timestamp, IsMouseKeyDown, IsMouseKeyUp);
    }
}