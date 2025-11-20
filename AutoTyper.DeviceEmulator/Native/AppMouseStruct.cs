using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// The AppMouseStruct structure contains information about a application-level mouse input event.
/// </summary>
/// <remarks>
/// See full documentation at http://globalmousekeyhook.codeplex.com/wikipage?title=MouseStruct
/// </remarks>
[StructLayout(LayoutKind.Explicit)]
internal struct AppMouseStruct
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
	[FieldOffset(22)]
	public short MouseData;

	/// <summary>
	/// Converts the current <see cref="T:AutoTyper.DeviceEmulator.Native.AppMouseStruct" /> into a <see cref="T:AutoTyper.DeviceEmulator.Native.MouseStruct" />.
	/// </summary>
	/// <returns></returns>
	/// <remarks>
	/// The AppMouseStruct does not have a timestamp, thus one is generated at the time of this call.
	/// </remarks>
	public MouseStruct ToMouseStruct()
	{
		return new MouseStruct
		{
			Point = Point,
			MouseData = MouseData,
			Timestamp = Environment.TickCount
		};
	}
}
