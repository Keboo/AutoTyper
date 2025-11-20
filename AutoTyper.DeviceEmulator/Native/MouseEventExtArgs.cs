using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides extended data for System.Windows.Forms.MouseEventArgs.
/// </summary>
/// <remarks>
/// MouseEventExtArgs.cs class allows MouseEvents to be triggered.
/// </remarks>
/// <visibility>internal</visibility>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-30  AS00448  v0.00.04.014  Initial Version
/// 2015-05-12  AS00456  v0.00.04.020  Use Point struct from System.Drawing.Point
/// 2015-08-24  AS00512  v0.00.04.052  Modified summary and remarks to represent this class
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the class
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2015-11-23  AS00573  v1.00.01.010  Removed public property Point
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-08  AS00743  v1.00.03.032  Use IntPtr for wParam instead of int
/// 2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags, modified access modifiers of methods to internal
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
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
	/// True if event singnals mouse button down.
	/// </summary>
	/// <visibility>internal</visibility>
	internal bool IsMouseKeyDown { get; private set; }

	/// <summary>
	/// True if event singnals mouse button up.
	/// </summary>
	/// <visibility>internal</visibility>
	internal bool IsMouseKeyUp { get; private set; }

	/// <summary>
	/// The system tick count of when the event occured.
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
