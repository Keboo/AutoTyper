using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;

namespace AutoTyper.DeviceEmulator;

/// <summary>
/// Provides information about display screens. Replaces System.Windows.Forms.Screen.
/// </summary>
public class ScreenInfo
{
    /// <summary>Gets the device name associated with a display.</summary>
    public string DeviceName { get; private set; }

    /// <summary>Gets the bounds of the display.</summary>
    public Rectangle Bounds { get; private set; }

    /// <summary>Gets the working area of the display.</summary>
    public Rectangle WorkingArea { get; private set; }

    /// <summary>Gets a value indicating whether a particular display is the primary device.</summary>
    public bool Primary { get; private set; }

    private ScreenInfo(string deviceName, Rectangle bounds, Rectangle workingArea, bool primary)
    {
        DeviceName = deviceName;
        Bounds = bounds;
        WorkingArea = workingArea;
        Primary = primary;
    }

    /// <summary>Gets an array of all displays on the system.</summary>
    public static ScreenInfo[] AllScreens
    {
        get
        {
            var screens = new List<ScreenInfo>();
            EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero,
                delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData)
                {
                    var mi = new MONITORINFOEX();
                    mi.Size = Marshal.SizeOf(mi);
                    if (GetMonitorInfo(hMonitor, ref mi))
                    {
                        screens.Add(new ScreenInfo(
                            mi.DeviceName,
                            new Rectangle(mi.Monitor.Left, mi.Monitor.Top,
                                mi.Monitor.Right - mi.Monitor.Left,
                                mi.Monitor.Bottom - mi.Monitor.Top),
                            new Rectangle(mi.WorkArea.Left, mi.WorkArea.Top,
                                mi.WorkArea.Right - mi.WorkArea.Left,
                                mi.WorkArea.Bottom - mi.WorkArea.Top),
                            (mi.Flags & 1) != 0));
                    }
                    return true;
                }, IntPtr.Zero);
            return screens.ToArray();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct MONITORINFOEX
    {
        public int Size;
        public RECT Monitor;
        public RECT WorkArea;
        public uint Flags;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string DeviceName;
    }

    private delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData);

    [DllImport("user32.dll")]
    private static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, MonitorEnumDelegate lpfnEnum, IntPtr dwData);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFOEX lpmi);
}

/// <summary>
/// Provides system-related information. Replaces System.Windows.Forms.SystemInformation.
/// </summary>
public static class SystemInfo
{
    /// <summary>Gets the dimensions of the primary display screen in pixels.</summary>
    public static System.Drawing.Size PrimaryMonitorSize
    {
        get
        {
            return new System.Drawing.Size(
                (int)SystemParameters.PrimaryScreenWidth,
                (int)SystemParameters.PrimaryScreenHeight);
        }
    }

    /// <summary>Gets the maximum number of milliseconds that can elapse between a first click and a second click for the OS to consider the mouse action a double-click.</summary>
    public static int DoubleClickTime => GetDoubleClickTime();

    [DllImport("user32.dll")]
    private static extern int GetDoubleClickTime();
}

/// <summary>
/// Provides cursor position information. Replaces System.Windows.Forms.Cursor.
/// </summary>
public static class CursorHelper
{
    /// <summary>Gets or sets the cursor's position.</summary>
    public static System.Drawing.Point Position
    {
        get
        {
            GetCursorPos(out System.Drawing.Point point);
            return point;
        }
        set
        {
            SetCursorPos(value.X, value.Y);
        }
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorPos(out System.Drawing.Point lpPoint);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int x, int y);
}