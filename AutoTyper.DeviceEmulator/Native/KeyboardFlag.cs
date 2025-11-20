using System;

namespace AutoTyper.DeviceEmulator.Native;

/// <summary>
/// Specifies various aspects of a keystroke. This member can be certain combinations of the following values.
/// </summary>
/// <remarks>
/// KeyboardFlag is internal enumeration exclusively used by KeyboardControlller.
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
/// 2015-11-19  AS00568  v1.00.01.009  Changed the access modifier for KeyboardFlag to internal
/// 2016-04-04  AS00663  v1.00.03.015  Replaced revisionhistory from CR# to AS#
/// 2016-10-18  AS00753  v1.00.03.039  Added visibility xml tag
/// 2018-03-26  AS01034  v1.01.02.002  Rename namespace from HenoohDeviceEmulator to Henooh.DeviceEmulator
/// </revisionhistory>
[Flags]
internal enum KeyboardFlag : uint
{
    /// <summary>
    /// KEYEVENTF_EXTENDEDKEY = 0x0001 (If specified, the scan code was preceded by a prefix byte that has the value 0xE0 (224).)
    /// </summary>
    ExtendedKey = 1u,
    /// <summary>
    /// KEYEVENTF_KEYUP = 0x0002 (If specified, the key is being released. If not specified, the key is being pressed.)
    /// </summary>
    KeyUp = 2u,
    /// <summary>
    /// KEYEVENTF_UNICODE = 0x0004 (If specified, wScan identifies the key and wVk is ignored.)
    /// </summary>
    Unicode = 4u,
    /// <summary>
    /// KEYEVENTF_SCANCODE = 0x0008 (Windows 2000/XP: If specified, the system synthesizes a VK_PACKET keystroke. 
    /// The wVk parameter must be zero. This flag can only be combined with the KEYEVENTF_KEYUP flag. 
    /// For more information, see the Remarks section.)
    /// </summary>
    ScanCode = 8u
}