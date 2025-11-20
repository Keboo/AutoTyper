namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Specifies the type of the input event. This member can be one of the following values. 
/// </summary>
/// <remarks>
/// Contains enumeration of Hardware devices, Mouse as 0, Keyboard as 1.
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
/// 2015-11-05  AS00555  v1.00.00.006  Renamed Namespace from HenoohInputSimulator to HenoohDeviceEmulator
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-07-13  AS00693  v1.00.03.018  Modified remarks
/// 2016-10-11  AS00746  v1.00.03.033  Modified access modifier to internal from public
/// 2016-10-20  AS00755  v1.00.03.041  Added visibility xml tags
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
internal enum InputType : uint
{
    /// <summary>
    /// INPUT_MOUSE = 0x00 (The event is a mouse event. Use the mi structure of the union.)
    /// </summary>
    Mouse,
    /// <summary>
    /// INPUT_KEYBOARD = 0x01 (The event is a keyboard event. Use the ki structure of the union.)
    /// </summary>
    Keyboard,
    /// <summary>
    /// INPUT_HARDWARE = 0x02 (Windows 95/98/Me: The event is from input hardware other than a keyboard or mouse. Use the hi structure of the union.)
    /// </summary>
    Hardware
}