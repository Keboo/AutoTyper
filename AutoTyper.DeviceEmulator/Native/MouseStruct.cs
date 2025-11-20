using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides extended data for the MouseClickExt and MouseMoveExt events. 
/// </summary>
/// <remarks>
/// MouseStruct contains structure of mouse
/// </remarks>
[StructLayout(LayoutKind.Explicit)]
internal struct MouseStruct
{
    /// <summary>
    /// Specifies a Point structure that contains the X- and Y-coordinates of the cursor, in screen coordinates. 
    /// </summary>
    [FieldOffset(0)]
    public Point Point;

    /// <summary>
    /// Specifies information associated with the message.
    /// </summary>
    /// <remarks>
    /// The possible values are:
    /// <list type="bullet">
    /// <item>
    /// <description>0 - No Information</description>
    /// </item>
    /// <item>
    /// <description>1 - X-Button1 Click</description>
    /// </item>
    /// <item>
    /// <description>2 - X-Button2 Click</description>
    /// </item>
    /// <item>
    /// <description>120 - Mouse Scroll Away from User</description>
    /// </item>
    /// <item>
    /// <description>-120 - Mouse Scroll Toward User</description>
    /// </item>
    /// </list>
    /// </remarks>
    [FieldOffset(10)]
    public short MouseData;

    /// <summary>
    /// Returns a Timestamp associated with the input, in System Ticks.
    /// </summary>
    [FieldOffset(16)]
    public int Timestamp;
}