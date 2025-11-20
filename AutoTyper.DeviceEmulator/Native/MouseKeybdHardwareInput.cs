using System.Runtime.InteropServices;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Provides combined/overlayed structure that includes Mouse, Keyboard and Hardware Input message data.
/// </summary>
/// <remarks>
/// MOUSEKEYBDHARDWAREINPUT is struct layout of Mouse, Keyboard and Hardware Input message data.
/// (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
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
/// 2015-11-04  AS00554  v1.00.00.005  Changed access modifier to internal for the struct
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-07-13  AS00693  v1.00.03.018  Modified summary and remarks
/// 2016-10-11  AS00746  v1.00.03.033  Renamed the class to MouseKeybdHardwareInput, and fields to CamelCasing
/// 2016-10-19  AS00754  v1.00.03.040  Modified access modifier to fields to internal
/// 2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// 2019-03-26  AS01161  v1.01.03.008  Resolve CA1823 by adding struct ToString override
/// </revisionhistory>
[StructLayout(LayoutKind.Explicit)]
internal struct MouseKeybdHardwareInput
{
	/// <summary>
	/// The <see cref="T:AutoTyper.DeviceEmulator.Native.MouseInput" /> definition.
	/// </summary>
	/// <visibility>internal</visibility>
	[FieldOffset(0)]
	internal MouseInput Mouse;

	/// <summary>
	/// The <see cref="T:AutoTyper.DeviceEmulator.Native.KeybdInput" /> definition.
	/// </summary>
	/// <visibility>internal</visibility>
	[FieldOffset(0)]
	internal KeybdInput Keyboard;

	/// <summary>
	/// The <see cref="T:AutoTyper.DeviceEmulator.Native.HardwareInput" /> definition.
	/// </summary>
	/// <visibility>internal</visibility>
	[FieldOffset(0)]
	internal HardwareInput Hardware;

	/// <summary>
	/// Provide string output of the <see cref="T:AutoTyper.DeviceEmulator.Native.MouseKeybdHardwareInput" /> content.
	/// </summary>
	/// <returns></returns>
	/// <visibility>public</visibility>
	public override string ToString()
	{
		return Mouse.ToString() + Keyboard.ToString() + Hardware;
	}
}
