using System.Drawing;
using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides extended data for the MouseClickExt and MouseMoveExt events. 
/// </summary>
/// <remarks>
/// MouseStruct contains structure of mouse
/// </remarks>
/// <revisionhistory>
/// YYYY-MM-DD  AS#####  v#.##.##.###  Change Description
/// ==========  =======  ============  ============================================================================
/// 2015-04-30  AS00448  v0.00.04.014  Initial Version
/// 2015-05-12  AS00456  v0.00.04.020  Use Point struct from System.Drawing.Point
/// 2015-08-18  AS00509  v0.00.04.050  Correct File name under remark as MouseStructures
/// 2015-11-02  AS00552  v1.00.00.003  Use the new commenting that works with HenoohDocumentationGenerator
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-07-13  AS00693  v1.00.03.018  Renamed the file to match the struct name
/// 2016-11-26  AS00781  v1.00.05.002  Removed unused using directives
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-03-11  AS01156  v1.01.03.007  Resolve IDE0017, Object initialization can be simplified
/// 2019-10-24  AS01242  v1.01.04.018  Resolve IDE0048 by Simplify the type name of fields
/// </revisionhistory>
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
